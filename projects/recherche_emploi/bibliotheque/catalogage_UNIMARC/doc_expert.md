# Catalogage UNIMARC – Dossier complet pour maîtriser le sujet

Objectif : comprendre le **catalogage UNIMARC** en profondeur, de façon opérationnelle, pour pouvoir :

* travailler de manière autonome sur un SIGB ;
* expliquer clairement ta pratique en entretien ;
* être perçue comme quelqu’un qui maîtrise le sujet, pas juste qui en a « entendu parler ».

---

## 1. Rappel : à quoi sert le catalogage UNIMARC ?

Le catalogage, c’est l’art de **décrire un document** (livre, DVD, revue, ressource numérique…) pour qu’il soit :

* trouvé facilement par les usagers ;
* géré correctement par le système (prêt, réservation, statistiques…) ;
* échangé entre bibliothèques (partage de notices, réseau, SUDOC, etc.).

**UNIMARC** est un **format de catalogage** (comme MARC21) : il définit une structure normalisée pour encoder les informations bibliographiques.

Concrètement :

* chaque **notice bibliographique** = une fiche très structurée ;
* chaque élément (titre, auteur, sujet, ISBN…) = un **champ** codé avec :

  * un **tag** (numéro à 3 chiffres, ex. : 200 pour le titre),
  * des **indicateurs** (précisent le type d’information),
  * des **sous-champs** (lettres précédées d’un $ : $a, $e, $f…).

---

## 2. Anatomie d’une notice UNIMARC

Une notice UNIMARC se compose en général de :

1. **Zones de contrôle** (00X)
   Ex. : 001 (numéro de notice), 010 (ISBN).

2. **Données codées** (100, 101, 102, 105, 110…)
   Ce sont des champs codés qui résument des infos sous forme de codes.

3. **Description bibliographique** (200, 210, 215…)
   C’est la description visible : titre, auteur, éditeur, date, description physique…

4. **Zones de collection et notes** (225, 3XX, 5XX)
   Collection, notes générales, notes de contenu, etc.

5. **Accès matières** (6XX)
   Sujets / descripteurs, indexation matière, classification.

6. **Accès auteurs et titres liés** (7XX)
   Auteurs, collaborateurs, autres formes de titre…

7. **Liens avec d’autres notices** (4XX)
   Périodiques, parties d’œuvres, liens entre différentes éditions, etc.

En SIGB, tu n’as pas toujours besoin de tout connaître par cœur, mais tu dois :

* repérer vite les **zones principales** ;
* savoir **où modifier une info** (ex. : erreur d’éditeur → zone 210) ;
* comprendre la logique : **qui fait quoi**.

### 2.1 Schéma visuel de la structure UNIMARC

```mermaid
graph TD
  A[Notice UNIMARC] --> B[Zones de contrôle (00X)]
  A --> C[Données codées (1XX)]
  A --> D[Description bibliographique (2XX)]
  A --> E[Collections & notes (225, 3XX, 5XX)]
  A --> F[Accès matières (6XX)]
  A --> G[Accès auteurs & titres liés (7XX)]
  A --> H[Liens entre notices (4XX)]

  C --> C1[100 – Données générales de traitement]
  C --> C2[101 – Langue]
  C --> C3[102 – Pays]

  D --> D1[200 – Titre & responsabilité]
  D --> D2[210 – Adresse (édition)]
  D --> D3[215 – Description matérielle]
  D --> D4[225 – Collection]

  E --> E1[300 – Note générale]
  E --> E2[327 – Note de contenu]
  E --> E3[330 – Résumé]

  F --> F1[606 – Accès matière]
  F --> F2[686 – Classification (cote)]

  G --> G1[700 – Auteur principal]
  G --> G2[701/702 – Autres responsabilités]
```

---

## 3. Les champs UNIMARC essentiels à maîtriser

### 3.1. Zones de contrôle

* **001 – Numéro de notice**
  Identifiant unique de la notice dans le SIGB. On ne le modifie presque jamais manuellement.

* **010 – ISBN**
  $a : numéro ISBN.
  Exemple : `010 ##$a9782070463886`

### 3.2. Informations codées

* **100 – Données générales de traitement**
  Contient des codes sur le type de document, la date, etc. Souvent généré automatiquement.

* **101 – Langue du document**
  $a : langue principale (ex. fre, eng).
  Exemple : `101 0#$afre`

* **102 – Pays de publication**
  $a : code du pays (ex. FR, GB…).

Ces zones sont cruciales pour les **recherches avancées** et les **statistiques**, mais souvent préremplies par import de notice.

### 3.3. Description bibliographique

