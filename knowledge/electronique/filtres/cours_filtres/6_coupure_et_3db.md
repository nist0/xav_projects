# Chapitre 6 — Fréquence de coupure et −3 dB

## 6.1 Pourquoi la notion de coupure est fondamentale

Dans le langage courant, on parle souvent de :

- fréquence de coupure,
- bande passante,
- atténuation.

Mais si l’on ne précise pas **ce que signifie exactement “couper”**,
ces notions deviennent floues et parfois mal utilisées.

La fréquence de coupure n’est pas un seuil brutal :
c’est une **frontière physique** entre ce que le système
peut transmettre efficacement
et ce qu’il ne peut plus suivre correctement.

---

## 6.2 Définition rigoureuse de la fréquence de coupure

### 6.2.1 Définition énergétique

La fréquence de coupure \( \omega_c \) est définie comme la fréquence
pour laquelle la **puissance transmise** est divisée par deux.

Cette définition est universelle
et indépendante de la nature précise du système,
tant qu’il est linéaire.

---

### 6.2.2 Lien entre puissance et amplitude

Pour un signal sinusoïdal :

- la puissance est proportionnelle au carré de l’amplitude,
- donc une division par deux de la puissance

  correspond à une division par \( \sqrt{2} \) de l’amplitude.

Ainsi, à la coupure :

$$
|H(j\omega_c)| = \frac{1}{\sqrt{2}}
$$

---

## 6.3 Origine du −3 dB

### 6.3.1 Définition du décibel (rappel)

Le gain en décibels est défini par :

$$
G_{\mathrm{dB}}(\omega) = 20 \log_{10} |H(j\omega)|
$$

---

### 6.3.2 Application à la coupure

En utilisant \( |H(j\omega_c)| = 1/\sqrt{2} \) :

$$
G_{\mathrm{dB}}(\omega_c)
= 20 \log_{10}\left(\frac{1}{\sqrt{2}}\right)
$$

Or :

$$
\log_{10}\left(\frac{1}{\sqrt{2}}\right)
= -\frac{1}{2} \log_{10}(2)
$$

D’où :

$$
G_{\mathrm{dB}}(\omega_c)
= -10 \log_{10}(2)
\approx -3.01\ \text{dB}
$$

Le −3 dB est donc la traduction logarithmique
d’une perte de puissance par un facteur 2.

---

## 6.4 Démonstration formelle pour un premier ordre

### 6.4.1 Rappel de la fonction de transfert

Pour un système du premier ordre :

$$
H(j\omega) = \frac{1}{1 + j\omega\tau}
$$

---

### 6.4.2 Calcul du module

Le module vaut :

$$
|H(j\omega)| = \frac{1}{\sqrt{1 + (\omega\tau)^2}}
$$

---

### 6.4.3 Condition de coupure

On impose :

$$
\frac{1}{\sqrt{1 + (\omega_c\tau)^2}} = \frac{1}{\sqrt{2}}
$$

En élevant au carré :

$$
1 + (\omega_c\tau)^2 = 2
$$

D’où :

$$
(\omega_c\tau)^2 = 1
$$

et finalement :

$$
\boxed{\omega_c = \frac{1}{\tau}}
$$

---

## 6.5 Interprétation physique profonde

### 6.5.1 Temps caractéristique et période

À la coupure :

$$
\omega_c\tau = 1
$$

Cela signifie que :

- la période du signal devient comparable

  au temps caractéristique du système.

Le système n’a alors **juste pas assez de temps**
pour suivre correctement les variations imposées.

---

### 6.5.2 Image mentale durable

On peut voir la coupure comme :

- la cadence maximale que le système peut suivre sans perte majeure,
- le point où l’effort demandé devient comparable

  à la capacité physique de réaction.

---

## 6.6 Coupure et phase

À la fréquence de coupure :

- le gain vaut −3 dB,
- la phase vaut −45°.

Ce résultat n’est pas un hasard :
il traduit un équilibre parfait
entre composante résistive et réactive.

---

## 6.7 Bande passante et filtre

### 6.7.1 Définition de la bande passante

La bande passante est l’intervalle de fréquences
pour lesquelles la transmission reste “acceptable”.

Pour un premier ordre passe-bas :

- la bande passante est définie par \( \omega \le \omega_c \).

---

### 6.7.2 Acceptable n’est pas absolu

Le −3 dB correspond à une définition standard,
mais selon les applications :

- on peut exiger −1 dB,
- ou tolérer −6 dB.

La coupure reste néanmoins
un repère universel.

---

## 6.8 Conséquences pratiques pour le design

Un mauvais choix de coupure peut :

- lisser excessivement un signal,
- introduire un retard gênant,
- masquer une information utile.

Le choix de \( \tau \) est donc toujours
un **compromis** entre :

- rapidité,
- fidélité,
- rejet des perturbations.

---

## 6.9 Coupure idéale vs coupure réelle

Dans un système réel :

- les pentes ne sont jamais infinies,
- les transitions sont progressives,
- les composants ont des limites.

Ces aspects seront approfondis
dans les chapitres suivants
et dans les annexes pratiques.

---

## 6.10 Transition vers les applications numériques

À ce stade, nous savons :

- définir rigoureusement une coupure,
- la calculer,
- l’interpréter physiquement.

Il est maintenant temps de passer
du concept au **design réel** :
choix des composants,
vérification temporelle,
vérification fréquentielle.

C’est l’objet du chapitre suivant :
**Applications numériques et design de filtres**.
