# Chapitre 5 — Diagrammes de Bode

## 5.1 Pourquoi les diagrammes de Bode sont indispensables

La fonction de transfert \( H(j\omega) \) contient toute l’information
sur le comportement fréquentiel d’un système.
Cependant, sous forme analytique, elle reste difficile à exploiter rapidement.

Les **diagrammes de Bode** ont été introduits pour répondre à un besoin fondamental :
> permettre à l’ingénieur de comprendre, comparer et concevoir des systèmes
> **sans recalculer** leur réponse à chaque fréquence.

Un diagramme de Bode permet, d’un seul coup d’œil, de répondre à des questions clés :

- quelles fréquences passent ?
- lesquelles sont atténuées ?
- avec quelle pente ?
- avec quel retard ?

---

## 5.2 Définition d’un diagramme de Bode

Un diagramme de Bode est composé de **deux graphiques distincts** :

1. le diagramme de **module** (gain),
2. le diagramme de **phase**.

Ces deux diagrammes représentent respectivement :

- l’amplitude de la réponse,
- le retard (ou avance) introduit par le système,

en fonction de la **fréquence**, représentée sur une échelle logarithmique.

---

## 5.3 Pourquoi une échelle logarithmique en fréquence

### 5.3.1 Raisons pratiques

Les systèmes électroniques peuvent être soumis à des fréquences
allant de quelques hertz à plusieurs gigahertz.

Une échelle linéaire serait inutilisable :
les basses fréquences seraient écrasées.

L’échelle logarithmique permet :

- de représenter plusieurs décades sur un même graphique,
- de donner la même importance relative à chaque plage de fréquence.

---

### 5.3.2 Sens physique

Chaque **décade** correspond à un changement d’échelle du phénomène observé.
En pratique, cela correspond souvent à un changement de comportement du système.

L’échelle logarithmique met donc en évidence
les **régimes de fonctionnement distincts**.

---

## 5.4 Diagramme de module

### 5.4.1 Définition du module

On rappelle que le module de la fonction de transfert est :

$$
|H(j\omega)|
$$

Il représente le rapport entre l’amplitude de sortie
et l’amplitude d’entrée pour la fréquence \( \omega \).

---

### 5.4.2 Gain en décibels

Le module est généralement exprimé en décibels :

$$
G_{\mathrm{dB}}(\omega) = 20 \log_{10} |H(j\omega)|
$$

L’utilisation des décibels permet :

- de transformer des produits en sommes,
- de rendre les variations plus lisibles,
- de comparer facilement des gains très différents.

---

## 5.5 Diagramme de phase

### 5.5.1 Définition de la phase

La phase est définie par :

$$
\varphi(\omega) = \arg(H(j\omega))
$$

Elle représente le décalage temporel
entre l’entrée et la sortie,
exprimé sous forme angulaire.

---

### 5.5.2 Interprétation physique

Un déphasage correspond à un **retard** :

- plus la phase est négative,
- plus le système met de temps à réagir.

La phase est donc une mesure directe
de l’inertie du système à une fréquence donnée.

---

## 5.6 Diagramme de Bode d’un premier ordre

### 5.6.1 Fonction de transfert de référence

On considère un système du premier ordre :

$$
H(j\omega) = \frac{1}{1 + j\omega\tau}
$$

---

### 5.6.2 Module exact

Le module vaut :

$$
|H(j\omega)| = \frac{1}{\sqrt{1 + (\omega\tau)^2}}
$$

À basse fréquence :

- \( \omega\tau \ll 1 \),
- le module est proche de 1 (0 dB).

À haute fréquence :

- \( \omega\tau \gg 1 \),
- le module décroît comme \( 1/\omega \).

---

### 5.6.3 Phase exacte

La phase vaut :

$$
\varphi(\omega) = -\arctan(\omega\tau)
$$

Elle évolue continûment :

- de 0° à basse fréquence,
- vers −90° à haute fréquence.

---

## 5.7 Asymptotes du diagramme de Bode

### 5.7.1 Intérêt des asymptotes

Les expressions exactes sont utiles,
mais les **asymptotes** permettent une lecture rapide
et une construction manuelle efficace.

---

### 5.7.2 Asymptote du module

- Pour \( \omega \ll \omega_c \) :
  - gain ≈ 0 dB
- Pour \( \omega \gg \omega_c \) :
  - pente de −20 dB/décade

Chaque pôle du système
introduit une pente de −20 dB/décade.

---

### 5.7.3 Asymptote de la phase

- Phase ≈ 0° à basse fréquence
- Phase ≈ −90° à haute fréquence
- Transition centrée autour de \( \omega_c \)

La transition de phase est progressive,
sur environ deux décades.

---

## 5.8 Fréquence de coupure sur le diagramme de Bode

La fréquence de coupure \( \omega_c \) est définie par :

$$
\omega_c = \frac{1}{\tau}
$$

Sur le diagramme de Bode :

- le gain vaut −3 dB,
- la phase vaut −45°.

Ce point marque la frontière
entre régime suivi et régime filtré.

---

## 5.9 Lecture physique d’un diagramme de Bode

Un diagramme de Bode permet de répondre immédiatement :

- le système est-il lent ou rapide ?
- quelles fréquences sont atténuées ?
- quel retard est introduit ?
- la transition est-elle douce ou brutale ?

Il constitue une **empreinte dynamique**
du système.

---

## 5.10 Lecture inverse : du Bode au modèle

Un ingénieur expérimenté peut :

- estimer l’ordre du système,
- localiser les pôles,
- estimer la constante de temps,

à partir d’un diagramme de Bode mesuré.

Cette capacité est essentielle
en diagnostic et en rétro-ingénierie.

---

## 5.11 Lien avec les limites réelles

Les diagrammes de Bode idéaux
ne tiennent pas compte :

- des tolérances,
- des parasites,
- des limitations de mesure.

Ces aspects seront approfondis :

- dans le chapitre « du modèle au composant réel »,
- et dans l’annexe EMI / CEM.

---

## 5.12 Transition vers la notion de coupure

Les diagrammes de Bode mettent en évidence
un point particulier :
la **fréquence de coupure**.

Ce point n’est pas arbitraire :
il correspond à une perte énergétique précise.

Le chapitre suivant sera consacré
à la démonstration rigoureuse
de la coupure à −3 dB
et à son interprétation physique profonde.
