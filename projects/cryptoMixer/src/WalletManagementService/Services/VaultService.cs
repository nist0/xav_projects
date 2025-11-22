using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;
using NBitcoin;

namespace CryptoMixer.WalletManagementService.Services;

/// <summary>
/// Pont sécurisé vers HashiCorp Vault.
/// Attention : cette implémentation est conceptuelle et ne doit pas être utilisée en production
/// sans configuration et durcissement exhaustifs.
/// </summary>
public class VaultService : IVaultService
{
    private readonly IVaultClient _vaultClient;
    private const string KvMountPoint = "secret"; // engine KV v2 par défaut en dev
    private const string MasterKeyRelativePath = "wms/masterkey"; // chemin logique du secret dans le mount

    public VaultService(IConfiguration config)
    {
        // Préférence : utiliser la section de configuration "Vault" (appsettings*.json, variables d'env préfixées Vault__).
        var vaultAddr  = config["Vault:Address"] ?? config["VAULT_ADDR"]  ?? throw new InvalidOperationException("Vault address is not configured");
        var vaultToken = config["Vault:Token"]   ?? config["VAULT_TOKEN"] ?? throw new InvalidOperationException("Vault token is not configured");

        var authMethod = new TokenAuthMethodInfo(vaultToken);
        _vaultClient   = new VaultClient(new VaultClientSettings(vaultAddr, authMethod));
    }

    public async Task<ExtKey> GetMasterKeyAsync()
    {
        // Avec KV v2, on fournit séparément le mountPoint ("secret") et le chemin relatif ("wms/masterkey").
        Secret<SecretData> secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            path: MasterKeyRelativePath,
            mountPoint: KvMountPoint);
        if (!secret.Data.Data.TryGetValue("seed", out var seedObj) || seedObj is null)
        {
            throw new InvalidOperationException("Master seed not found in Vault");
        }

        var seedHex = seedObj.ToString()!;
        return ExtKey.Parse(seedHex, Network.Main);
    }

    public async Task<string> SignTransactionHashAsync(string keyName, string hashToSignBase64)
    {
        // Appel Transit via l'API générique : dans cette version de VaultSharp, il n'y a pas
        // de client fort-typé pour Transit, nous utilisons donc une requête HTTP brute.
        // À ce stade, cette méthode doit être considérée comme un stub à adapter si Transit
        // est utilisé réellement.

        // Pour l'instant, la signature Transit n'est pas encore implémentée de manière fiable
        // avec cette version de VaultSharp. On laisse un stub explicite pour éviter toute
        // illusion de sécurité.
        throw new NotImplementedException("SignTransactionHashAsync is not wired to Vault Transit in this build.");
    }
}
