using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace WmsE2E.Integration;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    private static Dictionary<string, string>? ParseHeaders(string? env)
    {
        if (string.IsNullOrEmpty(env)) return null;
        try
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(env);
        }
        catch
        {
            return null;
        }
    }

    private static void ApplyHeaders(System.Net.Http.HttpRequestMessage req, Dictionary<string, string>? headers)
    {
        if (headers == null) return;
        foreach (var kv in headers)
        {
            // add without validation to allow Authorization and custom headers
            req.Headers.TryAddWithoutValidation(kv.Key, kv.Value);
        }
    }

    // Bounded optimal subset-sum coin selection.
    // Attempts to find a subset of utxos whose sum >= needed with minimal excess.
    // Uses DFS with pruning and caps on iterations and max utxos considered to avoid explosion.
    private static List<UtxoDto>? SelectBestUtxosOptimal(List<UtxoDto> utxos, long needed, int maxUtxos = 12, int maxIterations = 200000)
    {
        if (utxos == null || !utxos.Any()) return null;
        // Sort descending to try large coins first (helps prune)
        var ordered = utxos.OrderByDescending(u => u.AmountSatoshis).ToArray();

        long bestTotal = long.MaxValue;
        List<UtxoDto>? bestSel = null;
        long remainingSum = ordered.Sum(u => u.AmountSatoshis);

        int iterations = 0;

        void Dfs(int idx, long currentSum, List<UtxoDto> currentSel, long remSum)
        {
            if (iterations++ > maxIterations) return;
            if (currentSel.Count > maxUtxos) return;

            // If current sum already greater than best found, we can prune if it's worse
            if (currentSum >= needed)
            {
                if (currentSum < bestTotal || (currentSum == bestTotal && (bestSel == null || currentSel.Count < bestSel.Count)))
                {
                    bestTotal = currentSum;
                    bestSel = new List<UtxoDto>(currentSel);
                    // perfect match, stop early
                    if (bestTotal == needed) return;
                }
                // no need to add more coins to current selection
                // But still continue to explore because a later branch might have exact match
            }

            if (idx >= ordered.Length) return;

            // If even taking all remaining cannot beat current best, prune
            if (currentSum + remSum < needed) return;
            if (currentSum >= bestTotal) return;

            // Heuristic: try include current coin first
            var u = ordered[idx];
            currentSel.Add(u);
            Dfs(idx + 1, currentSum + u.AmountSatoshis, currentSel, remSum - u.AmountSatoshis);
            currentSel.RemoveAt(currentSel.Count - 1);

            // Then try excluding it
            Dfs(idx + 1, currentSum, currentSel, remSum - u.AmountSatoshis);
        }

        Dfs(0, 0, new List<UtxoDto>(), remainingSum);
        return bestSel;
    }

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Ensure in-memory Vault mode for tests
        Environment.SetEnvironmentVariable("Vault__Disabled", "true");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "http://localhost:5000;https://localhost:5001");
        Environment.SetEnvironmentVariable("WMS_NETWORK", "testnet");

        _factory = factory.WithWebHostBuilder(builder => builder.UseSetting("environment", "Development"));
    }

    [Fact]
    public async Task CreateThenVerify_ReturnsAllTrue()
    {
        using var client = _factory.CreateClient();

        // 1) create wallet
        var createReq = new { network = "testnet", importToVaultIfDisabled = true };
        var createJson = JsonConvert.SerializeObject(createReq);
        var createResp = await client.PostAsync("/api/wallets/new", new StringContent(createJson, Encoding.UTF8, "application/json"));
        createResp.EnsureSuccessStatusCode();
        var createBody = await createResp.Content.ReadAsStringAsync();
        var created = JsonConvert.DeserializeObject<CreateWalletDto>(createBody);
        Assert.NotNull(created);

        // 2) verify wallet
        var verifyReq = new
        {
            mnemonic = created!.Mnemonic,
            xPrv = created.XPrv,
            xPub = created.XPub,
            firstAddress = created.FirstAddress,
            network = created.Network
        };

        var verifyJson = JsonConvert.SerializeObject(verifyReq);
        var verifyResp = await client.PostAsync("/api/wallets/verify", new StringContent(verifyJson, Encoding.UTF8, "application/json"));
        verifyResp.EnsureSuccessStatusCode();
        var verifyBody = await verifyResp.Content.ReadAsStringAsync();
        var verified = JsonConvert.DeserializeObject<VerifyWalletDto>(verifyBody);
        Assert.NotNull(verified);

        Assert.True(verified!.mnemonicMatchesXPrv && verified.xPrvMatches && verified.xPubMatches && verified.addressMatches);
    }

    [Fact]
    public async Task CreateThenAddressDerivation_MatchesFirstAddress()
    {
        using var client = _factory.CreateClient();

        var createReq = new { network = "testnet", importToVaultIfDisabled = true };
        var createJson = JsonConvert.SerializeObject(createReq);
        var createResp = await client.PostAsync("/api/wallets/new", new StringContent(createJson, Encoding.UTF8, "application/json"));
        createResp.EnsureSuccessStatusCode();
        var createBody = await createResp.Content.ReadAsStringAsync();
        var created = JsonConvert.DeserializeObject<CreateWalletDto>(createBody);
        Assert.NotNull(created);

        // Ask the service to derive the same address for path m/0/0/0
        var addrReq = new { derivationPath = "m/0/0/0" };
        var addrJson = JsonConvert.SerializeObject(addrReq);
        var addrResp = await client.PostAsync("/api/addresses", new StringContent(addrJson, Encoding.UTF8, "application/json"));
        addrResp.EnsureSuccessStatusCode();
        var addrBody = await addrResp.Content.ReadAsStringAsync();
        var adr = JsonConvert.DeserializeObject<AddressDto>(addrBody);
        Assert.NotNull(adr);

        Assert.Equal(created!.FirstAddress, adr!.Address);
    }

    [Fact]
    public async Task Sign_NativeSegwit_Succeeds()
    {
        using var client = _factory.CreateClient();

        var createReq = new { network = "testnet", importToVaultIfDisabled = true };
        var createJson = JsonConvert.SerializeObject(createReq);
        var createResp = await client.PostAsync("/api/wallets/new", new StringContent(createJson, Encoding.UTF8, "application/json"));
        createResp.EnsureSuccessStatusCode();
        var createBody = await createResp.Content.ReadAsStringAsync();
        var created = JsonConvert.DeserializeObject<CreateWalletDto>(createBody);
        Assert.NotNull(created);

        // Derive first address (m/0/0/0) - should be native segwit as created by the service
        var addrReq = new { derivationPath = "m/0/0/0" };
        var addrJson = JsonConvert.SerializeObject(addrReq);
        var addrResp = await client.PostAsync("/api/addresses", new StringContent(addrJson, Encoding.UTF8, "application/json"));
        addrResp.EnsureSuccessStatusCode();
        var addrBody = await addrResp.Content.ReadAsStringAsync();
        var addr = JsonConvert.DeserializeObject<AddressDto>(addrBody);
        Assert.NotNull(addr);

        // Build a fake prevout and unsigned tx referencing it
        var network = NBitcoin.Network.TestNet;
        var txid = new NBitcoin.uint256(Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"));
        var outpoint = new NBitcoin.OutPoint(txid, 0);
        var prevTxOut = new NBitcoin.TxOut(NBitcoin.Money.Satoshis(15000), NBitcoin.Script.FromHex(addr!.Address == null ? "" : NBitcoin.BitcoinAddress.Create(addr.Address, network).ScriptPubKey.ToHex()));

        var tx = network.CreateTransaction();
        tx.Inputs.Add(new NBitcoin.TxIn(outpoint));
        tx.Outputs.Add(new NBitcoin.TxOut(NBitcoin.Money.Satoshis(10000), NBitcoin.BitcoinAddress.Create(created!.FirstAddress, network)));

        var signatureReq = new
        {
            unsignedTransactionHex = tx.ToHex(),
            inputsToSign = new[] { new { inputIndex = 0, derivationPath = "m/0/0/0", scriptPubKeyHex = NBitcoin.BitcoinAddress.Create(created.FirstAddress, network).ScriptPubKey.ToHex(), amountSatoshis = 15000L } }
        };

        var sigJson = JsonConvert.SerializeObject(signatureReq);
        var sigResp = await client.PostAsync("/api/signatures", new StringContent(sigJson, Encoding.UTF8, "application/json"));
        var sigBody = await sigResp.Content.ReadAsStringAsync();

        Assert.True(sigResp.IsSuccessStatusCode, "Signing native segwit should succeed in test mode");
        var resp = JsonConvert.DeserializeObject<SignatureRespDto>(sigBody);
        Assert.NotNull(resp);
        Assert.False(string.IsNullOrWhiteSpace(resp!.SignedTransactionHex));
    }

    [Fact]
    public async Task Sign_P2shP2wpkh_Succeeds()
    {
        using var client = _factory.CreateClient();

        var createReq = new { network = "testnet", importToVaultIfDisabled = true };
        var createJson = JsonConvert.SerializeObject(createReq);
        var createResp = await client.PostAsync("/api/wallets/new", new StringContent(createJson, Encoding.UTF8, "application/json"));
        createResp.EnsureSuccessStatusCode();
        var createBody = await createResp.Content.ReadAsStringAsync();
        var created = JsonConvert.DeserializeObject<CreateWalletDto>(createBody);
        Assert.NotNull(created);

        // Derive pubkey and compute P2SH-P2WPKH address from the created xprv
        var network = NBitcoin.Network.TestNet;
        var mnemonic = new NBitcoin.Mnemonic(created!.Mnemonic);
        var extKey = mnemonic.DeriveExtKey();
        var derived = extKey.Derive(new NBitcoin.KeyPath("m/0/0/0"));
        var pub = derived.Neuter().PubKey;
        var p2shAddr = pub.GetAddress(NBitcoin.ScriptPubKeyType.SegwitP2SH, network);

        var txid = new NBitcoin.uint256(Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"));
        var outpoint = new NBitcoin.OutPoint(txid, 0);
        var prevTxOut = new NBitcoin.TxOut(NBitcoin.Money.Satoshis(20000), p2shAddr.ScriptPubKey);

        var tx = network.CreateTransaction();
        tx.Inputs.Add(new NBitcoin.TxIn(outpoint));
        tx.Outputs.Add(new NBitcoin.TxOut(NBitcoin.Money.Satoshis(15000), NBitcoin.BitcoinAddress.Create(created.FirstAddress, network)));

        var signatureReq = new
        {
            unsignedTransactionHex = tx.ToHex(),
            inputsToSign = new[] { new { inputIndex = 0, derivationPath = "m/0/0/0", scriptPubKeyHex = p2shAddr.ScriptPubKey.ToHex(), amountSatoshis = 20000L } }
        };

        var sigJson = JsonConvert.SerializeObject(signatureReq);
        var sigResp = await client.PostAsync("/api/signatures", new StringContent(sigJson, Encoding.UTF8, "application/json"));
        var sigBody = await sigResp.Content.ReadAsStringAsync();

        Assert.True(sigResp.IsSuccessStatusCode, "Signing P2SH-P2WPKH should succeed in test mode");
        var resp = JsonConvert.DeserializeObject<SignatureRespDto>(sigBody);
        Assert.NotNull(resp);
        Assert.False(string.IsNullOrWhiteSpace(resp!.SignedTransactionHex));
    }

    [Fact]
    public async Task SignatureEndpoint_BasicValidationOrSigning()
    {
        using var client = _factory.CreateClient();

        // Create a wallet so a master key exists in in-memory Vault
        var createReq = new { network = "testnet", importToVaultIfDisabled = true };
        var createJson = JsonConvert.SerializeObject(createReq);
        var createResp = await client.PostAsync("/api/wallets/new", new StringContent(createJson, Encoding.UTF8, "application/json"));
        createResp.EnsureSuccessStatusCode();
        var createBody = await createResp.Content.ReadAsStringAsync();
        var created = JsonConvert.DeserializeObject<CreateWalletDto>(createBody);
        Assert.NotNull(created);

        // Build an empty transaction hex (parsing should succeed)
        var tx = NBitcoin.Transaction.Create(NBitcoin.Network.TestNet);
        var unsignedHex = tx.ToHex();

        var signatureReq = new
        {
            unsignedTransactionHex = unsignedHex,
            inputsToSign = new[] { new { inputIndex = 0, derivationPath = "m/0/0/0", scriptPubKeyHex = "", amountSatoshis = 0L } }
        };

        var sigJson = JsonConvert.SerializeObject(signatureReq);
        var sigResp = await client.PostAsync("/api/signatures", new StringContent(sigJson, Encoding.UTF8, "application/json"));

        var sigBody = await sigResp.Content.ReadAsStringAsync();

        if (sigResp.IsSuccessStatusCode)
        {
            // If signing succeeded, the response should contain a SignedTransactionHex
            var resp = JsonConvert.DeserializeObject<SignatureRespDto>(sigBody);
            Assert.NotNull(resp);
            Assert.False(string.IsNullOrWhiteSpace(resp!.SignedTransactionHex));
        }
        else
        {
            // Otherwise ensure the server returned a clear validation error (e.g. Invalid InputIndex)
            Assert.Contains("Invalid InputIndex", sigBody);
        }
    }

    [Fact]
    public async Task FullSignAndBroadcast_Conditional()
    {
        // Runs only when FAUCET_URL, BLOCK_EXPLORER_UTXO_URL and BROADCAST_URL are provided as env vars.
        var faucetUrl = Environment.GetEnvironmentVariable("FAUCET_URL");
        var explorerUtxoUrl = Environment.GetEnvironmentVariable("BLOCK_EXPLORER_UTXO_URL");
        var broadcastUrl = Environment.GetEnvironmentVariable("BROADCAST_URL");

        if (string.IsNullOrEmpty(faucetUrl) || string.IsNullOrEmpty(explorerUtxoUrl) || string.IsNullOrEmpty(broadcastUrl))
        {
            // Not configured for live funded test — skip by returning early (test is considered passed).
            return;
        }

        using var client = _factory.CreateClient();

        // 1) create wallet
        var createReq = new { network = "testnet", importToVaultIfDisabled = true };
        var createJson = JsonConvert.SerializeObject(createReq);
        var createResp = await client.PostAsync("/api/wallets/new", new StringContent(createJson, Encoding.UTF8, "application/json"));
        createResp.EnsureSuccessStatusCode();
        var createBody = await createResp.Content.ReadAsStringAsync();
        var created = JsonConvert.DeserializeObject<CreateWalletDto>(createBody);
        Assert.NotNull(created);

        // 2) request funding from faucet (support optional headers via FAUCET_HEADERS env var)
        var faucetReq = new { address = created!.FirstAddress, amountSatoshis = 25000L };
        using var http = new HttpClient();
        var faucetHeaders = ParseHeaders(Environment.GetEnvironmentVariable("FAUCET_HEADERS"));
        var faucetRequest = new HttpRequestMessage(HttpMethod.Post, faucetUrl)
        {
            Content = new StringContent(JsonConvert.SerializeObject(faucetReq), Encoding.UTF8, "application/json")
        };
        ApplyHeaders(faucetRequest, faucetHeaders);
        var faucetResp = await http.SendAsync(faucetRequest);
        faucetResp.EnsureSuccessStatusCode();

        // 3) poll block explorer for UTXOs for this address and select enough UTXOs to cover target+fee
        List<UtxoDto>? utxos = null;
        var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));

        var explorerHeaders = ParseHeaders(Environment.GetEnvironmentVariable("EXPLORER_HEADERS"));
        while (!cts.IsCancellationRequested)
        {
            var explRequest = new HttpRequestMessage(HttpMethod.Get, explorerUtxoUrl + "?address=" + Uri.EscapeDataString(created.FirstAddress));
            ApplyHeaders(explRequest, explorerHeaders);
            var utxoResp = await http.SendAsync(explRequest, cts.Token);
            if (utxoResp.IsSuccessStatusCode)
            {
                var body = await utxoResp.Content.ReadAsStringAsync();
                try
                {
                    utxos = JsonConvert.DeserializeObject<List<UtxoDto>>(body);
                }
                catch
                {
                    utxos = null;
                }

                if (utxos != null && utxos.Any()) break;
            }

            await Task.Delay(5000, cts.Token);
        }

        Assert.True(utxos != null && utxos.Any(), "No UTXOs found for funded address within timeout");

        // Configurable amounts/fees
        long targetAmount = 15000L; // satoshis to send
        long feePerTx = 1000L; // default fee
        long dustThreshold = 1000L; // if change < dust, absorb into fee

        var envTarget = Environment.GetEnvironmentVariable("TEST_TARGET_SATOSHIS");
        var envFee = Environment.GetEnvironmentVariable("TEST_FEE_SATOSHIS");
        var envDust = Environment.GetEnvironmentVariable("TEST_DUST_SATOSHIS");
        if (long.TryParse(envTarget, out var t)) targetAmount = t;
        if (long.TryParse(envFee, out var f)) feePerTx = f;
        if (long.TryParse(envDust, out var d)) dustThreshold = d;

        var network = NBitcoin.Network.TestNet;


        // Select UTXOs with multiple strategies and pick the best one
        List<UtxoDto> bestSelected = new();
        long bestTotal = long.MaxValue;
        long needed = targetAmount + feePerTx;

        // Strategy A: single large UTXO
        var single = utxos!.Where(u => u.AmountSatoshis >= needed).OrderBy(u => u.AmountSatoshis).FirstOrDefault();
        if (single != null)
        {
            bestSelected = new List<UtxoDto> { single };
            bestTotal = single.AmountSatoshis;
        }

        // Strategy B: greedy ascending (smallest first)
        var ascSel = new List<UtxoDto>();
        long ascTotal = 0;
        foreach (var u in utxos.OrderBy(u => u.AmountSatoshis))
        {
            ascSel.Add(u);
            ascTotal += u.AmountSatoshis;
            if (ascTotal >= needed) break;
        }
        if (ascTotal >= needed && ascTotal < bestTotal)
        {
            bestSelected = ascSel.ToList();
            bestTotal = ascTotal;
        }

        // Strategy C: greedy descending (largest first)
        var descSel = new List<UtxoDto>();
        long descTotal = 0;
        foreach (var u in utxos.OrderByDescending(u => u.AmountSatoshis))
        {
            descSel.Add(u);
            descTotal += u.AmountSatoshis;
            if (descTotal >= needed) break;
        }
        if (descTotal >= needed && descTotal < bestTotal)
        {
            bestSelected = descSel.ToList();
            bestTotal = descTotal;
        }

        // Try to improve selection using bounded optimal subset-sum search
        List<UtxoDto>? selected = null;
        long totalSelected = 0;
        var optimal = SelectBestUtxosOptimal(utxos!, needed, maxUtxos: 12, maxIterations: 200000);
        if (optimal != null && optimal.Any())
        {
            selected = optimal;
            totalSelected = selected.Sum(s => s.AmountSatoshis);
        }
        else
        {
            Assert.True(bestTotal != long.MaxValue && bestTotal >= needed, "Not enough UTXO balance to cover target+fee");
            selected = bestSelected;
            totalSelected = bestTotal;
        }

        // 4) request a change address from the service (use m/1/0/0)
        var changeAddrReq = new { derivationPath = "m/1/0/0" };
        var changeResp = await client.PostAsync("/api/addresses", new StringContent(JsonConvert.SerializeObject(changeAddrReq), Encoding.UTF8, "application/json"));
        changeResp.EnsureSuccessStatusCode();
        var changeBody = await changeResp.Content.ReadAsStringAsync();
        var changeDto = JsonConvert.DeserializeObject<AddressDto>(changeBody);
        Assert.NotNull(changeDto);

        // Build unsigned tx with multiple inputs and outputs (target + change)
        var tx = network.CreateTransaction();
        foreach (var s in selected)
        {
            var outpoint = new NBitcoin.OutPoint(new NBitcoin.uint256(s.TxId), s.Vout);
            tx.Inputs.Add(new NBitcoin.TxIn(outpoint));
        }

        // send to target (in this test we send back to receive address)
        tx.Outputs.Add(new NBitcoin.TxOut(NBitcoin.Money.Satoshis(targetAmount), NBitcoin.BitcoinAddress.Create(created.FirstAddress, network)));

        var change = totalSelected - targetAmount - feePerTx;
        if (change >= dustThreshold)
        {
            tx.Outputs.Add(new NBitcoin.TxOut(NBitcoin.Money.Satoshis(change), NBitcoin.BitcoinAddress.Create(changeDto!.Address, network)));
        }
        else
        {
            // absorb small change into fee
            feePerTx += change;
            change = 0;
        }

        // Build inputsToSign array for the service (assumes all UTXOs are from the first receive address m/0/0/0)
        var inputsToSign = selected.Select((s, i) => new { inputIndex = i, derivationPath = "m/0/0/0", scriptPubKeyHex = s.ScriptPubKeyHex, amountSatoshis = s.AmountSatoshis }).ToArray();

        var signatureReq = new
        {
            unsignedTransactionHex = tx.ToHex(),
            inputsToSign = inputsToSign
        };

        var sigJson = JsonConvert.SerializeObject(signatureReq);
        var sigResp = await client.PostAsync("/api/signatures", new StringContent(sigJson, Encoding.UTF8, "application/json"));
        sigResp.EnsureSuccessStatusCode();
        var sigBody = await sigResp.Content.ReadAsStringAsync();
        var sigResult = JsonConvert.DeserializeObject<SignatureRespDto>(sigBody);
        Assert.NotNull(sigResult);
        Assert.False(string.IsNullOrWhiteSpace(sigResult!.SignedTransactionHex));

        // 6) broadcast signed tx
        var bcResp = await http.PostAsync(broadcastUrl, new StringContent(JsonConvert.SerializeObject(new { rawtx = sigResult.SignedTransactionHex }), Encoding.UTF8, "application/json"));
        if (!bcResp.IsSuccessStatusCode)
        {
            // fallback to plain text post
            bcResp = await http.PostAsync(broadcastUrl, new StringContent(sigResult.SignedTransactionHex, Encoding.UTF8, "text/plain"));
        }

        bcResp.EnsureSuccessStatusCode();
    }

    record UtxoDto(string TxId, int Vout, long AmountSatoshis, string ScriptPubKeyHex);

    record AddressDto(string Address, string DerivationPath);

    record CreateWalletDto(string Mnemonic, string XPrv, string XPub, string FirstAddress, string Network);
    record VerifyWalletDto(bool mnemonicMatchesXPrv, bool xPrvMatches, bool xPubMatches, bool addressMatches, string derivedXPrv, string derivedXPub, string derivedFirstAddress, string message);
    record SignatureRespDto(string SignedTransactionHex);
}
