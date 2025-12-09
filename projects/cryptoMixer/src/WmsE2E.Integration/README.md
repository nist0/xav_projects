WmsE2E.Integration - Live funded test

How to run the conditional live funded sign+broadcast integration test

- The test `FullSignAndBroadcast_Conditional` runs only when these environment variables are set:
  - `FAUCET_URL` - POST endpoint that funds an address. The test posts `{ "address": "<addr>", "amountSatoshis": 25000 }` as JSON.
  - `BLOCK_EXPLORER_UTXO_URL` - GET endpoint that returns UTXOs for an address. The test calls `GET {BLOCK_EXPLORER_UTXO_URL}?address=<addr>` and expects a JSON array of objects: `{ "TxId": "<hex>", "Vout": 0, "AmountSatoshis": 25000, "ScriptPubKeyHex": "<hex>" }`.
  - `BROADCAST_URL` - POST endpoint used to broadcast a signed transaction. The test first posts `{ "rawtx": "<signed-hex>" }` as JSON, and if that fails it falls back to posting the raw hex as `text/plain`.

Optional configuration env vars (defaults shown):
- `TEST_TARGET_SATOSHIS` - satoshis to send in the test (default `15000`).
- `TEST_FEE_SATOSHIS` - fee to reserve for the tx (default `1000`).
- `TEST_DUST_SATOSHIS` - minimum change threshold (default `1000`). If computed change is below this, it will be absorbed into the fee.

Example (PowerShell) - run tests with a configured faucet/explorer/broadcaster:

```powershell
# $env:FAUCET_URL = "https://your-faucet.example/api/fund"
# $env:BLOCK_EXPLORER_UTXO_URL = "https://your-explorer.example/api/utxos"
# $env:BROADCAST_URL = "https://your-broadcaster.example/api/broadcast"
# $env:TEST_TARGET_SATOSHIS = "15000"
# $env:TEST_FEE_SATOSHIS = "1000"
# dotnet test .\projects\cryptoMixer\src\WmsE2E.Integration\WmsE2E.Integration.csproj
```

Notes
- The test is intended for ephemeral CI runs against testnet faucets and broadcasters you control. It will attempt to sign using the in-memory master key (the service runs with `Vault__Disabled=true` during tests).
- Make sure the faucet provides sufficient confirmations/UTXO visibility via your explorer endpoint within the test polling timeout (~3 minutes).

If you need the test to use API keys or custom headers for the faucet/block-explorer/broadcaster endpoints, provide the header shape and I can update the test to send them.
