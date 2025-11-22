using CryptoMixer.Shared;
using CryptoMixer.WalletManagementService.Models;
using NBitcoin;
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
    private readonly Network _network = Network.Main; // TODO: rendre configurable (mainnet / testnet)

    public WalletService(IVaultService vaultService)
    {
        _vaultService = vaultService;
        // Éviter .Wait() dans le constructeur : initialisation paresseuse.
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

            // La partie signature réelle n'est pas encore implémentée de manière sûre dans cette build.
            // On renvoie donc explicitement un échec pour éviter toute confusion.
            return Result<SignatureResponse>.Failure("Signing is not implemented yet in this build. TODO: wire Vault Transit + NBitcoin signature.");
        }
        catch (Exception ex)
        {
            return Result<SignatureResponse>.Failure($"Failed to sign transaction: {ex.Message}");
        }
    }
}
