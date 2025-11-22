# WalletManagementService

Petit service ASP.NET Core pour la gestion de wallet (génération d'adresses HD, future signature) avec HashiCorp Vault.

## Configuration Vault

Les paramètres Vault sont lus dans l'ordre suivant :

1. **Section `Vault` de la configuration** (`appsettings*.json`, variables d'env `Vault__Address`, `Vault__Token`)
2. **Variables d'environnement** `VAULT_ADDR` et `VAULT_TOKEN`

### Fichiers de config

Dans ce projet, les fichiers suivants existent :

- `appsettings.json`
- `appsettings.Development.json`

Exemple minimal (déjà présent) :

```json
{
  "Vault": {
    "Address": "http://127.0.0.1:8200",
    "Token": "DEV_ROOT_TOKEN_PLACEHOLDER"
  }
}
```

Adapte `Address` et `Token` à ton environnement local (dev Vault).

## Workflow de dev local

1. Démarrer Vault en mode dev (dans un terminal séparé) et créer la master key HD dans KV v2 :
   - mount par défaut : `secret/`
   - chemin : `wms/masterkey`

2. Dans le repo `projects/cryptoMixer`, configure une fois pour toutes le fichier `.env.local` :

   ```env
   VAULT_ADDR=http://127.0.0.1:8200
   VAULT_TOKEN=<TON_VRAI_TOKEN_DEV>
   ```

3. À **chaque nouveau terminal PowerShell** pour bosser sur le projet :

   ```powershell
   cd C:\Users\yanis\source\repos\xav_projects\projects\cryptoMixer
   . .\load-env.ps1
   ```

   Ceci charge automatiquement les variables d'environnement de `.env.local` dans ta session :

   - `VAULT_ADDR`
   - `VAULT_TOKEN`

4. Lancer le service :

   ```powershell
   cd .\src\WalletManagementService
   dotnet run
   ```

5. Tester l'endpoint `/api/addresses` depuis un autre terminal (qui aura aussi chargé l'env via `load-env.ps1`) :

   ```powershell
   $baseUrl = "http://localhost:5000"  # adapte si besoin

   try {
       $resp = Invoke-WebRequest -Uri "$baseUrl/api/addresses" `
                                 -Method POST `
                                 -ContentType "application/json" `
                                 -Body '{ "derivationPath": "m/0/0/0" }'

       $resp.StatusCode
       $resp.Content
   } catch [System.Net.WebException] {
       $webEx = $_.Exception
       if ($webEx.Response) {
           $reader = New-Object System.IO.StreamReader($webEx.Response.GetResponseStream())
           $body = $reader.ReadToEnd()
           $webEx.Status
           $body
       } else {
           $_ | Format-List * -Force
       }
   } catch {
       $_ | Format-List * -Force
   }
   ```

## Résumé

- Les paramètres Vault peuvent venir des `appsettings` ou des variables d'environnement.
- `.env.local` + `load-env.ps1` évitent de retaper `VAULT_ADDR`/`VAULT_TOKEN` à chaque fois.
- Toujours lancer `load-env.ps1` au début de chaque nouvelle session PowerShell avant de démarrer le service.
