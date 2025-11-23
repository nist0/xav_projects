# Fiche mémo A4 – SIGB (Système intégré de gestion de bibliothèque)

↩️ Retour au sommaire SIGB : [`README.md`](./README.md)

Objectif : avoir sous la main une synthèse A4 des notions essentielles sur les SIGB pour réviser rapidement avant un entretien ou une prise de poste.

Document synthétique pour maîtriser rapidement les fonctions professionnelles d’un SIGB.

---

## 🎯 Objectif

Savoir utiliser les fonctionnalités essentielles d’un SIGB : notices, exemplaires, prêts/retours, réservations, usagers, statistiques.

Pour **visualiser concrètement les écrans typiques d’un SIGB**, tu peux aussi consulter : [`ecrans_courants.md`](./ecrans_courants.md).

---

## 1. Modules principaux

- **Catalogue / notices** : création, modification, import Z39.50, autorités.
- **Exemplaires** : code-barres, statut, localisation, cote Dewey.
- **Usagers** : création de compte, catégories, historique, renouvellement.
- **Circulation** : prêts, retours, réservations, renouvellements.
- **Statistiques** : taux de rotation, prêts par secteur, usagers actifs.
- **Administration** : paramètres, règles de prêt, sauvegardes.

---

## 2. Workflow : traitement documentaire

1. Importer ou créer la notice.
2. Vérifier les zones UNIMARC.
3. Ajouter l’exemplaire : code-barres, localisation, statut.
4. Définir la cote Dewey.
5. Imprimer l’étiquette.
6. Mettre en rayon.

---

## 3. Workflow : prêt et retour

### Prêt

1. Scanner la carte usager.
2. Scanner le document.
3. Vérifier les quotas.
4. Enregistrer le prêt.

### Retour

1. Scanner le document.
2. Gérer les retards.
3. Vérifier les réservations.
4. Mettre de côté si réservé.

---

## 4. Gestion des réservations

- Document emprunté → réservation possible.
- À son retour : statut « mis de côté ».
- Notification automatique à l’usager.
- Délai de retrait (souvent 7 jours).

---

## 5. Gestion des usagers

- Création / modification de compte.
- Catégories (adulte, enfant, étudiant…).
- Renouvellement d’adhésion.
- Historique (si RGPD activé).

---

## 6. Statuts d’exemplaires (très utilisés)

- **En rayon**
- **Emprunté**
- **Réservé**
- **Mis de côté**
- **Perdu**
- **En réparation**
- **Non empruntable**

---

## 7. Bonnes pratiques

- Harmoniser les données (cotes, statuts, localisation).
- Utiliser les autorités pour auteurs / sujets.
- Traquer les doublons de notices et les fusionner.
- Vérifier régulièrement les réservations non retirées.
- S’assurer de sauvegardes régulières (si rôle admin).

---

## 8. Raccourcis utiles

- Prêt rapide → module prêt + scan successif.
- Recherche notice → ISBN ou mots-clés.
- Usager → rechercher par nom ou numéro de carte.

---

Fiche conçue pour un usage quotidien en médiathèque, BU ou centre de documentation.
