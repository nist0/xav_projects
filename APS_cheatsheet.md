# APS OS – Cheat Sheet Lead Dev & APS (v1)

Ce fichier résume comment interagir avec ton système APS + Lead Dev pour tes demandes les plus fréquentes. Il sera maintenu et enrichi au fil de l’usage.

---

## 1. Principes de base

- **Tu es le Fondateur / Chef d’orchestre.**
- **APS (@workspace)** :
  - Clarifie ta demande.
  - Rappelle le contexte projet (docs, `plan_action.md`, `todo.md`).
  - Route vers les bons rôles (Lead Dev, CTO, Architecte IA, PMO, etc.).
  - Tient à jour la documentation (avec l’aide du Lead Dev).
- **Lead Dev (mode Agent)** :
  - Analyse erreurs / code.
  - Modifie directement les fichiers (`.cs`, `.md`, config, etc.).
  - Lance build/tests (via terminal dans VS Code si nécessaire).
  - Ne te renvoie pas le travail manuel de patch.

Quand tu dis : **« tu es mon lead dev »**, cela implique :

- Le Lead Dev :
  - applique lui-même les changements de code et de docs nécessaires,
  - utilise l’APS pour consigner ce qui a été fait et mettre à jour les fichiers de suivi.

---

## 2. Demandes fréquentes et phrases clés

### 2.1. Corriger une erreur de build ou de runtime

**Ce que tu peux dire :**

> « Tu es mon lead dev, corrige cette erreur de build/runtime à ma place. Voici la sortie de `dotnet run`/`dotnet build`. »

**Ce que fait le Lead Dev (automatique) :**

1. Lit la sortie d’erreur.
2. Ouvre les fichiers concernés (`*.cs`, `*.csproj`, etc.).
3. Modifie directement le code avec les bons outils.
4. Si besoin, met à jour `projects/cryptoMixer/todo.md` ou `plan_action.md` (avec l’APS).
5. Te renvoie :
   - les fichiers modifiés,
   - un résumé de ce qui a changé,
   - ce qu’il faut relancer (`dotnet run`, tests, etc.).

### 2.2. Implémenter une nouvelle fonctionnalité

**Ce que tu peux dire :**

> « En tant que lead dev, implémente tel endpoint / telle fonctionnalité dans `WalletManagementService` en respectant l’architecture actuelle. Documente ce que tu fais. »

**Ce que fait le Lead Dev :**

1. Vérifie les specs dans :
   - `projects/cryptoMixer/plan_action.md`,
   - `projects/cryptoMixer/phase*.md` et docs associées.
2. Crée / modifie :
   - contrôleurs (`Controllers/*.cs`),
   - services (`Services/*.cs`),
   - modèles (`Models/*.cs`),
   - docs (`docs/*.md` si nécessaire).
3. Met à jour :
   - `projects/cryptoMixer/todo.md` (tâches réalisées/restantes),
   - éventuellement un `ADR` si la décision est structurante.
4. Recommende les commandes à lancer pour tester.

### 2.3. Mettre à jour la doc et le suivi

**Ce que tu peux dire :**

> « APS, mets à jour tout ce qu’il faut (todo, plan_action, docs) pour refléter ce qu’on vient de faire sur `WalletManagementService`. »

**Ce que fait l’APS, avec l’aide implicite du Lead Dev :**

1. Utilise les infos de la session (logs, modifications de code) pour :
   - cocher / créer des items dans `projects/cryptoMixer/todo.md`,
   - ajuster `projects/cryptoMixer/plan_action.md` et les phases (`phaseX.md`) si nécessaire,
   - éventuellement compléter `docs/vault_setup.md` ou d’autres guides techniques.
2. Te fournit un **mini-rapport** :
   - ce qui a été fait,
   - ce qui reste à faire,
   - risques / points de vigilance.

---

## 3. Rôles et responsabilités (version courte)

### 3.1. Toi (Fondateur / Chef d’orchestre)

- Donne l’intention, la vision, les priorités.
- Valide les décisions structurantes (via APS + DGD + CTO/Architecte IA).
- Ne s’occupe pas des détails d’implémentation, sauf curiosité.

### 3.2. APS (@workspace)

- Clarifie les demandes.
- Rappelle le contexte et les décisions passées.
- Coordonne Lead Dev, CTO, Architecte IA, PMO, etc.
- Tient la mémoire opérationnelle : `todo.md`, `plan_action.md`, `phaseX.md`, docs.

### 3.3. Lead Dev (mode Agent)

- Fait les changements de code et de configuration lui-même.
- Ne renvoie jamais vers toi des tâches du type : « modifie tel fichier / applique ce patch à la main ».
- Utilise l’APS pour :
  - savoir quoi faire (contexte),
  - consigner ce qui a été fait (suivi).

---

## 4. Exemples de prompts prêts à l’emploi

### 4.1. Debug build .NET

> « Tu es mon lead dev. Voici la sortie de `dotnet run` dans `WalletManagementService`. Analyse et corrige le code à ma place, puis dis-moi ce que je dois relancer pour vérifier. »

### 4.2. Validation d’un flow end-to-end

> « En tant que lead dev, assure-toi que le flow `/api/addresses` est opérationnel end-to-end (Vault KV + WMS + harness). Modifie le code et la doc si nécessaire, et donne-moi une checklist de validation. »

### 4.3. Mise à jour globale de la doc

> « APS, mets à jour `projects/cryptoMixer/todo.md`, `plan_action.md` et les docs associées pour refléter toutes les modifications que tu viens de faire sur `WalletManagementService`. Fais un résumé en 10 lignes max. »

---

## 5. Rappel : ce qui est dans tes prérogatives quand tu parles à l’Agent

Quand tu t’adresses à ton système IA en disant :

- « tu es mon lead dev »
- « tu es mon APS »

alors, par convention :

- L’Agent :
  - prend en charge **toutes** les modifications de code ou de docs qu’il peut raisonnablement effectuer dans le repo courant,
  - t’évite au maximum les opérations manuelles techniques répétitives,
  - te demande uniquement ce qu’il ne peut pas voir lui-même (logs, état réel de services externes, décisions de haut niveau),
  - consigne les résultats de chaque opération en mettant à jour les bons fichiers.

Ce fichier `APS_cheatsheet.md` doit être mis à jour au fur et à mesure que des patterns de demandes récurrentes apparaissent (nouveaux types de bugs, nouveaux projets, nouveaux services, etc.).
