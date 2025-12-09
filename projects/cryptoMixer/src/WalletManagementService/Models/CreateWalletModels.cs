namespace CryptoMixer.WalletManagementService.Models;

public sealed record CreateWalletRequest(string? Network, bool ImportToVaultIfDisabled = true);

public sealed record CreateWalletResponse(string Mnemonic, string XPrv, string XPub, string FirstAddress, string Network);
