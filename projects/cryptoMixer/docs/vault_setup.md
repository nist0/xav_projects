# Vault – Setup de base pour WalletManagementService (dev / testnet)

Ce document décrit une configuration **minimale** de HashiCorp Vault pour faire tourner le `WalletManagementService` (WMS) en environnement de développement / testnet.

> ⚠️ **Important** : tout ce qui suit est destiné au **dev local**. Ne jamais utiliser ces réglages (mode dev, token root, clés non protégées) en production.

---

## 1. Prérequis

- Vault installé en local (binaire `vault` ou container).
- .NET 8 SDK installé.
- Le projet `WalletManagementService` compilable (voir `projects/cryptoMixer/README.md`).

---

## 2. Démarrer Vault en mode dev

Dans un premier terminal PowerShell :

```powershell
vault server -dev -dev-root-token-id=root
```

- Adresse par défaut : `http://127.0.0.1:8200`
- Token root : `root`

Dans un second terminal, configure les variables d’environnement :

```powershell
$env:VAULT_ADDR="http://127.0.0.1:8200"
$env:VAULT_TOKEN="root"
```

Ces variables sont utilisées **à la fois** par les commandes `vault` et par le service .NET.

---

## 3. Activer les moteurs `kv` et `transit`

Active un moteur KV v2 et le moteur Transit :

```powershell
vault secrets enable -path=kv kv-v2
vault secrets enable transit
```

- `kv` : stockage de la clé maîtresse HD (`wms/masterkey`).
- `transit` : proxy de signature (WMS lui envoie un hash à signer).

---

## 4. Créer la master key HD (wallet racine)

### 4.1. Générer une clé HD de test (tprv)

Pour l’instant, la forme attendue par le code est une **clé étendue HD** (ex : `tprv...`).
Tu peux la générer avec un petit programme NBitcoin ou un outil externe fiable.

Exemple en pseudo-code (C#) :

```csharp
var network = Network.TestNet;
var master = new ExtKey();
Console.WriteLine(master.ToString(network)); // tprv...
```

Copie la valeur `tprv...` obtenue.

### 4.2. Stocker la clé dans Vault

En supposant que ta tprv est `tprvEXEMPLE...` :

```powershell
vault kv put kv/wms/masterkey seed="tprvEXEMPLE..."
```

Le code WMS lira cette valeur via le path v2 : `kv/data/wms/masterkey` et la clé `seed`.

---

## 5. Créer les clés Transit pour la signature

Le `WalletService` dérive un nom de clé Transit à partir du chemin HD :

- Chemin : `m/0/0/0`
- Nom de clé Transit : `key-m-0-0-0`

Formule :

```text
keyName = "key-" + derivationPath.Replace("/", "-")
```

Pour commencer, crée quelques clés Transit pour des chemins de test :

```powershell
vault write transit/keys/key-m-0-0-0 type="ecdsa-p256"
vault write transit/keys/key-m-0-0-1 type="ecdsa-p256"
```

> 💡 Remarque : Bitcoin utilise `secp256k1`, pas `p256`. Ici, on reste dans un modèle **conceptuel de dev** où Transit renvoie une signature brute `(r || s)` de 64 octets. En production, il faudra un HSM/Transit compatible `secp256k1` ou une autre approche de gestion des clés.

---

## 6. Créer une policy et un token dédiés (hors root)

Même en dev, évite d’utiliser toujours le token root.

### 6.1. Écrire la policy `wms`

Crée un fichier `wms-policy.hcl` (où tu veux, par exemple dans `projects/cryptoMixer/docs/`) avec le contenu suivant :

```hcl
path "kv/data/wms/masterkey" {
  capabilities = ["read"]
}

path "transit/sign/key-*" {
  capabilities = ["update"]
}
```

Puis applique la policy :

```powershell
vault policy write wms wms-policy.hcl
```

### 6.2. Créer un token pour WMS

```powershell
vault token create -policy="wms" -display-name="wallet-management-service"
```

Vault renvoie un token de la forme `s.XXXX`. Note-le : ce sera ton `VAULT_TOKEN` pour le service.

---

## 7. Brancher Vault au WalletManagementService

Dans le terminal où tu lanceras le service .NET :

```powershell
$env:VAULT_ADDR="http://127.0.0.1:8200"
$env:VAULT_TOKEN="s.XXXX"    # token créé à l’étape 6.2
```

Puis démarre le service :

```powershell
cd c:\Users\yanis\source\repos\xav_projects\projects\cryptoMixer\src\WalletManagementService

dotnet restore
dotnet run
```

L’API devrait être accessible (par défaut) sur `https://localhost:5xxx` avec :

- `GET /health`
- Swagger UI sur `/swagger`.

---

## 8. Smoke tests WMS

### 8.1. Générer une adresse

Requête HTTP :

```http
POST /api/addresses
Content-Type: application/json

{
  "derivationPath": "m/0/0/0"
}
```

Réponse attendue (exemple) :

```json
{
  "address": "tb1q...",
  "derivationPath": "m/0/0/0"
}
```

### 8.2. Signer une transaction (cas P2WPKH testnet)

Une fois que tu as une transaction brute non signée et son `scriptPubKeyHex` + `amountSatoshis`, tu peux appeler :

```http
POST /api/signatures
Content-Type: application/json

{
  "unsignedTransactionHex": "01000000...",
  "inputsToSign": [
    {
      "inputIndex": 0,
      "derivationPath": "m/0/0/0",
      "scriptPubKeyHex": "0014...",
      "amountSatoshis": 100000
    }
  ]
}
```

Le service :

1. Calcule le hash de signature Bitcoin (SigHash.All).
2. L’envoie à `transit/sign/key-m-0-0-0`.
3. Décode la signature renvoyée par Transit.
4. Injecte la signature + clé publique dérivée dans le witness (P2WPKH).
5. Retourne la transaction signée (`signedTransactionHex`).

---

## 9. Points à surveiller / évolutions futures

- **Courbe elliptique réelle** : pour aller vers un environnement sérieux, il faudra un moteur de signature compatible `secp256k1` (plugin Transit, HSM externe, ou code NBitcoin protégé). Le schéma actuel est conçu pour le prototypage.
- **Rotation de clés** : prévoir la gestion de versions de clés Transit (changement de dérivation et de `keyName`).
- **Séparation envs** : avoir des mounts séparés pour dev / test / prod (ex : `kv-dev`, `kv-prod`, etc.) et des policies adaptées.

Ce document suffit pour mettre en route un environnement de dev fonctionnel pour le `WalletManagementService` et tester la génération d’adresses + la signature basique sur testnet.
