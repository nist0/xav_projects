# Chapitre 14 — Amplificateurs opérationnels : des systèmes du second ordre déguisés

## 14.1 Pourquoi parler d’AOP après les filtres RLC

Les amplificateurs opérationnels (AOP) sont souvent présentés
comme des composants “idéaux” :

- gain infini,
- bande passante infinie,
- réponse instantanée.

Ces hypothèses sont **pédagogiquement utiles**,
mais **physiquement fausses**.

En réalité :
> un AOP réel est un **système dynamique**,  
> très souvent assimilable à un **second ordre**.

Comprendre les AOP sans comprendre le second ordre
revient à apprendre à conduire sans comprendre l’inertie.

---

## 14.2 Rappel : modèle idéal vs modèle réel

### 14.2.1 Modèle idéal (rappel rapide)

Dans le modèle idéal :

- gain \( A \to \infty \),
- bande passante infinie,
- phase nulle,
- aucune limitation dynamique.

Ce modèle ne permet **aucune analyse de stabilité**.

---

### 14.2.2 Pourquoi ce modèle est insuffisant

Dans un AOP réel :

- le gain chute avec la fréquence,
- la phase varie,
- des pôles internes existent,
- des oscillations peuvent apparaître.

👉 Le comportement fréquentiel devient **central**.

---

## 14.3 Modèle fréquentiel d’un AOP réel

### 14.3.1 Modèle à un pôle dominant

La majorité des AOP sont conçus pour être modélisés par :

$$
A(j\omega) = \frac{A_0}{1 + j\frac{\omega}{\omega_p}}
$$

où :

- \( A_0 \) est le gain statique très élevé,
- \( \omega_p \) est le pôle dominant.

Ce modèle est **un premier ordre** interne.

---

### 14.3.2 Pourquoi ce n’est pas suffisant

Dès que l’on ferme la boucle :

- on ajoute un second pôle (réseau externe),
- la dynamique globale devient **du second ordre**.

C’est ici que tout ce que tu as vu sur RLC s’applique.

---

## 14.4 AOP en boucle fermée = système du second ordre

### 14.4.1 Schéma conceptuel

Un AOP en boucle fermée contient :

- une dynamique interne (pôle dominant),
- une dynamique externe (RC, RLC, réseau de retour).

👉 Deux stockages d’énergie implicites  
→ **second ordre**.

---

### 14.4.2 Fonction de transfert typique

La fonction de transfert fermée prend la forme :

$$
H(j\omega)
= \frac{A(j\omega)}{1 + A(j\omega)\beta(j\omega)}
$$

où \( \beta(j\omega) \) est le réseau de contre-réaction.

Le dénominateur est souvent **quadratique en \( j\omega \)**.

---

## 14.5 Lecture en termes de pôles

### 14.5.1 Origine des pôles

- pôle interne de l’AOP (compensation),
- pôle du réseau externe (filtre, charge, PCB).

Ces pôles peuvent :

- s’additionner,
- se rapprocher,
- dégrader la phase.

---

### 14.5.2 Lien avec la stabilité

Chaque pôle ajoute :

- −20 dB/décade,
- −90° de phase.

Deux pôles proches :

- −40 dB/décade,
- jusqu’à −180° de phase.

👉 Condition classique d’oscillation.

---

## 14.6 Marge de phase : lecture physique

### 14.6.1 Définition

La **marge de phase** est la distance angulaire
entre la phase réelle et −180°
au point où le gain vaut 0 dB.

---

### 14.6.2 Interprétation physique

- grande marge de phase :
  - système amorti,
  - réponse propre,
  - peu ou pas de dépassement.

- faible marge de phase :
  - oscillations,
  - résonance,
  - instabilité potentielle.

👉 Exactement les mêmes effets
que pour un RLC sous-amorti.

---

## 14.7 Analogie directe avec le second ordre

| AOP | Second ordre |
| --- | --- |
| Pôles internes | \( L \) et \( C \) |
| Compensation | Amortissement |
| Marge de phase | Facteur \( \zeta \) |
| Oscillation | Résonance |

Un AOP **n’est pas magique** :
il obéit aux mêmes lois dynamiques.

---

## 14.8 Exemple concret : AOP non-inverseur réel

### 14.8.1 Gain théorique

Pour un non-inverseur :

$$
G = 1 + \frac{R_2}{R_1}
$$

Valable uniquement à basse fréquence.

---

### 14.8.2 Gain réel

À haute fréquence :

- le gain chute,
- la phase dérive,
- le montage peut osciller si mal compensé.

Le réseau RC externe
joue le rôle d’un **second stockage d’énergie**.

---

## 14.9 Pourquoi les fabricants compensent les AOP

Les AOP “stables en gain unité” :

- possèdent un pôle dominant volontairement bas,
- forcent un comportement proche du premier ordre.

C’est un **choix d’ingénierie** :
on sacrifie la bande passante
pour garantir la stabilité.

---

## 14.10 Lecture ingénieur d’un montage à AOP

Avant de faire confiance à un montage :

1. combien de pôles au total ?
2. où sont-ils situés ?
3. quelle est la marge de phase ?
4. y a-t-il un risque de résonance ?

Ces questions sont identiques
à celles posées pour un RLC.

---

## 14.11 Pourquoi ce chapitre est fondamental

Ce chapitre explique :

- pourquoi un montage AOP peut osciller,
- pourquoi un petit condensateur “magique” stabilise tout,
- pourquoi la bande passante n’est jamais gratuite.

Il relie définitivement :

- filtres passifs,
- filtres actifs,
- stabilité,
- contrôle.

---

## 14.12 Transition naturelle

Maintenant que l’on comprend :

- les AOP comme systèmes dynamiques,
- leur lien avec le second ordre,

la suite naturelle est :
👉 **applications numériques complètes avec AOP**
ou
👉 **stabilité, compensation et marges en pratique**.
