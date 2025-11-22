# cryptoMixer – Suivi technique

Ce fichier liste ce qui a déjà été réalisé techniquement et ce qu’il reste à faire pour le sous-projet `WalletManagementService` et la base de code.

## ✅ Réalisé

- Création de la structure de projet :
  - `projects/cryptoMixer/docs/`
  - `projects/cryptoMixer/src/Shared/`
  - `projects/cryptoMixer/src/WalletManagementService/`
- Mise en place de `Shared` :
  - Projet `Shared.csproj` (net8.0).
  - Type de résultat commun `Result` / `Result<T>`.
- Mise en place du microservice `WalletManagementService` :
  - Projet `WalletManagementService.csproj` (ASP.NET Core, net8.0) avec références à `NBitcoin` et `VaultSharp`.
  - `Program.cs` minimal mais robuste : controllers, Swagger, health checks, DI, middleware global d’erreurs.
  - `GlobalExceptionHandlerMiddleware` pour capturer les exceptions et renvoyer une erreur JSON neutre.
  - Services :
    - `IVaultService` / `VaultService` pour l’accès à HashiCorp Vault (seed HD + moteur Transit pour la signature).
    - `IWalletService` / `WalletService` pour la logique métier (génération d’adresses HD, orchestration de la signature).
  - Modèles (DTOs) : `AddressRequest`, `AddressResponse`, `SignatureRequest`, `InputToSign`, `SignatureResponse`.
  - Contrôleur `WalletController` avec les endpoints internes :
    - `POST /api/addresses` pour générer une adresse HD.
    - `POST /api/signatures` pour signer une transaction.
- README de base dans `projects/cryptoMixer/README.md` pour décrire la structure.

## 🔜 À faire / points durs

- Configuration Vault (environnement de dev/test) :
  - [ ] Définir et documenter `VAULT_ADDR` (URL du serveur Vault).
  - [ ] Définir et documenter `VAULT_TOKEN` (token d’auth utilisé par le service en dev/test).
  - [ ] Créer le secret `kv/data/wms/masterkey` avec la clé `seed` (seed HD au bon format pour NBitcoin ou phrase BIP-39 si supportée côté code).
  - [ ] Créer/paramétrer les clés Transit associées (`key-…`) côté Vault, avec les bonnes policies.
- Génération d’adresses (/api/addresses) :
  - [ ] Remplacer le bypass in-memory temporaire de `EnsureMasterKeyLoadedAsync` par le chargement réel de la master key depuis Vault.
  - [ ] Documenter clairement le format attendu de la master key (xprv/tprv ou phrase BIP-39) et l’algorithme de dérivation (chemins, réseau).
  - [ ] Ajouter un petit jeu de tests automatisés pour vérifier la cohérence des adresses générées (différents derivation paths).
- Durcissement de la signature des transactions :
  - [ ] Implémenter la conversion de la signature Vault (format `vault:v1:...`) en signature ECDSA brute / DER utilisable par NBitcoin.
  - [ ] Injecter effectivement les signatures dans `Transaction` (ScriptSig / witness) pour un cas de base (P2WPKH sur testnet).
  - [ ] Ajouter des tests ciblés sur la fonction de signature (chemin heureux sur testnet).
- Robustesse & ergonomie :
  - [ ] Rendre le réseau (mainnet/testnet) configurable via settings.
  - [ ] Améliorer la gestion d’erreurs pour les appels Vault (timeouts, indisponibilité, logs plus riches).
  - [ ] Ajouter un document dans `docs/` décrivant le protocole d’intégration WMS (schémas mermaid, séquence, exemples d’appels) et le protocole de validation end-to-end (/api/addresses + harness).

Ce fichier doit être mis à jour à chaque évolution significative du sous-projet afin de garder une vision claire de l’avancement technique.
