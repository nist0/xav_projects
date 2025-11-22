# cryptoMixer – Codebase

Ce dossier contient l’implémentation technique progressive du projet **cryptoMixer**.

- `docs/` : documents techniques dérivés des phases (spécifications détaillées).
- `src/Shared/` : composants communs (types de résultats, helpers de config, logging de base…).
- `src/WalletManagementService/` : premier sous-projet .NET 8 pour le **WalletManagementService (WMS)**, coeur sécuritaire du système.

Les règles d’architecture et d’ingénierie IA sont alignées sur :
- `instructions/instructions_ia_engineering_advanced.md`
- `teams/team_aps_optimised.md`

La première cible est un microservice interne : génération d’adresses HD et signature de transactions via un HSM externe (Vault).
