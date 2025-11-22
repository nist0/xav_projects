using CryptoMixer.WalletManagementService.Models;
using CryptoMixer.Shared;

namespace CryptoMixer.WalletManagementService.Services;

public interface IWalletService
{
    Result<AddressResponse> GenerateNewAddress(AddressRequest request);
    Task<Result<SignatureResponse>> SignTransactionAsync(SignatureRequest request);
}
