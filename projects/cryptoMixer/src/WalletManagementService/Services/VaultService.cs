using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;
using NBitcoin;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CryptoMixer.WalletManagementService.Services;

/// <summary>
/// Pont sécurisé vers HashiCorp Vault.
/// Attention : cette implémentation est conceptuelle et ne doit pas être utilisée en production
/// sans configuration et durcissement exhaustifs.
/// </summary>
public class VaultService : IVaultService
{
    private readonly IVaultClient? _vaultClient;
    private const string KvMountPoint = "secret"; // engine KV v2 par défaut en dev
    private const string MasterKeyRelativePath = "wms/masterkey"; // chemin logique du secret dans le mount
    private readonly bool _disabled;
    private ExtKey? _inMemoryMasterKey;
    private readonly IConfiguration _config;

    public VaultService(IConfiguration config)
    {
        _config = config;
        _disabled = string.Equals(config["Vault:Disabled"], "true", StringComparison.OrdinalIgnoreCase);
        if (_disabled)
        {
            // When Vault is disabled we operate in-memory; do not require VAULT_ADDR/VAULT_TOKEN.
            return;
        }

        // Préférence : utiliser la section de configuration "Vault" (appsettings*.json, variables d'env préfixées Vault__).
        var vaultAddr  = config["Vault:Address"] ?? config["VAULT_ADDR"]  ?? throw new InvalidOperationException("Vault address is not configured");
        var vaultToken = config["Vault:Token"]   ?? config["VAULT_TOKEN"] ?? throw new InvalidOperationException("Vault token is not configured");

        var authMethod = new TokenAuthMethodInfo(vaultToken);
        _vaultClient   = new VaultClient(new VaultClientSettings(vaultAddr, authMethod));
    }

    public bool Disabled => _disabled;

    public async Task<ExtKey> GetMasterKeyAsync()
    {
        if (_disabled)
        {
            if (_inMemoryMasterKey is not null)
                return _inMemoryMasterKey;

            // If a mnemonic is provided via config, derive from it; otherwise generate a new master.
            var configuredMnemonic = _config["Wms:LocalMasterMnemonic"];
            if (!string.IsNullOrWhiteSpace(configuredMnemonic))
            {
                var m = new Mnemonic(configuredMnemonic);
                _inMemoryMasterKey = m.DeriveExtKey();
                return _inMemoryMasterKey;
            }

            var newMnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            _inMemoryMasterKey = newMnemonic.DeriveExtKey();
            return _inMemoryMasterKey;
        }
        // With KV v2, read the secret. Prefer an explicitly stored xpub to avoid materializing seed.
        Secret<SecretData> secret = await _vaultClient!.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            path: MasterKeyRelativePath,
            mountPoint: KvMountPoint);
        if (secret.Data.Data.TryGetValue("seed", out var seedObj) && seedObj is not null)
        {
            var seedHex = seedObj.ToString()!;
            return ExtKey.Parse(seedHex, Network.Main);
        }

        // If no seed is present, but an xpub is stored, we cannot return ExtKey safely.
        throw new InvalidOperationException("Master seed not found in Vault; call GetMasterXPubAsync instead.");
    }

    public async Task<ExtPubKey> GetMasterXPubAsync()
    {
        if (_disabled)
        {
            if (_inMemoryMasterKey is not null)
                return _inMemoryMasterKey.Neuter();

            var configuredMnemonic = _config["Wms:LocalMasterMnemonic"];
            if (!string.IsNullOrWhiteSpace(configuredMnemonic))
            {
                var m = new Mnemonic(configuredMnemonic);
                _inMemoryMasterKey = m.DeriveExtKey();
                return _inMemoryMasterKey.Neuter();
            }

            var newMnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            _inMemoryMasterKey = newMnemonic.DeriveExtKey();
            return _inMemoryMasterKey.Neuter();
        }

        Secret<SecretData> secret = await _vaultClient!.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            path: MasterKeyRelativePath,
            mountPoint: KvMountPoint);

        // Prefer stored xpub
        if (secret.Data.Data.TryGetValue("xpub", out var xpubObj) && xpubObj is not null)
        {
            var xpub = xpubObj.ToString()!;
            return ExtPubKey.Parse(xpub, Network.Main);
        }

        // If xpub not stored, derive from seed if available
        if (secret.Data.Data.TryGetValue("seed", out var seedObj) && seedObj is not null)
        {
            var seedHex = seedObj.ToString()!;
            var ext = ExtKey.Parse(seedHex, Network.Main);
            return ext.Neuter();
        }

        throw new InvalidOperationException("Master key not found in Vault KV (no xpub or seed).");
    }

    public async Task<string> SignTransactionHashAsync(string keyName, string hashToSignBase64)
    {
        if (_disabled)
        {
            throw new InvalidOperationException("Vault is disabled; Transit signing is not available.");
        }

        // We call the Vault Transit API directly using the configured VAULT address and token.
        // Expected request body: { "input": "<base64>", "prehashed": true }
        var vaultAddr = _config["Vault:Address"] ?? _config["VAULT_ADDR"] ?? throw new InvalidOperationException("Vault address is not configured for Transit call");
        var vaultToken = _config["Vault:Token"] ?? _config["VAULT_TOKEN"] ?? throw new InvalidOperationException("Vault token is not configured for Transit call");

        // Build URL: POST {vaultAddr}/v1/transit/sign/{keyName}
        var baseUri = vaultAddr.TrimEnd('/');
        var url = $"{baseUri}/v1/transit/sign/{Uri.EscapeDataString(keyName)}";

        using var http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-Vault-Token", vaultToken);

        var body = new Dictionary<string, object?>
        {
            { "input", hashToSignBase64 },
            { "prehashed", true }
        };

        var payload = JsonConvert.SerializeObject(body);
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var resp = await http.PostAsync(url, content);
        var respBody = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Vault Transit sign failed: {resp.StatusCode} - {respBody}");
        }

        // Expected response format: { "data": { "signature": "vault:v1:<BASE64>" } }
        try
        {
            var j = JObject.Parse(respBody);
            var sigFull = j["data"]?["signature"]?.ToString();
            if (string.IsNullOrWhiteSpace(sigFull))
                throw new InvalidOperationException("Vault Transit response missing signature field: " + respBody);

            // signature format: vault:v1:<base64>
            var parts = sigFull.Split(':');
            var b64 = parts.Last();
            return b64;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to parse Vault Transit response: " + ex.Message + " - " + respBody);
        }
    }

    public async Task SetMasterKeyAsync(ExtKey extKey)
    {
        if (_disabled)
        {
            _inMemoryMasterKey = extKey;
            return;
        }

        // In non-disabled mode, persist the seed (hex) into KV v2. This will overwrite existing.
        var seedHex = extKey.ToString();
        var data = new Dictionary<string, object?>
        {
            { "seed", seedHex }
        };

        await _vaultClient!.V1.Secrets.KeyValue.V2.WriteSecretAsync(path: MasterKeyRelativePath, data: data, mountPoint: KvMountPoint);
    }
}
