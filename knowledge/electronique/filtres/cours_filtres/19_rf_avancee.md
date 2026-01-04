# Chapitre 19 — RF avancé : filtrage, bande passante et sélectivité réelle

## 19.1 Pourquoi la RF est une rupture d’échelle (pas de théorie)

En RF, ce ne sont pas de nouvelles lois qui apparaissent,
mais **les mêmes lois** qui deviennent dominantes.

Ce qui était négligeable en BF devient central :

- délais de propagation,
- impédances distribuées,
- rayonnement,
- couplages non intentionnels.

👉 La RF est la zone où **le modèle lumped (R, L, C concentrés) cesse d’être valable**.

---

## 19.2 Bande passante et contenu spectral réel

### 19.2.1 Un signal RF n’est jamais “une fréquence”

Un signal RF contient toujours :

- une fréquence centrale,
- une bande passante,
- des lobes spectraux.

Même une porteuse “pure” est élargie par :

- la modulation,
- le bruit de phase,
- l’instabilité d’horloge.

👉 En RF, **on filtre des bandes, pas des fréquences**.

---

## 19.3 Lien fondamental avec le second ordre

### 19.3.1 Tout filtre RF est (au minimum) du second ordre

- RC : inutilisable en RF (pentes trop faibles),
- RLC : minimum viable,
- filtres RF réels : ordre élevé.

👉 La RF commence là où le **Q devient critique**.

---

### 19.3.2 Facteur de qualité et sélectivité

En RF :

- un Q trop faible → interférences,
- un Q trop élevé → instabilité, dérive, accord difficile.

Le compromis RF est **structurel**, pas esthétique.

---

## 19.4 Résonateurs RF

### 19.4.1 Types de résonateurs

- LC accordé,
- quartz,
- céramique,
- cavité,
- lignes quart d’onde.

Tous sont des **second (ou haut) ordres physiques**.

---

### 19.4.2 Vision énergétique

Un résonateur RF :

- stocke l’énergie,
- la restitue lentement,
- agit comme un “tampon fréquentiel”.

Plus le stockage est efficace,
plus le Q est élevé,
plus la bande est étroite.

---

## 19.5 Filtres RF réels

### 19.5.1 Chaînes de filtrage

Une chaîne RF typique :

1. filtre d’entrée (EMI / protection),
2. filtre de présélection,
3. filtre de canal,
4. filtrage IF ou baseband.

👉 Le filtrage est **réparti**, pas concentré.

---

### 19.5.2 Ordres élevés et stabilité

Les filtres RF sont :

- très sélectifs,
- très sensibles,
- fortement dépendants du layout.

Un filtre RF mal amorti **oscille ou décroche**.

---

## 19.6 Impédance et adaptation

### 19.6.1 Pourquoi 50 Ω existe

50 Ω est :

- un compromis pertes / puissance / stabilité,
- une norme de chaîne,
- un facilitateur de filtrage.

L’adaptation est une **condition de stabilité fréquentielle**.

---

### 19.6.2 Filtrage = adaptation fréquentielle

Un filtre RF agit aussi comme :

- un adaptateur d’impédance dépendant de la fréquence.

👉 Mauvaise adaptation = mauvais filtrage.

---

## 19.7 Passage aux lignes de transmission

Quand la longueur devient comparable à \( \lambda/10 \) :

- les tensions ne sont plus uniformes,
- les phases deviennent spatiales,
- les filtres deviennent distribués.

👉 Le RLC devient une **ligne**.

---

## 19.8 Lecture ingénieur RF

Un ingénieur RF se demande toujours :

1. quelle bande utile ?
2. quel rejet nécessaire ?
3. quel Q acceptable ?
4. quelle stabilité thermique ?
5. quelle sensibilité au PCB ?

👉 En RF, le filtrage est un **système**, pas un composant.

---

## 19.9 Transition vers le numérique

La RF moderne ne s’arrête pas au matériel :

- conversion RF → numérique,
- filtrage hybride,
- DSP en aval.

👉 Prochain chapitre :
**Filtrage numérique et échantillonnage**.
