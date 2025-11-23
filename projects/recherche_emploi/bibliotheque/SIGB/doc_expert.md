# Document expert – SIGB (Système Intégré de Gestion de Bibliothèque)

Objectif : maîtriser les fonctionnalités, logiques, workflows et bonnes pratiques professionnelles des SIGB pour travailler en médiathèque comme une personne expérimentée.

↩️ Retour au sommaire SIGB : [`README.md`](./README.md)

---

## 1. Définition et rôle d’un SIGB

Un **SIGB** (Système Intégré de Gestion de Bibliothèque) est un logiciel centralisant toutes les opérations quotidiennes d’une bibliothèque :

* gestion des **notices bibliographiques**,
* gestion des **exemplaires**,
* **prêts / retours / réservations**,
* gestion des **usagers**,
* suivi des **statistiques**,
* gestion des **autorités** (auteurs, sujets),
* gestion du **catalogue public en ligne** (OPAC).

SIGB utilisés en France : **PMB**, **Koha**, **Orphée**, **Decalog**, **AFI Nanook**, **Syrtis**…

Pour **voir à quoi ressemblent concrètement les écrans** (recherche, prêt, usager, statistiques…), tu peux t’appuyer sur le document : [`ecrans_courants.md`](./ecrans_courants.md).

---

## 1.b. Liens utiles sur quelques SIGB (Koha, PMB, Orphée)

### Koha