* **200 – Titre et mention de responsabilité**
  C’est une des zones les plus importantes.

  Sous-champs courants :

  * $a : titre propre
  * $e : complément du titre (facultatif)
  * $f : mention de responsabilité principale (souvent l’auteur)
  * $g : autres mentions de responsabilité (co-auteurs, illustrateurs…)

  Exemple :
  `200 1#$aLes misérables$fVictor Hugo`

* **210 – Adresse (édition)**
  Informations sur l’éditeur et la date.

  Sous-champs courants :

  * $a : lieu de publication
  * $c : nom de l’éditeur
  * $d : date de publication

  Exemple :
  `210 ##$aParis$cGallimard$d2010`

* **215 – Description matérielle**
  Décrit l’aspect physique du document.

  Sous-champs courants :

  * $a : nombre de pages ou volumes
  * $c : dimensions

  Exemple :
  `215 ##$a350 p.$c22 cm`

* **225 – Collection**
  Utilisée si le document appartient à une collection éditoriale.

  Sous-champs :

  * $a : titre de la collection
  * $v : numéro dans la collection

  Exemple :
  `225 0#$aFolio$v1234`

### 3.4. Zones de notes (3XX, 5XX)

* **300 – Note générale**
  Remarques sur le document (traduction, édition revue, etc.).

  Exemple :
  `300 ##$aTraduit de l’anglais.`

* **327 – Note de contenu**
  Table des matières, description du contenu.

  Exemple :
  `327 ##$aContient : Introduction ; Chapitre 1…`

* **330 – Résumé** (très important pour les usagers sur l’OPAC)
  Résumé ou présentation du contenu.

  Exemple :
  `330 ##$aRoman historique se déroulant au XIXe siècle…`

### 3.5. Zones de sujets / indexation (6XX)

* **606 – Accès matière – Nom commun**
  Sert à décrire le sujet avec des mots-clés (thésaurus interne, RAMEAU…).

  Sous-champs courants :

  * $a : sujet principal
  * $x : subdivision de sujet
  * $y : subdivision chronologique
  * $z : subdivision géographique

  Exemple :
  `606 ##$aRévolution française$x1789-1799`

* **686 – Classification**
  Numéro de cote (par exemple Dewey) utilisé pour le rangement.

  Exemple :
  `686 ##$a944.04$2dewey`

### 3.6. Zones d’auteur et d’accès secondaire (7XX)

* **700 – Auteur principal (personne physique)**
  Sous-champs :

  * $a : forme rejetée
  * $f : prénom
  * $g : dates
    (le détail dépend du SIGB et des autorités gérées)

  Exemple (simplifiée) :
  `700 #0$aHugo$fVictor$g1802-1885`

* **701, 702** – Co-auteurs, autres responsabilités (préfaciers, illustrateurs…).

Ces zones sont liées aux **notices d’autorité** (fichiers d’auteurs standardisés).

---

## 4. Logique d’un travail de catalogage UNIMARC

Quand tu catalogues un document, tu suis typiquement ces étapes :

1. **Identifier le document**

   * Vérifier ISBN, auteur, titre, éditeur, année.

2. **Chercher une notice existante** (catalogue partagé, SUDOC, BnF, Electre…)

   * Si elle existe → l’importer puis l’adapter si nécessaire (collection locale, cote…).
   * Si elle n’existe pas → créer une **nouvelle notice**.

3. **Créer ou compléter la notice** dans le SIGB :

   * Renseigner les zones 010, 101, 200, 210, 215, 225, 330, 606, 686, etc.
   * Vérifier la cohérence globale :

     * la date dans 210$d correspond à ce qui est sur le document ;
     * le résumé correspond bien au contenu ;
     * les sujets décrivent correctement le thème.

4. **Attribuer la cote et rattacher l’exemplaire** :

   * cote Dewey (zone 686) ;
   * exemplaire avec code-barres, localisation, statut (empruntable, consultation sur place…).

5. **Contrôle final** :

   * affichage public (OPAC) : vérifier que la notice est claire pour le public ;
   * correction d’éventuelles fautes.

---

## 5. Bonnes pratiques pour paraître experte

Pour être perçue comme « experte » ou très à l’aise en catalogage UNIMARC, il ne suffit pas de connaître les codes : il faut montrer que tu maîtrises :

### 5.1. La cohérence et la qualité des données

* Vérifier toujours la **concordance** entre : ISBN, titre, auteur, éditeur, date, nombre de pages.
* Respecter les **normes de transcription** (majuscule/minuscule, accents, ponctuation).
* Être attentive à l’orthographe dans les zones visibles par le public (titre, résumé, notes).

### 5.2. Le respect des autorités

