using System.Text;
using Newtonsoft.Json;

Console.WriteLine("=== WMS E2E Test ===");

var baseUrl = Environment.GetEnvironmentVariable("WMS_BASE_URL") ?? "https://localhost:5001";
Console.WriteLine($"WMS_BASE_URL = {baseUrl}");

using var http = new HttpClient(new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

// 1) Create wallet
var createReq = new { network = "testnet", importToVaultIfDisabled = true };
var createJson = JsonConvert.SerializeObject(createReq);
var createResp = await http.PostAsync($"{baseUrl.TrimEnd('/')}/api/wallets/new", new StringContent(createJson, Encoding.UTF8, "application/json"));
Console.WriteLine("Create status: " + createResp.StatusCode);
var createBody = await createResp.Content.ReadAsStringAsync();
Console.WriteLine("Create body: " + createBody);
if (!createResp.IsSuccessStatusCode)
{
    Console.WriteLine("[FAIL] /api/wallets/new failed");
    return 1;
}

var created = JsonConvert.DeserializeObject<CreateWalletDto>(createBody);
if (created is null)
{
    Console.WriteLine("[FAIL] Cannot deserialize create response");
    return 1;
}

// 2) Verify wallet
var verifyReq = new
{
    mnemonic = created.Mnemonic,
    xPrv = created.XPrv,
    xPub = created.XPub,
    firstAddress = created.FirstAddress,
    network = created.Network
};

var verifyJson = JsonConvert.SerializeObject(verifyReq);
var verifyResp = await http.PostAsync($"{baseUrl.TrimEnd('/')}/api/wallets/verify", new StringContent(verifyJson, Encoding.UTF8, "application/json"));
Console.WriteLine("Verify status: " + verifyResp.StatusCode);
var verifyBody = await verifyResp.Content.ReadAsStringAsync();
Console.WriteLine("Verify body: " + verifyBody);
if (!verifyResp.IsSuccessStatusCode)
{
    Console.WriteLine("[FAIL] /api/wallets/verify failed");
    return 1;
}

var verified = JsonConvert.DeserializeObject<VerifyWalletDto>(verifyBody);
if (verified is null)
{
    Console.WriteLine("[FAIL] Cannot deserialize verify response");
    return 1;
}

if (verified.mnemonicMatchesXPrv && verified.xPrvMatches && verified.xPubMatches && verified.addressMatches)
{
    Console.WriteLine("[PASS] E2E verification succeeded");
    return 0;
}

Console.WriteLine("[FAIL] Verification flags not all true");
return 1;

record CreateWalletDto(string Mnemonic, string XPrv, string XPub, string FirstAddress, string Network);
record VerifyWalletDto(bool mnemonicMatchesXPrv, bool xPrvMatches, bool xPubMatches, bool addressMatches, string derivedXPrv, string derivedXPub, string derivedFirstAddress, string message);
