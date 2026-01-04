# Chapitre 8 — Du modèle idéal au composant réel

## 8.1 Pourquoi les modèles idéaux finissent toujours par échouer

Les chapitres précédents ont volontairement utilisé :

- des résistances idéales,
- des condensateurs idéaux,
- des inductances idéales.

Ces modèles sont indispensables pour comprendre,
calculer et concevoir.

Mais aucun composant réel n’est idéal.
Plus on augmente :

- la fréquence,
- la précision requise,
- ou les contraintes environnementales,

plus les écarts entre modèle et réalité deviennent visibles.

Comprendre ces écarts est une compétence d’ingénieur,
pas un détail.

---

## 8.2 Résistance réelle

### 8.2.1 Tolérance

Une résistance réelle n’a jamais exactement sa valeur nominale.

Tolérances courantes :

- ±5 % (usage général),
- ±1 % (précision),
- ±0.1 % (métrologie).

Cette tolérance impacte directement :

- la constante de temps,
- la fréquence de coupure.

---

### 8.2.2 Coefficient de température

La valeur d’une résistance varie avec la température :

- coefficient typique : quelques dizaines de ppm/°C.

Dans un filtre :

- une variation de température entraîne

  une dérive lente de la coupure.

---

### 8.2.3 Bruit thermique

Toute résistance génère du bruit thermique (bruit de Johnson) :

$$
v_n^2 = 4kTRB
$$

Ce bruit est :

- inévitable,
- proportionnel à la bande passante,
- indépendant du signal.

Un filtre agit aussi sur ce bruit.

---

## 8.3 Condensateur réel

### 8.3.1 Capacité réelle et tolérance

Les condensateurs présentent souvent
des tolérances plus larges que les résistances :

- ±10 %,
- ±20 %,
- parfois plus selon la technologie.

Le choix du type de condensateur
est donc fondamental en design.

---

### 8.3.2 Résistance série équivalente (ESR)

Un condensateur réel peut être modélisé par :

- un condensateur idéal,
- une résistance série (ESR),
- parfois une inductance série (ESL).

L’ESR :

- dissipe de l’énergie,
- limite l’efficacité du filtrage,
- modifie la pente réelle du Bode à haute fréquence.

---

### 8.3.3 Inductance série équivalente (ESL)

À haute fréquence,
les connexions et les armatures
introduisent une inductance parasite.

Conséquence :

- le condensateur peut se comporter

  comme une inductance au-delà d’une certaine fréquence.

C’est une surprise classique pour les débutants.

---

### 8.3.4 Technologie des condensateurs

| Type | Avantages | Inconvénients |
| --- | --- | --- |
| Céramique | Faible ESR, HF | Capacité variable |
| Film | Stable, précis | Encombrement |
| Électrolytique | Forte capacité | ESR, vieillissement |

Le choix du composant est un acte de design,
pas un détail d’approvisionnement.

---

## 8.4 Inductance réelle

### 8.4.1 Résistance série

Une inductance réelle possède toujours :

- une résistance de fil,
- donc des pertes ohmiques.

Cela réduit :

- le facteur de qualité,
- la sélectivité.

---

### 8.4.2 Saturation magnétique

Au-delà d’un certain courant,
le noyau magnétique sature.

Conséquences :

- l’inductance effective chute,
- le comportement devient non linéaire,
- le filtre ne se comporte plus comme prévu.

---

### 8.4.3 Capacité parasite

Les spires d’une inductance
introduisent une capacité parasite.

À haute fréquence,
cela crée une fréquence de résonance propre,
au-delà de laquelle l’inductance n’est plus inductive.

---

## 8.5 Impact global sur un filtre du premier ordre

### 8.5.1 Modification de la coupure

Les parasites peuvent :

- déplacer la fréquence de coupure,
- modifier la pente,
- introduire des résonances inattendues.

---

### 8.5.2 Dégradation de la phase

Les éléments parasites
ajoutent des retards supplémentaires,
souvent non prévus dans le modèle simple.

---

## 8.6 Ordres cachés et complexification réelle

Un filtre théoriquement du premier ordre
peut devenir, en pratique :

- un système d’ordre supérieur,
- avec plusieurs constantes de temps,
- des comportements inattendus.

Ces “ordres cachés”
proviennent des éléments parasites.

---

## 8.7 Importance du montage et du PCB

### 8.7.1 Inductance des pistes

Une piste de PCB a une inductance
de l’ordre du nH par millimètre.

À haute fréquence :

- cette inductance devient significative,
- le schéma ne suffit plus.

---

### 8.7.2 Boucles de courant

Les boucles larges :

- rayonnent,
- captent des perturbations,
- dégradent le filtrage.

Le routage est donc une partie intégrante du filtre.

---

## 8.8 Lecture ingénieur d’un écart théorie / mesure

Face à un écart entre théorie et mesure,
un ingénieur se demande :

- est-ce un problème de modèle ?
- un problème de composant ?
- un problème de montage ?
- un problème de mesure ?

Ce raisonnement est systématique.

---

## 8.9 Transition vers la mesure expérimentale

Après avoir compris :

- les modèles idéaux,
- les composants réels,
- les parasites,

il devient naturel de se demander :

> comment mesurer correctement
> le comportement réel d’un filtre ?

Le chapitre suivant est consacré
à la **mesure et à la validation expérimentale**,
avec oscilloscope et analyse fréquentielle.