* Utiliser les formes d’auteurs et de sujets **normalisées** (RAMEAU, autorités BnF/SUDOC).
* Éviter de créer des variantes "sauvages" (ex. Hugo, V. au lieu de Hugo, Victor).

### 5.3. La maîtrise des zones clés

En entretien, tu peux :

* citer précisément les zones que tu sais renseigner (200, 210, 215, 225, 300, 327, 330, 606, 686, 700…) ;
* expliquer que tu sais **importer, contrôler, corriger** une notice ;
* mentionner que tu sais **adapter le niveau de description** en fonction du type de document (roman, documentaire, DVD, ressource numérique…).

Exemple de phrase :

> « En catalogage UNIMARC, je sais créer et corriger des notices complètes : zones descriptives (200, 210, 215), zones de notes (300, 327, 330), indexation matière (606) et cote Dewey (686), ainsi que les accès auteurs (700 et suivants). Je suis particulièrement attentive à la cohérence des données et à l’utilisation des autorités normalisées. »

### Autres formulations possibles en entretien

* « Quand je catalogue, je commence par voir s’il existe déjà une notice de qualité (BnF, SUDOC), que j’importe puis j’adapte au contexte local plutôt que de tout recréer. »
* « Je sais expliquer clairement la différence entre notice bibliographique, notice d’autorité et exemplaire, ce qui rassure les équipes sur ma maîtrise du catalogage. »
* « Je suis attentive à l’homogénéité des notices (auteurs, sujets, résumés) pour que le catalogue reste cohérent et lisible pour les usagers. »

---

## 6. Application concrète : exemple de notice UNIMARC simplifiée

Imaginons un roman adulte simple.

**Document :**
Titre : *Les saisons du cœur*
Auteur : Jeanne Martin
Éditeur : Éditions Soleil
Lieu : Paris
Année : 2023
Nombre de pages : 280
Sujet : Roman contemporain, relations familiales
Cote : R MAR

**Notice UNIMARC simplifiée (vue logique)**

* 010 ##$a9781234567890
* 101 0#$afre
* 102 ##$aFR
* 200 1#$aLes saisons du cœur$fJeanne Martin
* 210 ##$aParis$cÉditions Soleil$d2023
* 215 ##$a280 p.$c22 cm
* 300 ##$aRoman contemporain.
* 330 ##$aRoman familial qui suit plusieurs générations au fil des saisons, entre secrets, réconciliations et choix de vie.
* 606 ##$aFamille$xRelations familiales$vFiction
* 686 ##$a843$2dewey
* 700 #0$aMartin$fJeanne

Tu peux t’entraîner à **reconstruire ce type de notice** pour des livres que tu as chez toi.

---

## 7. Comment en parler en entretien pour paraître très à l’aise

Quelques formulations que tu peux utiliser :

* « J’ai une bonne maîtrise du format UNIMARC : je sais créer, importer et corriger des notices dans un SIGB, en utilisant les zones descriptives, les accès matière et les zones d’autorité. »
* « Je suis attentive à la qualité du catalogage, car je sais que cela impacte directement la recherche documentaire des usagers. »
* « Je suis capable de vérifier la cohérence des données bibliographiques (ISBN, date, éditeur), de rédiger des résumés clairs et d’appliquer une indexation matière et une cote Dewey adaptées. »

---

## 8. Plan d’auto-formation pour consolider ton expertise

1. **Observer des notices existantes** dans un catalogue (BnF, SUDOC, catalogue de réseau) et repérer les zones.
2. **Comparer plusieurs notices** pour le même document (éditeur / éditions différentes).
3. **T’entraîner à analyser une notice** et à dire : à quoi sert chaque zone ?
4. **Faire des exercices mentaux** :

   * Je prends un livre chez moi ;
   * je rédige sur papier ce que je mettrais en 200, 210, 215, 330, 606, 686 ;
   * je compare avec une notice trouvée dans un catalogue public.

En suivant ce document, tu as une base solide pour :

* comprendre le catalogage UNIMARC ;
* parler comme quelqu’un qui le pratique vraiment ;
* être rapidement opérationnelle en poste dans une médiathèque ou bibliothèque.

---

## Liens pour approfondir

* [Wikipédia – UNIMARC](https://fr.wikipedia.org/wiki/UNIMARC)
* [IFLA – UNIMARC (en anglais)](https://www.ifla.org/unimarc/)
* [Abes – documentation sur les formats et autorités (SUDOC)](https://www.abes.fr/)
* [BnF – Catalogage et formats MARC](https://www.bnf.fr/fr/catalogage-et-formats)
* Rechercher « guide catalogage UNIMARC PDF » pour trouver un manuel de pratique proche de ton environnement de travail.
