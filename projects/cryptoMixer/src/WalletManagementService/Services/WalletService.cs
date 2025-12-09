using CryptoMixer.Shared;
using CryptoMixer.WalletManagementService.Models;
using NBitcoin;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CryptoMixer.WalletManagementService.Services;

/// <summary>
/// Logique métier du WalletManagementService.
/// Génération d'adresses HD et délégation de la signature à Vault.
/// </summary>
public class WalletService : IWalletService
{
    private readonly IVaultService _vaultService;
    private ExtKey? _masterKey;
    private readonly Network _network;

    public WalletService(IVaultService vaultService, IConfiguration config)
    {
        _vaultService = vaultService;
        // Network selection via config Wms:Network = "main" or "testnet". Default: main.
        var net = config["Wms:Network"] ?? config["WMS_NETWORK"] ?? "main";
        _network = net.Equals("testnet", StringComparison.OrdinalIgnoreCase) ? Network.TestNet : Network.Main;
    }

    private async Task EnsureMasterKeyLoadedAsync()
    {
        if (_masterKey is null)
        {
            // Mode normal : on laisse Vault fournir la master key HD.
            _masterKey = await _vaultService.GetMasterKeyAsync();
        }
    }

    public Result<AddressResponse> GenerateNewAddress(AddressRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.DerivationPath))
        {
            return Result<AddressResponse>.Failure("DerivationPath is required.");
        }

        try
        {
            // Utilisation de la clé publique étendue pour dériver une adresse.
            EnsureMasterKeyLoadedAsync().GetAwaiter().GetResult();
            var keyPath = new KeyPath(request.DerivationPath);
            var neuter = _masterKey!.Neuter();
            var pubKey = neuter.Derive(keyPath).PubKey;
            var address = pubKey.GetAddress(ScriptPubKeyType.Segwit, _network);

            return Result<AddressResponse>.Success(new AddressResponse(address.ToString(), request.DerivationPath));
        }
        catch (Exception ex)
        {
            return Result<AddressResponse>.Failure($"Failed to generate address: {ex.GetType().Name} - {ex.Message}");
        }
    }

    public async Task<Result<CreateWalletResponse>> CreateWalletAsync(string? network = null, bool importToVaultIfDisabled = true)
    {
        try
        {
            // Choose network for creation (allows creating testnet/mainnet wallets on demand)
            var useNetwork = _network;
            if (!string.IsNullOrWhiteSpace(network))
            {
                useNetwork = network.Equals("testnet", StringComparison.OrdinalIgnoreCase) ? Network.TestNet : Network.Main;
            }

            // Generate mnemonic and ext key
            var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            var extKey = mnemonic.DeriveExtKey();

            // Derive a first address (use a simple path compatible with GenerateNewAddress usage)
            var neuter = extKey.Neuter();
            var firstPath = new KeyPath("m/0/0/0");
            var pubKey = neuter.Derive(firstPath).PubKey;
            var firstAddress = pubKey.GetAddress(ScriptPubKeyType.Segwit, useNetwork).ToString();

            // If VaultService supports setting the master key (disabled mode), import it there so subsequent calls use it
            try
            {
                await _vaultService.SetMasterKeyAsync(extKey);
            }
            catch
            {
                // Ignore: non-critical if underlying Vault is read-only or remote
            }

            var response = new CreateWalletResponse(
                Mnemonic: mnemonic.ToString(),
                XPrv: extKey.ToString(useNetwork),
                XPub: extKey.Neuter().ToString(useNetwork),
                FirstAddress: firstAddress,
                Network: useNetwork == Network.TestNet ? "testnet" : "main"
            );

            return Result<CreateWalletResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<CreateWalletResponse>.Failure($"Failed to create wallet: {ex.Message}");
        }
    }

    public async Task<Result<SignatureResponse>> SignTransactionAsync(SignatureRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UnsignedTransactionHex))
        {
            return Result<SignatureResponse>.Failure("UnsignedTransactionHex is required.");
        }

        if (request.InputsToSign is null || request.InputsToSign.Count == 0)
        {
            return Result<SignatureResponse>.Failure("At least one input to sign is required.");
        }

        try
        {
            await EnsureMasterKeyLoadedAsync();

            var tx = Transaction.Parse(request.UnsignedTransactionHex, _network);

            // Prepare coins and keys for signing
            var coins = new List<Coin>();
            var keys = new List<Key>();

            foreach (var input in request.InputsToSign)
            {
                if (input.InputIndex < 0 || input.InputIndex >= tx.Inputs.Count)
                    return Result<SignatureResponse>.Failure($"Invalid InputIndex: {input.InputIndex}");

                if (string.IsNullOrWhiteSpace(input.ScriptPubKeyHex))
                    return Result<SignatureResponse>.Failure("ScriptPubKeyHex is required for each input.");

                if (input.AmountSatoshis < 0)
                    return Result<SignatureResponse>.Failure("AmountSatoshis must be >= 0 for each input.");

                // Build the TxOut corresponding to the previous output we are signing
                var prevOut = tx.Inputs[input.InputIndex].PrevOut;
                var prevTxOut = new TxOut(Money.Satoshis(input.AmountSatoshis), Script.FromHex(input.ScriptPubKeyHex));

                // Create a Coin for signing. Use a plain Coin - TransactionBuilder will handle script types.
                Coin baseCoin = new Coin(prevOut, prevTxOut);
                coins.Add(baseCoin);

                // Derive the private key for this input from the master key
                if (string.IsNullOrWhiteSpace(input.DerivationPath))
                    return Result<SignatureResponse>.Failure("DerivationPath is required for each input.");

                var keyPath = new KeyPath(input.DerivationPath);
                var derived = _masterKey!.Derive(keyPath);
                var priv = derived.PrivateKey;
                keys.Add(priv);
            }

            // Use TransactionBuilder (created from the Network) to sign the transaction
            var builder = _network.CreateTransactionBuilder();
            builder.AddCoins(coins.ToArray());
            builder.AddKeys(keys.ToArray());

            var signed = builder.SignTransaction(tx);

            return Result<SignatureResponse>.Success(new SignatureResponse(signed.ToHex()));
        }
        catch (Exception ex)
        {
            return Result<SignatureResponse>.Failure($"Failed to sign transaction: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
