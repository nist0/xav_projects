namespace CryptoMixer.WalletManagementService.Models;

public sealed record SignatureRequest(string UnsignedTransactionHex, List<InputToSign> InputsToSign);

public sealed record InputToSign(int InputIndex, string DerivationPath, string ScriptPubKeyHex, long AmountSatoshis);

public sealed record SignatureResponse(string SignedTransactionHex);
