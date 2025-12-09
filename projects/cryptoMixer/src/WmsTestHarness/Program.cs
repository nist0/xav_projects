using System.Text;
using Newtonsoft.Json;
using NBitcoin;

Console.WriteLine("=== WMS Test Harness (dev / testnet) ===");

// Configuration de base
var wmsBaseUrl = Environment.GetEnvironmentVariable("WMS_BASE_URL")
                 ?? "https://localhost:5001"; // adapter au port réel

Console.WriteLine($"WMS_BASE_URL = {wmsBaseUrl}");

using var http = new HttpClient(new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

// 1) Appeler /api/addresses pour vérifier le flow d'adresse HD

var addressRequest = new
{
    derivationPath = "m/0/0/0"
};

var addressJson = JsonConvert.SerializeObject(addressRequest);
var addressContent = new StringContent(addressJson, Encoding.UTF8, "application/json");

Console.WriteLine("Calling WMS /api/addresses...");

var addressResponse = await http.PostAsync($"{wmsBaseUrl.TrimEnd('/')}/api/addresses", addressContent);
Console.WriteLine("HTTP status: " + addressResponse.StatusCode);

var addressBody = await addressResponse.Content.ReadAsStringAsync();
Console.WriteLine("Response body: " + addressBody);

if (!addressResponse.IsSuccessStatusCode)
{
    Console.WriteLine("[KO] /api/addresses returned an error.");
    return;
}

var addr = JsonConvert.DeserializeObject<AddressResponseDto>(addressBody);
if (addr is null)
{
    Console.WriteLine("Failed to deserialize address response");
    return;
}

Console.WriteLine($"[OK] Generated address: {addr.Address} (path {addr.DerivationPath})");

// 2) Create a new wallet (mnemonic + xprv/xpub + first address)
Console.WriteLine("Calling WMS /api/wallets/new...");
var walletReq = new { network = "testnet", importToVaultIfDisabled = true };
var walletJson = JsonConvert.SerializeObject(walletReq);
var walletContent = new StringContent(walletJson, Encoding.UTF8, "application/json");

var walletResponse = await http.PostAsync($"{wmsBaseUrl.TrimEnd('/')}/api/wallets/new", walletContent);
Console.WriteLine("HTTP status (wallet create): " + walletResponse.StatusCode);
var walletBody = await walletResponse.Content.ReadAsStringAsync();
Console.WriteLine("Response body (wallet): " + walletBody);

if (!walletResponse.IsSuccessStatusCode)
{
    Console.WriteLine("[KO] /api/wallets/new returned an error.");
    return;
}

var wallet = JsonConvert.DeserializeObject<WalletResponseDto>(walletBody);
if (wallet is null)
{
    Console.WriteLine("Failed to deserialize wallet response");
    return;
}

Console.WriteLine($"[OK] Created wallet (network {wallet.Network}). FirstAddress: {wallet.FirstAddress}");

Console.WriteLine("=== End of harness run ===");


public sealed record AddressResponseDto(string Address, string DerivationPath);
public sealed record WalletResponseDto(string Mnemonic, string XPrv, string XPub, string FirstAddress, string Network);
