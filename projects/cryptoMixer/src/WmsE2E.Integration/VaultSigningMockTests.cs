using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoMixer.WalletManagementService.Services;
using CryptoMixer.WalletManagementService.Models;
using CryptoMixer.Shared;
using Microsoft.Extensions.Configuration;
using NBitcoin;
using Xunit;

namespace WmsE2E.Integration.Tests
{
    public class VaultSigningMockTests
    {
        private class MockVaultService : IVaultService
        {
            private ExtKey _master;
            private readonly Dictionary<string, Key> _map = new();

            public MockVaultService(ExtKey master)
            {
                _master = master;
            }

            public bool Disabled => false;

            public Task<ExtKey> GetMasterKeyAsync() => Task.FromResult(_master);

            public Task<ExtPubKey> GetMasterXPubAsync() => Task.FromResult(_master.Neuter());

            public Task SetMasterKeyAsync(ExtKey extKey)
            {
                _master = extKey;
                return Task.CompletedTask;
            }

            public void RegisterKeyForPath(string derivationPath)
            {
                var keyName = derivationPath.Replace('/', '-').Replace("m", "key");
                var k = _master.Derive(new KeyPath(derivationPath)).PrivateKey;
                _map[keyName] = k;
            }

            public Task<string> SignTransactionHashAsync(string keyName, string hashToSignBase64)
            {
                if (!_map.TryGetValue(keyName, out var key))
                    throw new KeyNotFoundException(keyName);

                var hashBytes = System.Convert.FromBase64String(hashToSignBase64);
                var h = new uint256(hashBytes);
                var sig = key.Sign(h);
                var der = sig.ToDER();
                return Task.FromResult(System.Convert.ToBase64String(der));
            }
        }

        [Fact]
        public async Task VaultEnabledSigning_WithMockedVault_Succeeds()
        {
            // Arrange
            var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            var master = mnemonic.DeriveExtKey();
            var derivation = "m/0/0/0";
            var keyName = derivation.Replace('/', '-').Replace("m", "key");

            var amount = 100000L;

            // Build previous transaction that pays to the derived pubkey (P2PKH)
            var pub = master.Neuter().Derive(new KeyPath(derivation)).PubKey;
            var prevTx = Network.Main.CreateTransaction();
            prevTx.Outputs.Add(new TxOut(Money.Satoshis(amount), pub.Hash.ScriptPubKey));

            // Build unsigned transaction that spends prevTx:0
            var unsigned = Network.Main.CreateTransaction();
            unsigned.Inputs.Add(new TxIn(new OutPoint(prevTx.GetHash(), 0)));
            unsigned.Outputs.Add(new TxOut(Money.Satoshis(amount - 1000), new Key().PubKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main)));

            var request = new SignatureRequest(unsigned.ToHex(), new List<InputToSign>
            {
                new InputToSign(0, derivation, prevTx.Outputs[0].ScriptPubKey.ToHex(), amount)
            });

            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string?>{{"Wms:Network","main"}}).Build();
            var mock = new MockVaultService(master);
            mock.RegisterKeyForPath(derivation);

            var svc = new CryptoMixer.WalletManagementService.Services.WalletService(mock, config);

            // Act
            var res = await svc.SignTransactionAsync(request);

            // Assert
            Assert.True(res.IsSuccess, res.Error);
            Assert.False(string.IsNullOrWhiteSpace(res.Value!.SignedTransactionHex));

            var signedTx = Transaction.Parse(res.Value.SignedTransactionHex, Network.Main);

            // Verify signature
            var builder = Network.Main.CreateTransactionBuilder();
            builder.AddCoins(new Coin(unsigned.Inputs[0].PrevOut, prevTx.Outputs[0]));
            var ok = builder.Verify(signedTx);
            Assert.True(ok, "Signed transaction did not verify");
        }
    }
}
