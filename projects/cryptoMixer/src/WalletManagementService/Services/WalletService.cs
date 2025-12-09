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
    private ExtPubKey? _masterPub;
    private readonly Network _network;
        private readonly IConfiguration _config;

    public WalletService(IVaultService vaultService, IConfiguration config)
    {
        _vaultService = vaultService;
        _config = config;
        // Network selection via config Wms:Network = "main" or "testnet". Default: main.
        var net = config["Wms:Network"] ?? config["WMS_NETWORK"] ?? "main";
        _network = net.Equals("testnet", StringComparison.OrdinalIgnoreCase) ? Network.TestNet : Network.Main;
    }

    private async Task EnsureMasterKeyLoadedAsync()
    {
        if (_masterKey is not null || _masterPub is not null) return;

        if (_vaultService.Disabled)
        {
            _masterKey = await _vaultService.GetMasterKeyAsync();
        }
        else
        {
            _masterPub = await _vaultService.GetMasterXPubAsync();
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
            PubKey pubKey;
            if (_masterPub is not null)
            {
                pubKey = _masterPub.Derive(keyPath).PubKey;
            }
            else
            {
                var neuter = _masterKey!.Neuter();
                pubKey = neuter.Derive(keyPath).PubKey;
            }
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

            // Harden sensitive exposure: only return mnemonic/xprv when explicitly allowed.
            // Allowed when ASPNETCORE_ENVIRONMENT=Development OR Wms:ExposeSensitive=true
            var env = _config["ASPNETCORE_ENVIRONMENT"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var expose = string.Equals(_config["Wms:ExposeSensitive"], "true", StringComparison.OrdinalIgnoreCase);
            var allowSensitive = string.Equals(env, "Development", StringComparison.OrdinalIgnoreCase) || expose;

            var response = new CreateWalletResponse(
                Mnemonic: allowSensitive ? mnemonic.ToString() : "",
                XPrv: allowSensitive ? extKey.ToString(useNetwork) : "",
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

            // Prepare coins for verification
            var coins = new List<Coin>();

            foreach (var input in request.InputsToSign)
            {
                if (input.InputIndex < 0 || input.InputIndex >= tx.Inputs.Count)
                    return Result<SignatureResponse>.Failure($"Invalid InputIndex: {input.InputIndex}");

                if (string.IsNullOrWhiteSpace(input.ScriptPubKeyHex))
                    return Result<SignatureResponse>.Failure("ScriptPubKeyHex is required for each input.");

                if (input.AmountSatoshis < 0)
                    return Result<SignatureResponse>.Failure("AmountSatoshis must be >= 0 for each input.");

                var prevOut = tx.Inputs[input.InputIndex].PrevOut;
                var prevTxOut = new TxOut(Money.Satoshis(input.AmountSatoshis), Script.FromHex(input.ScriptPubKeyHex));
                coins.Add(new Coin(prevOut, prevTxOut));
            }

            // If Vault is disabled, keep existing local signing behavior
            if (_vaultService.Disabled)
            {
                var keys = new List<Key>();
                foreach (var input in request.InputsToSign)
                {
                    if (string.IsNullOrWhiteSpace(input.DerivationPath))
                        return Result<SignatureResponse>.Failure("DerivationPath is required for each input.");

                    var keyPath = new KeyPath(input.DerivationPath);
                    var derived = _masterKey!.Derive(keyPath);
                    keys.Add(derived.PrivateKey);
                }

                var builder = _network.CreateTransactionBuilder();
                builder.AddCoins(coins.ToArray());
                builder.AddKeys(keys.ToArray());
                var signed = builder.SignTransaction(tx);
                return Result<SignatureResponse>.Success(new SignatureResponse(signed.ToHex()));
            }

            // Vault Transit signing path (vault enabled)
            if (_masterPub is null)
                return Result<SignatureResponse>.Failure("Master public key not available for vault signing.");

            // For each input compute the appropriate sighash, call Vault Transit, and attach signature
            for (int i = 0; i < request.InputsToSign.Count; i++)
            {
                var input = request.InputsToSign[i];
                var script = Script.FromHex(input.ScriptPubKeyHex);
                var pub = _masterPub.Derive(new KeyPath(input.DerivationPath)).PubKey;
                var prevTxOut = new TxOut(Money.Satoshis(input.AmountSatoshis), script);

                // Determine script type roughly
                var scriptBytes = script.ToBytes();
                bool isNativeSegwit = scriptBytes.Length >= 2 && scriptBytes[0] == 0x00;
                bool isP2PKH = scriptBytes.Length >= 1 && scriptBytes[0] == 0x76;
                bool isP2SH = scriptBytes.Length >= 1 && scriptBytes[0] == 0xa9;

                byte[] sighashBytes;
                if (isNativeSegwit || isP2SH)
                {
                    // For native segwit or nested P2SH-P2WPKH we use the pubkey-hash based script as scriptCode
                    var scriptCode = pub.Hash.ScriptPubKey; // P2PKH script for signature hashing
                    var sh = tx.GetSignatureHash(scriptCode, input.InputIndex, SigHash.All, prevTxOut);
                    sighashBytes = sh.ToBytes();
                }
                else if (isP2PKH)
                {
                    var scriptCode = pub.Hash.ScriptPubKey;
                    var sh = tx.GetSignatureHash(scriptCode, input.InputIndex, SigHash.All);
                    sighashBytes = sh.ToBytes();
                }
                else
                {
                    return Result<SignatureResponse>.Failure($"Unsupported script type for input {input.InputIndex}");
                }

                var keyName = input.DerivationPath.Replace('/', '-').Replace("m", "key");
                var vaultSigB64 = await _vaultService.SignTransactionHashAsync(keyName, Convert.ToBase64String(sighashBytes));
                var sigBytes = Convert.FromBase64String(vaultSigB64);
                // append sighash byte (SIGHASH_ALL)
                var sigWithHash = sigBytes.Concat(new byte[] { (byte)SigHash.All }).ToArray();

                // attach signature to tx
                if (isNativeSegwit)
                {
                    tx.Inputs[input.InputIndex].WitScript = new WitScript(new[] { sigWithHash, pub.ToBytes() });
                }
                else if (isP2SH)
                {
                    // nested P2SH-P2WPKH: set scriptSig to push the redeemScript (witness program), and set witness
                    try
                    {
                        var redeemScript = PayToWitPubKeyHashTemplate.Instance.GenerateScriptPubKey(pub);
                        tx.Inputs[input.InputIndex].ScriptSig = new Script(Op.GetPushOp(redeemScript.ToBytes()));
                        tx.Inputs[input.InputIndex].WitScript = new WitScript(new[] { sigWithHash, pub.ToBytes() });
                    }
                    catch
                    {
                        return Result<SignatureResponse>.Failure($"Failed to assemble nested P2SH witness for input {input.InputIndex}");
                    }
                }
                else // P2PKH
                {
                    tx.Inputs[input.InputIndex].ScriptSig = new Script(Op.GetPushOp(sigWithHash), Op.GetPushOp(pub.ToBytes()));
                }
            }

            // Verify the transaction signatures using TransactionBuilder
            var verifier = _network.CreateTransactionBuilder();
            verifier.AddCoins(coins.ToArray());
            var isValid = verifier.Verify(tx);
            if (!isValid)
            {
                return Result<SignatureResponse>.Failure("Transaction verification failed after assembling signatures.");
            }

            return Result<SignatureResponse>.Success(new SignatureResponse(tx.ToHex()));
        }
        catch (Exception ex)
        {
            return Result<SignatureResponse>.Failure($"Failed to sign transaction: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
