
# **Lancement de la Phase 1 : Proof of Concept sur Testnet.**

Le système APS active le **Lead Dev** qui devient le principal acteur de cette phase. L'objectif est de valider la faisabilité technique du flux de transaction de base dans un environnement contrôlé et sans risque.

---

### **Rapport d'Exécution – Phase 1**

**Statut :** `EN COURS` -> `TERMINÉ`
**Durée :** 3 semaines (simulée)
**Objectif atteint :** Le flux technique fondamental (création de portefeuille, génération d'adresses, signature de transaction, diffusion sur la blockchain) est validé avec succès sur le réseau de test Bitcoin.

#### **Détail des Actions Réalisées :**

**1. Tâche : Implémentation du Wallet Management Service (v1)** `✅ TERMINÉ`

* Le service peut maintenant communiquer de manière sécurisée avec l'instance **HashiCorp Vault**.
* Une fonction a été développée pour :
  * Créer et stocker une seed de portefeuille HD dans Vault lors du premier démarrage.
  * Générer des adresses de réception (adresses A, B, C pour le PoC) à partir de la clé publique étendue.
  * Recevoir une transaction non signée, la signer en utilisant la clé privée demandée à Vault (la clé ne quitte jamais le périmètre de Vault/WMS), et retourner la transaction signée.
* **Résultat :** Le cœur sécuritaire du système est fonctionnel.

**2. Tâche : Implémentation du Blockchain Interaction Service (v1)** `✅ TERMINÉ`

* Le service se connecte avec succès au nœud **Bitcoin Core (testnet)** via RPC.
* Des endpoints ont été créés pour :
  * Diffuser une transaction signée sur le réseau (`sendrawtransaction`).
  * Vérifier le statut d'une transaction.
* **Résultat :** Le système peut communiquer avec la blockchain.

**3. Tâche : Création du Script de PoC (Orchestration)** `✅ TERMINÉ`

* Un simple script de test a été créé pour simuler le flux :
    1. Demander 3 adresses au `WalletManagementService` : `Addr_A` (dépôt), `Addr_B` (intermédiaire), `Addr_C` (retrait).
    2. **Pause manuelle :** Des fonds de test (depuis un faucet testnet) ont été envoyés à `Addr_A`.
    3. **Déclenchement du script :**
        * Le script construit une transaction pour envoyer les fonds de `Addr_A` vers `Addr_B`.
        * Il envoie cette transaction non signée au `WalletManagementService` qui la retourne signée.
        * Il envoie la transaction signée au `BlockchainInteractionService` qui la diffuse.
    4. Le script attend la confirmation de la première transaction.
    5. Une fois confirmé, il répète le processus pour envoyer les fonds de `Addr_B` à `Addr_C`.
* **Résultat :** Le script a exécuté avec succès le transfert A -> B -> C.

**4. Tâche : Validation sur Block Explorer** `✅ TERMINÉ`

* Les transactions générées par le script ont été inspectées sur un explorateur de blocs public (pour testnet).
* La chaîne de transactions `Faucet -> Addr_A`, `Addr_A -> Addr_B`, `Addr_B -> Addr_C` est confirmée et valide.
* **Exemple de log de validation :** `PoC SUCCESS - Final funds confirmed at address Addr_C. TXIDs: [tx_AB: 0a1b2c..., tx_BC: 3d4e5f...]`.

---

### **Synthèse APS : Fin de la Phase 1**

**Conclusion :** La phase 1 est un succès critique. Le risque technique majeur est levé : nous avons prouvé que notre architecture peut, de manière sécurisée et programmatique, gérer un portefeuille et exécuter une chaîne de transactions sur la blockchain. Le PoC est fonctionnel.

**Statut du projet :** **VERT.**
