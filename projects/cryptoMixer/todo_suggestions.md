# Suggestions TODOs for cryptoMixer

This file captures suggested next tasks and improvements organized under a `Suggestions` category.

## Suggestions

- **Expose coin-selection caps via env:** Add `COINSEL_MAX_UTXOS` and `COINSEL_MAX_ITERS` environment variables to tune the bounded optimal subset-sum search at runtime.
- **DP knapsack fallback:** Implement a dynamic-programming knapsack-based coin selection fallback for larger UTXO sets (bounded sums) to improve optimality when optimal DFS is impractical.
- **Change address indexing:** Derive change addresses using `m/1/0/<index>` and implement a simple index allocator in the service so tests can request the next unused change address.
- **API header management in tests:** Allow per-endpoint header configuration for faucet/explorer/broadcast (already supported) and add examples for common providers (headers + auth shapes).
- **Vault Transit signing integration:** Implement Vault Transit signing in `VaultService` and wire it into `WalletService.SignTransactionAsync` for production-safe signing (avoid exposing private keys locally).
- **Remove mnemonic/xprv in non-dev:** Harden APIs to never return mnemonic or xprv unless `ASPNETCORE_ENVIRONMENT=Development` (or a clear feature flag), and document the risk.
- **Add CI job template:** Provide a GitHub Actions job example that runs the live funded test against a controlled testnet faucet/broadcaster with secrets and timeouts.
- **Add more script-type tests:** Extend integration tests to cover P2PKH, P2WPKH, P2SH-P2WPKH, and multisig signing with real prevouts (or simulated funded UTXOs).
- **Improve logging and observability:** Add structured logging (Serilog) and correlation IDs for E2E runs to make troubleshooting easier.
- **Security checklist:** Add a small checklist to the repo for production readiness: secret management, key rotation, audit logging, and least-privilege Vault policies.

## Notes

- These suggestions are prioritized for security and test reliability. If you'd like, I can pick one to implement next and add it to the active TODO workflow.