* Wikipédia (FR) : [Koha (logiciel)](https://fr.wikipedia.org/wiki/Koha_(logiciel))
* Site du projet : [Koha Community](https://koha-community.org/)
* Démonstrations en ligne : [Koha Demo (by ByWater Solutions)](https://demo.bywatersolutions.com/) (interface de test libre d’accès)

Vidéos (FR) pour voir l’interface Koha en action :

* YouTube – « Présentation de Koha (interface et modules) » : [voir les vidéos](https://www.youtube.com/results?search_query=koha+sigb+d%C3%A9monstration+fran%C3%A7ais)
* YouTube – playlists de tutoriels Koha (modules prêts, notices, usagers) : [voir les playlists](https://www.youtube.com/results?search_query=formation+koha+biblioth%C3%A9caires)
* YouTube – tutoriel vidéo Koha (exemple concret) : [Tutoriel Koha](https://www.youtube.com/watch?v=_LIsY8MQKAw)

### PMB

* Wikipédia (FR) : [PMB (logiciel)](https://fr.wikipedia.org/wiki/PMB_(logiciel))
* Site officiel : [PMB Services](https://www.sigb.net/)
* Démo en ligne : [Démo PMB](https://demo.sigb.net/) (selon disponibilité sur le site éditeur)

Vidéos (FR) pour voir l’interface PMB en action :

* YouTube – résultats pour « tutoriel PMB SIGB » : [voir les vidéos](https://www.youtube.com/results?search_query=tutoriel+PMB+SIGB)
* YouTube – résultats pour « formation PMB bibliothécaires » : [voir les vidéos](https://www.youtube.com/results?search_query=formation+PMB+biblioth%C3%A9caires)

### Orphée

* Wikipédia (FR) : [Orphée (SIGB)](https://fr.wikipedia.org/wiki/Orph%C3%A9e_(logiciel))
* Site éditeur (Opsys) : [Orphée – SIGB pour bibliothèques](https://www.opsys.fr/)

Vidéos (FR) pour voir l’interface Orphée en action :

* YouTube – résultats pour « Orphée SIGB démonstration » : [voir les vidéos](https://www.youtube.com/results?search_query=Orph%C3%A9e+SIGB+d%C3%A9monstration)
* Dailymotion – résultats pour « Orphée bibliothèque logiciel » : [voir les vidéos](https://www.dailymotion.com/search/Orph%C3%A9e%20biblioth%C3%A8que%20logiciel/videos)

---

## 2. Architecture générale d’un SIGB

Un SIGB est organisé en plusieurs modules :

## 2.1. Catalogue / Notices

* Import, création, modification des notices.
* Gestion des zones UNIMARC / MARC.
* Liens vers notices d’autorités.

## 2.2. Exemplaires

* Code-barres,
* Localisation (secteur adulte, jeunesse, musique…),
* Statut (empruntable, consultation, exclu du prêt…),
* Cote (Dewey),
* État (bon, à réparer…).

## 2.3. Usagers

* Création et suivi des comptes lecteurs,
* Catégories : adulte / enfant / étudiant / partenaire,
* Historique des prêts,
* Avertissements, amendes (selon politique locale).

## 2.4. Circulation (Prêts/Retours)

* Passage lecteur + code-barres exemplaire,
* Renouvellements,
* Gestion des réservations.

## 2.5. Statistiques

* Nombre de prêts par section,
* Taux de rotation des collections,
* Fréquentation,
* Profil des usagers.

## 2.6. OPAC (catalogue public)

* Recherche simple/avancée,
* Résumés,
* Disponibilité des documents,
* Réservations en ligne.

---

## 3. Workflows professionnels essentiels

## 3.1. Traitement documentaire complet

1. Réception du document.
2. Recherche/import de notice existante (SUDOC/BnF/Réseau).
3. Vérification des zones (200, 210, 215, 330…).
4. Ajout/modification des indexations (606) et classification (686).
5. Création de l’exemplaire :

   * code-barres,
   * localisation,
   * cote Dewey,
   * statut.
6. Mise en rayon.

## 3.2. Accueil public : Prêt / Retour

1. Scanner la carte usager.
2. Scanner le code-barres du document.
3. Vérifier les réservations.
4. Vérifier la date de retour.
5. Gérer éventuels retards.

## 3.3. Gestion des réservations

* Sur un document emprunté.
* Mise de côté à son retour.
* Notification à l’usager.
* Délais de retrait.

## 3.4. Désherbage

* Extraction des statistiques : documents peu empruntés.
* État physique.
* Pertinence du contenu.
* Décision : pilon / don / stockage.

---

## 4. Tâches fréquentes que tu dois savoir maîtriser

* Rechercher une notice.
* Modifier une notice (zones bibliographiques et d’autorité).
* Ajouter un exemplaire avec une cote correcte.
* Enregistrer un nouvel usager.
* Réaliser un prêt ou un retour.
* Gérer une réservation.
* Imprimer une étiquette de cote.
* Faire des statistiques simples.

---

## 4.b Plan d’auto-formation spécial PMB / Koha / Orphée

### Objectif

Être capable de dire en entretien :

* « Je sais utiliser un SIGB libre (PMB/Koha) pour cataloguer, gérer les prêts et faire des recherches. »
* « Je comprends la logique d’Orphée et je peux m’y adapter rapidement en situation de travail. »

### Étape 1 – PMB : ton terrain d’entraînement principal

1. Ouvrir une **démo PMB** (ou instance de test) à partir des liens en haut du document.
2. T’entraîner à réaliser les opérations suivantes :
   * créer une **notice simple** (titre, auteur, éditeur, date, résumé) ;
   * ajouter un **exemplaire** avec cote, localisation et statut ;
   * faire un **prêt** et un **retour** sur un lecteur de test ;
   * faire une **recherche simple** par auteur, titre, sujet ;
   * tester une **recherche avancée** (par année, type de document, etc.).
3. Noter dans un carnet ou dans ce dossier :
   * noms des **menus** utilisés ;
   * ordre des **étapes** pour cataloguer / prêter / réserver ;
   * 2–3 points que tu trouves pratiques dans PMB.

### Étape 2 – Koha : observer et reproduire les mêmes gestes

1. Aller sur une **démo Koha** (par exemple la démo ByWater ou autre démo publique).
2. Repérer les modules équivalents à PMB :
   * catalogage (notices),
   * circulation (prêts/retours),
   * usagers,
   * OPAC.
3. Refais les mêmes exercices que pour PMB :
   * créer ou modifier une notice simple ;
   * faire un prêt / retour sur un lecteur de test ;
   * lancer une recherche dans l’OPAC.
4. Note les **différences de vocabulaire** ou d’interface par rapport à PMB.

### Étape 3 – Orphée : comprendre la logique sans l’installer

Orphée est un logiciel **propriétaire** : tu ne peux pas légalement le télécharger et l’installer librement chez toi.

Pour te familiariser :

1. Lire les **présentations et plaquettes** d’Orphée sur le site éditeur.
2. Regarder des **captures d’écran / vidéos de démo** (liens au début du document).
3. Repérer les grands modules :
   * catalogue / notices,
   * exemplaires,
   * circulation,
   * usagers,
   * statistiques.
4. Faire le parallèle avec ce que tu connais déjà dans PMB et Koha.

En entretien, tu peux formuler par exemple :

> « J’ai pratiqué PMB et Koha sur des instances de test pour les opérations de base (catalogage, prêts, recherches). Pour Orphée, je me suis documentée à partir du site éditeur et de démonstrations vidéo ; comme la logique notice/exemplaire/usager est la même, je sais que je peux m’adapter rapidement. »

---

## 5. Bonnes pratiques professionnelles

## 5.1. Harmonisation

Toujours respecter :

* les normes internes (ex. présentation de cote),
* les autorités nationales (RAMEAU, BnF).

## 5.2. Cohérence des données

* Pas de fautes dans les titres.
* Dates conformes au document.
* Formats de code-barres homogènes.

## 5.3. Sécurité et RGPD

* Ne jamais divulguer les données usagers.
* Mettre à jour les droits d’accès staff.

---

## 6. Savoir parler SIGB en entretien

Formulations professionnelles :

* « Je maîtrise les workflows du SIGB, de la création de notices à la gestion des prêts. »
* « Je suis capable d’importer, corriger et harmoniser des notices dans le respect des autorités UNIMARC. »
* « J’ai de l’expérience dans l’accueil public, la gestion des réservations et le suivi des fiches lecteurs. »
* « Je sais utiliser les outils statistiques du SIGB pour analyser l’activité documentaire. »

### Autres formulations possibles en entretien

* « Je maîtrise les principaux workflows d’un SIGB : catalogage, gestion des exemplaires, prêts/retours, réservations, gestion des usagers et statistiques. »
* « En situation d’accueil, je sais gérer les cas concrets (retard, perte, réservation, carte expirée) tout en gardant un bon contact avec le public. »
* « Je peux rapidement m’adapter à un nouveau SIGB, car les logiques de base restent les mêmes : notices, exemplaires, usagers et règles de prêt. »

---

## 7. Annexes : notions avancées

## 7.1. Autorités

* Auteurs, collectivités, sujets, titres.

## 7.2. Paramétrages avancés

* Modèles d’impression,
* Types d’exemplaires,
* Règles de prêt.

## 7.3. Interopérabilité

* Z39.50,
* Import/export XML/MARC,
* Liens vers ressources numériques.

---

Ce document constitue une base complète pour comprendre, expliquer et utiliser un SIGB en environnement professionnel.

---

## Liens pour approfondir

* [Wikipédia – Système intégré de gestion de bibliothèque](https://fr.wikipedia.org/wiki/Syst%C3%A8me_int%C3%A9gr%C3%A9_de_gestion_de_biblioth%C3%A8que)
* [Wikipédia – Koha (logiciel de SIGB libre)](https://fr.wikipedia.org/wiki/Koha_(logiciel))
* [Wikipédia – PMB (logiciel de SIGB libre)](https://fr.wikipedia.org/wiki/PMB_(logiciel))
* [Documentation communautaire Koha](https://koha-community.org/documentation/)
* [Site de PMB (documentation et démos)](https://www.sigb.net/)
