using NBitcoin;

namespace CryptoMixer.WalletManagementService.Services;

public interface IVaultService
{
    Task<ExtKey> GetMasterKeyAsync();
    Task<ExtPubKey> GetMasterXPubAsync();
    Task<string> SignTransactionHashAsync(string keyName, string hashToSignBase64);
    Task SetMasterKeyAsync(ExtKey extKey);
    bool Disabled { get; }
}
