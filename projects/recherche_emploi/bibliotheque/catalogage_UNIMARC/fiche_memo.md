# Fiche mémo A4 – catalogage UNIMARC

Document synthétique pour réviser et cataloguer efficacement.

---

## 🎯 Objectif

Comprendre et utiliser rapidement les zones essentielles du format **UNIMARC** pour créer, corriger et contrôler des notices dans un SIGB.

---

## 1. Structure générale d’une notice UNIMARC

Une notice comprend :

* **Zones de contrôle (00X)** : identifiants, ISBN.
* **Données codées (1XX)** : langue, pays, type de document.
* **Description bibliographique (2XX)** : titre, édition, description matérielle.
* **Collection et notes (225, 3XX, 5XX)**.
* **Sujets (6XX)** : matières / indexation.
* **Auteurs (7XX)** : responsabilités intellectuelles.
* **Liens (4XX)** : lien vers d’autres notices.

---

## 2. Zones essentielles à connaître

## 🔵 Zones de contrôle

**001** Numéro de notice (auto).
**010** ISBN → `$a` numéro.

## 🔵 Données codées

**101** Langue → `$a` (ex. fre, eng).
**102** Pays → `$a` (ex. FR, GB).

## 🔵 Description bibliographique

### **200 – Titre et mentions de responsabilité**

* `$a` Titre propre
* `$e` Complément du titre
* `$f` Auteur principal

### **210 – Adresse** (édition)

* `$a` Lieu
* `$c` Éditeur
* `$d` Date

### **215 – Description matérielle**

* `$a` Pagination
* `$c` Dimensions

### **225 – Collection**

* `$a` Titre de la collection
* `$v` Numéro dans la collection

---

## 3. Zones de notes (3XX / 5XX)

**300** Note générale.
**327** Note de contenu.
**330** Résumé → important pour l’OPAC.

---

## 4. Zones d’indexation (6XX)

**606** Sujet (nom commun) → `$a` sujet, `$x` subdivision.
**686** Classification (ex. Dewey) → `$a` cote.

---

## 5. Zones d’auteur (7XX)

**700** Auteur principal.
**701 / 702** Co-auteurs, préfaciers, illustrateurs.

---

## 6. Méthode rapide pour cataloguer

### Étape 1 — Identifier les éléments bibliographiques

Titre, auteur, éditeur, date, pages, ISBN.

### Étape 2 — Vérifier l’existence d’une notice

SUDOC, BnF, réseau local.

### Étape 3 — Compléter ou modifier les zones

* 200 → titre / auteur
* 210 → éditeur / date
* 215 → pages / dimensions
* 330 → résumé
* 606 / 686 → matières / cote

### Étape 4 — Contrôler la cohérence

* ISBN correct ?
* Date cohérente avec l’éditeur ?
* Auteur dans les autorités ?

---

## 7. Exemple ultra-simple de notice

```
010 ##$a9782070463886
101 0#$afre
102 ##$aFR
200 1#$aLes misérables$fVictor Hugo
210 ##$aParis$cGallimard$d2010
215 ##$a350 p.$c22 cm
330 ##$aRoman historique du XIXe siècle.
606 ##$aLittérature française$y19e siècle
686 ##$a843.7$2dewey
700 #0$aHugo$fVictor
```

---

## 8. Erreurs fréquentes à éviter

* Confondre `$a` titre propre et `$e` complément du titre.
* Oublier la zone 330 (résumé visible par les usagers).
* Ne pas harmoniser les zones d’auteur (700).
* Mélanger les rôles (préfacier/illustrateur en 702, pas en 700).

---

## 9. Bonnes pratiques professionnelles

* Importer les notices quand c’est possible.
* Toujours vérifier ISBN, auteur, éditeur.
* Utiliser formes d’autorité officielles (BnF/SUDOC).
* Rédiger un résumé clair et utile pour l’OPAC.

---

Fiche conçue pour : apprentissage rapide, révision, utilisation en situation réelle.
