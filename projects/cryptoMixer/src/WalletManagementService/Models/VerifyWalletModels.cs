namespace CryptoMixer.WalletManagementService.Models;

public sealed record VerifyWalletRequest(
    string? Mnemonic,
    string? XPrv,
    string? XPub,
    string? FirstAddress,
    string? Network
);

public sealed record VerifyWalletResponse(
    bool MnemonicMatchesXPrv,
    bool XPrvMatches,
    bool XPubMatches,
    bool AddressMatches,
    string? DerivedXPrv,
    string? DerivedXPub,
    string? DerivedFirstAddress,
    string Message
);
