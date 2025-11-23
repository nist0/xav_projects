# Corrections détaillées – Exercices SIGB (1 à 30)

↩️ Retour au sommaire SIGB : [`README.md`](./README.md)

Chaque correction décrit la **procédure professionnelle complète**, les modules SIGB concernés et les bonnes pratiques.

---

## 🔵 Exercices 1 à 10 — Prêts / Retours / Réservations

## **1. Retour en retard (12 jours)**

**Module : Circulation → Retours**

1. Scanner le document.
2. Le SIGB signale le retard (+12 jours).
3. Vérifier :

   * catégorie d’usager,
   * règles d’amende (selon politique locale).
4. Si amende : appliquer automatiquement ou saisir manuellement.
5. Informer l’usager avec diplomatie.
6. Vérifier si le document est réservé → mise de côté si oui.

## **2. Renouvellement impossible (réservé)**

1. Scanner la carte usager.
2. Ouvrir la liste des prêts.
3. Le SIGB indique une réservation → renouvellement bloqué.
4. Expliquer à l’usager qu’il doit rendre le livre.
5. Noter la date limite de retour.

## **3. Livre perdu**

1. Aller dans *Circulation → Usager → Prêts en cours*.
2. Marquer l’exemplaire en **« perdu »**.
3. Le SIGB applique :

   * facturation (prix du livre),
   * blocage éventuel.
4. Informer l’usager du montant.
5. Côté exemplaire : statut = *perdu*.

## **4. Dépassement du quota**

1. Le SIGB bloque automatiquement.
2. Vérifier catégorie de l’usager.
3. Proposer de rendre ou terminer une réservation.
4. Ne jamais forcer le prêt sauf autorisation hiérarchie.

## **5. Retour multiple (10 docs)**

**Module : Retours**

1. Scanner les documents en série.
2. Contrôler les alertes : réservations, retards, erreurs.
3. Ranger par localisation.

## **6. Document rendu réservé**

1. Message du SIGB : *réservé*.
2. Changer statut → **« Mis de côté »**.
3. Glisser dans bac réservations (ordre alphabétique).
4. Notifier l’usager (courriel automatique ou manuel).

## **7. Historique de prêt**

**Usager → Historique**
Affiche : dates, documents, retards.
*⚠️ nécessite activation de l’historique selon RGPD.*

## **8. Réservation expirée**

1. Vérifier date de mise de côté.
2. Passé le délai (souvent 7 jours) → annuler réservation.
3. Réattribuer :

   * soit à la réservation suivante,
   * soit remettre en rayon.

## **9. Carte usager expirée**

1. Message SIGB : *abonnement expiré*.
2. Ouvrir la fiche usager → prolonger.
3. Encaisser le renouvellement si nécessaire.

## **10. Document « en transit »**

1. Module *Circulation → Transit*.
2. Vérifier point A → point B.
3. Si non arrivé après X jours : rechercher physiquement.
4. Noter incident dans SIGB.

---

## 🔵 Exercices 11 à 20 — Notices / Exemplaires

## **11. Import BnF + ajout exemplaire**

1. Module *Catalogue → Import Z39.50*.
2. Chercher par ISBN.
3. Importer notice.
4. Créer exemplaire : code-barres, cote Dewey, statut, localisation.

## **12. Modifier une notice**

1. Ouvrir la notice → Éditer.
2. Zone 210$c (éditeur) → corriger.
3. Zone 200$a (titre) → corriger faute.
4. Sauvegarder → recalcul index.

## **13. Ajouter cote/localisation**

1. Menu *Exemplaires → Modifier*.
2. Remplir :

   * Cote Dewey,
   * Localisation (ex. Adultes),
   * Section.

## **14. Supprimer une notice**

Autorisé seulement si :

* aucun exemplaire lié,
* aucune réservation.
  Procédure : *Options → Supprimer notice*.

## **15. Correction auteur (autorité existante)**

1. Ouvrir la notice.
2. Zone 700 → remplacer par forme autorisée.
3. Lier à l’autorité existante.

## **16. Ajouter résumé (330)**

1. Éditer notice.
2. Ajouter zone 330.
3. Résumé clair, utile, rédigé pour OPAC.

## **17. Fusion de doublons**

1. Repérer notices doubles.
2. *Outil → Fusionner notices*.
3. Choisir notice principale.
4. Transférer exemplaires.

## **18. Changer la section d’un exemplaire**

1. Exemplaire → Modifier.
2. Changer localisation.
3. Générer nouvelle étiquette si nécessaire.

## **19. Document « hors prêt »**

1. Éditer exemplaire.
2. Statut = *non empruntable* ou *consultation*.

## **20. Mettre un exemplaire « en réparation »**

1. Statut = *en réparation*.
2. Ajouter note interne.
3. Empêcher emprunt.

---

## 🔵 Exercices 21 à 30 — Usagers / Statistiques / Administration

## **21. Nouveau compte lecteur adulte**

1. Menu *Usagers → Nouveau*.
2. Remplir identité.
3. Choisir catégorie *Adulte*.
4. Durée abonnement → Personnalisée.
5. Générer numéro de carte.

## **22. Modifier catégorie enfant → adulte**

1. Usager → Modifier.
2. Catégorie : *Adulte*.
3. Impact :

   * durée de prêt différente,
   * quotas augmentés.

## **23. Réinitialiser mot de passe OPAC**

1. Usager → Authentification.
2. « Réinitialiser mot de passe ».
3. Courriel automatique envoyé.

## **24. Vérifier prêts usager absent 6 mois**

1. Historique.
2. Prêts en cours.
3. Statut carte.
4. Envoyer rappel (selon règles locales).

## **25. Statistique : prêts jeunesse 3 derniers mois**

1. Module *Statistiques*.
2. Filtre : localisation = Jeunesse.
3. Période = derniers 3 mois.
4. Export CSV.

## **26. Documents jamais empruntés (2 ans)**

1. Statistiques → rotation.
2. Filtrer « 0 prêt ».
3. Liste pour désherbage.

## **27. Export usagers inactifs**

1. Usagers → Requêtes / filtres.
2. Critère : absence de prêt ≥ 12 mois.
3. Export CSV.

## **28. Tester les règles de prêt**

1. Module Administration → Règles de prêt.
2. Tester catégorie X + type document Y.
3. Identifier l’anomalie et corriger.

## **29. Modifier horaires OPAC**

1. Administration OPAC.
2. Section « Informations pratiques ».
3. Mettre à jour horaires.

## **30. Sauvegarde complète**

1. Administration → Sauvegardes.
2. Lancer sauvegarde BDD.
3. Télécharger le fichier.
4. Stockage sécurisé.
5. Jamais modifier les fichiers manuellement.

---

Ces corrections couvrent les procédures professionnelles de SIGB niveau bibliothécaire confirmé.
