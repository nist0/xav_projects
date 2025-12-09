using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace WmsE2E.Tests;

public class E2ETests
{
    private const string ServiceProject = "c:\\Users\\yanis\\source\\repos\\xav_projects\\projects\\cryptoMixer\\src\\WalletManagementService\\WalletManagementService.csproj";
    private const string BaseUrl = "https://localhost:5001";

    [Fact]
    public async Task CreateThenVerify_ReturnsAllTrue()
    {
        var psi = new ProcessStartInfo("dotnet", $"run --project \"{ServiceProject}\"")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        // set env vars for in-memory Vault and fixed URLs
        psi.Environment["Vault__Disabled"] = "true";
        psi.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        psi.Environment["ASPNETCORE_URLS"] = "https://localhost:5001;http://localhost:5000";

        var proc = Process.Start(psi);
        Assert.NotNull(proc);

        try
        {
            using var http = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator });

            // wait until /health or swagger ready
            var timeout = Task.Delay(15000);
            var ready = false;
            while (!timeout.IsCompleted)
            {
                try
                {
                    var r = await http.GetAsync(BaseUrl + "/health");
                    if (r.IsSuccessStatusCode) { ready = true; break; }
                }
                catch { }

                try { await Task.Delay(500); } catch { }
            }

            Assert.True(ready, "Service did not become ready in time");

            // 1) create wallet
            var createReq = new { network = "testnet", importToVaultIfDisabled = true };
            var createJson = JsonConvert.SerializeObject(createReq);
            var createResp = await http.PostAsync(BaseUrl + "/api/wallets/new", new StringContent(createJson, Encoding.UTF8, "application/json"));
            Assert.True(createResp.IsSuccessStatusCode, "Create wallet failed");
            var createBody = await createResp.Content.ReadAsStringAsync();
            var created = JsonConvert.DeserializeObject<CreateWalletDto>(createBody);
            Assert.NotNull(created);

            // 2) verify wallet
            var verifyReq = new
            {
                mnemonic = created.Mnemonic,
                xPrv = created.XPrv,
                xPub = created.XPub,
                firstAddress = created.FirstAddress,
                network = created.Network
            };

            var verifyJson = JsonConvert.SerializeObject(verifyReq);
            var verifyResp = await http.PostAsync(BaseUrl + "/api/wallets/verify", new StringContent(verifyJson, Encoding.UTF8, "application/json"));
            Assert.True(verifyResp.IsSuccessStatusCode, "Verify wallet failed");
            var verifyBody = await verifyResp.Content.ReadAsStringAsync();
            var verified = JsonConvert.DeserializeObject<VerifyWalletDto>(verifyBody);
            Assert.NotNull(verified);

            Assert.True(verified.mnemonicMatchesXPrv, "mnemonicMatchesXPrv false");
            Assert.True(verified.xPrvMatches, "xPrvMatches false");
            Assert.True(verified.xPubMatches, "xPubMatches false");
            Assert.True(verified.addressMatches, "addressMatches false");
        }
        finally
        {
            try
            {
                if (!proc.HasExited)
                {
                    proc.Kill(true);
                    proc.WaitForExit(5000);
                }
            }
            catch { }
        }
    }

    record CreateWalletDto(string Mnemonic, string XPrv, string XPub, string FirstAddress, string Network);
    record VerifyWalletDto(bool mnemonicMatchesXPrv, bool xPrvMatches, bool xPubMatches, bool addressMatches, string derivedXPrv, string derivedXPub, string derivedFirstAddress, string message);
}
