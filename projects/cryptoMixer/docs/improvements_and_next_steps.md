# cryptoMixer – Améliorations & Next Steps

Ce fichier regroupe toutes les recommandations et idées d’évolution pour le projet `cryptoMixer`, afin de garder une trace centralisée sans perdre de pistes.

---

## 1. WalletManagementService (WMS)

### 1.1. Signature & Vault

- [ ] **Courbe elliptique réelle (secp256k1)**
  - Actuellement, les exemples Transit utilisent `ecdsa-p256`. Pour un environnement réaliste, utiliser un moteur de signature qui supporte `secp256k1` (plugin Transit, HSM dédié Bitcoin, ou service externe) et adapter la conversion `(r || s)` → DER.
- [ ] **Support d’autres scripts que P2WPKH**
  - Étendre `WalletService` à :
    - P2PKH
    - P2SH(P2WPKH)
    - P2TR (Taproot) à terme
  - Gérer les witness / ScriptSig appropriés pour chaque type.
- [ ] **Rotation des clés Transit**
  - Gérer les versions (`key-m-0-0-0` v1, v2, etc.) et la migration progressive des chemins HD.
- [ ] **Gestion avancée des erreurs Vault**
  - Timeouts, retries exponentiels, classification des erreurs (réseau vs. logique).
  - Logging structuré des erreurs (correlation ID, inputIndex, keyName).

### 1.2. Réseau & configuration

- [ ] **Rendre le réseau configurable** (mainnet / testnet / regtest) via `appsettings.*.json` ou variables d’environnement (`BITCOIN_NETWORK`).
- [ ] **Séparer les seeds par environnement** :
  - Seed et dérivations différentes pour dev / test / prod.
- [ ] **Validation stricte des derivation paths**
  - Valider le format (`m/x/y/z`) et rejeter les chemins anormaux.

### 1.3. Tests & qualité

- [ ] **Tests unitaires WalletService**
  - Mock d’`IVaultService` pour simuler signatures et erreurs (format incorrect, token expiré…).
- [ ] **Tests d’intégration WMS + Vault (dev)**
  - Scénarios bout-en-bout avec Vault dev : génération d’adresse + signature testnet.
- [ ] **Vérification cryptographique complète côté harness**
  - Reconstruire le scriptPubKey, dériver la clé publique, et vérifier la signature avec NBitcoin (`Verify`), plutôt que de vérifier seulement la présence du witness.

---

## 2. Vault & sécurité

- [ ] **Passer du mode dev à un déploiement "réaliste"**
  - Instance Vault non-dev (stock backend file ou autre), TLS configuré, token root désactivé.
- [ ] **Policies plus fines**
  - Cloisonner davantage :
    - un rôle lecture seule KV
    - un rôle signature Transit
- [ ] **Audit & logs Vault**
  - Activer l’audit backend pour tracer qui signe quoi et quand.
- [ ] **Gestion des secrets côté OS / orchestrateur**
  - Utiliser un gestionnaire sécurisé (ex : variables d’env chiffrées, secret store de l’orchestrateur) pour injecter `VAULT_TOKEN` / `VAULT_ADDR`.

---

## 3. Orchestration & autres services

- [ ] **Créer l’OrchestrationService**
  - Service central qui :
    - interagit avec WMS pour les adresses et signatures,
    - implémente l’algorithme de mixage (buckets, timing, montants, chemins).
- [ ] **API d’Ingress**
  - Endpoint public pour recevoir les demandes utilisateur (montant, adresses de sortie, préférences de délai) et créer un contexte de mixage.
- [ ] **Service d’interaction blockchain**
  - En charge de :
    - récupérer les UTXOs,
    - construire les transactions brutes,
    - les envoyer à WMS pour signature,
    - les diffuser sur le réseau (testnet dans un premier temps).

---

## 4. Documentation & DX

- [ ] **Compléter `vault_setup.md` avec des exemples par environnement** (dev/test/prod) et un tableau récapitulatif des paths et policies.
- [ ] **Guide développeur**
  - Comment cloner, builder, lancer WMS + harness + Vault en 10 minutes.
- [ ] **Diagrammes d’architecture** (mermaid) dans `docs/` :
  - Flow complet : Ingress → Orchestration → WMS → Vault → Blockchain.

---

## 5. Stratégie globale

- [ ] **Revalider la trajectoire produit / compliance**
  - En se basant sur `projects/cryptoMixer/juridique.md` et `plan_action.md` (risques AML/CTF, options "Version Safe" type CoinJoin, etc.).
- [ ] **Scinder clairement les modes**
  - Mode recherche / labo (testnet uniquement, pas de fonds réels).
  - Mode produit éventuel, qui ne serait lancé qu’après validation juridique stricte.

Ce fichier doit être mis à jour régulièrement par l’Architecte IA / CTO pour garder une vision claire de la roadmap technique.
