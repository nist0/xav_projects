namespace CryptoMixer.WalletManagementService.Models;

public sealed record AddressRequest(string DerivationPath);

public sealed record AddressResponse(string Address, string DerivationPath);
