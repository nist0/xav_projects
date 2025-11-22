using NBitcoin;

namespace CryptoMixer.WalletManagementService.Services;

public interface IVaultService
{
    Task<ExtKey> GetMasterKeyAsync();
    Task<string> SignTransactionHashAsync(string keyName, string hashToSignBase64);
}
