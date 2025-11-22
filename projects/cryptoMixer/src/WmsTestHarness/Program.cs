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

Console.WriteLine("=== End of harness run ===");


public sealed record AddressResponseDto(string Address, string DerivationPath);
