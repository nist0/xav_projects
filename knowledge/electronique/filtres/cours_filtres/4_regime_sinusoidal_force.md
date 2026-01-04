# Chapitre 4 — Régime sinusoïdal forcé

## 4.1 Pourquoi étudier le régime sinusoïdal

Jusqu’à présent, l’analyse temporelle a permis de comprendre :

- la dynamique du système,
- son inertie,
- son comportement face à des excitations simples.

Cependant, dans de nombreux contextes (filtrage, transmission, bruit, RF),
les signaux ne sont pas des échelons isolés,
mais des **signaux périodiques** ou **composés de fréquences multiples**.

Le régime sinusoïdal forcé constitue alors un outil fondamental,
car il permet de caractériser complètement un système linéaire
à partir de sa réponse à une excitation sinusoïdale.

---

## 4.2 Propriété clé des systèmes linéaires invariants

Un système **linéaire et invariant dans le temps (LTI)** possède une propriété essentielle :

> soumis à une excitation sinusoïdale,
> il répond par une sinusoïde de même fréquence,
> mais d’amplitude et de phase différentes.

Cette propriété est indépendante de la forme précise du système :
elle découle uniquement de la linéarité et de l’invariance temporelle.

---

## 4.3 Définition du régime sinusoïdal forcé

### 4.3.1 Signal d’entrée

On considère une entrée sinusoïdale :

$$
x(t) = X_0 \cos(\omega t)
$$

où :

- \( X_0 \) est l’amplitude,
- \( \omega \) est la pulsation (en rad/s).

---

### 4.3.2 Réponse du système

Après disparition du régime transitoire,
la sortie s’écrit nécessairement sous la forme :

$$
y(t) = Y_0 \cos(\omega t + \varphi)
$$

Le système agit donc comme :

- un **amplificateur ou atténuateur** (via \( Y_0/X_0 \)),
- un **retardateur ou avanceur de phase** (via \( \varphi \)).

---

## 4.4 Pourquoi passer aux nombres complexes

### 4.4.1 Limite du formalisme réel

La résolution directe des équations différentielles
avec des fonctions trigonométriques
est possible mais lourde et peu lisible.

Le calcul différentiel sur des sinusoïdes
introduit systématiquement des déphasages,
ce qui complique les expressions.

---

### 4.4.2 Astuce fondamentale (justifiée)

On utilise l’identité d’Euler :

$$
\cos(\omega t) = \Re\left\{ e^{j\omega t} \right\}
$$

L’idée clé est la suivante :

- on résout le problème avec \( e^{j\omega t} \),
- on prend la partie réelle à la fin.

Cette méthode est **rigoureusement valide**
pour les systèmes linéaires.

---

## 4.5 Dérivation en régime sinusoïdal

### 4.5.1 Propriété fondamentale

Pour une fonction de la forme :

$$
x(t) = X e^{j\omega t}
$$

on a :

$$
\frac{dx(t)}{dt} = j\omega X e^{j\omega t}
$$

Ainsi :

- dériver revient à multiplier par \( j\omega \),
- intégrer revient à diviser par \( j\omega \).

Cette propriété transforme
les équations différentielles
en équations algébriques.

---

## 4.6 Application au système du premier ordre

### 4.6.1 Équation différentielle de départ

On rappelle l’équation canonique :

$$
\tau \frac{dy(t)}{dt} + y(t) = x(t)
$$

---

### 4.6.2 Passage en régime sinusoïdal

On pose :

$$
x(t) = X e^{j\omega t}
\qquad
y(t) = Y e^{j\omega t}
$$

En substituant dans l’équation :

$$
\tau (j\omega Y e^{j\omega t}) + Y e^{j\omega t} = X e^{j\omega t}
$$

---

### 4.6.3 Simplification

On factorise \( e^{j\omega t} \) :

$$
(j\omega\tau + 1) Y = X
$$

D’où :

$$
\boxed{
\frac{Y}{X} = \frac{1}{1 + j\omega\tau}
}
$$

---

## 4.7 Définition de la fonction de transfert

### 4.7.1 Définition formelle

La **fonction de transfert** est définie comme :

$$
\boxed{
H(j\omega) = \frac{Y}{X}
}
$$

Elle caractérise complètement
le comportement fréquentiel du système.

---

### 4.7.2 Sens physique

La fonction de transfert décrit :

- comment chaque fréquence est transmise,
- comment chaque fréquence est retardée.

Elle est indépendante du signal précis :
elle est une propriété intrinsèque du système.

---

## 4.8 Module et phase

### 4.8.1 Module

Le module :

$$
|H(j\omega)|
$$

représente :

- le gain ou l’atténuation de la fréquence \( \omega \).

---

### 4.8.2 Phase

L’argument :

$$
\arg(H(j\omega))
$$

représente :

- le retard (ou l’avance) introduit par le système.

Un retard de phase correspond physiquement
à une incapacité à suivre instantanément le signal.

---

## 4.9 Interprétation intuitive du premier ordre en fréquence

À basse fréquence (\( \omega\tau \ll 1 \)) :

- le système suit presque parfaitement,
- le gain est proche de 1,
- la phase est proche de 0°.

À haute fréquence (\( \omega\tau \gg 1 \)) :

- le système n’a plus le temps de réagir,
- le gain chute,
- la phase tend vers −90°.

---

## 4.10 Lien profond entre temps et fréquence

La constante de temps \( \tau \) apparaît :

- dans la réponse à échelon,
- dans la fonction de transfert,
- dans le comportement fréquentiel.

Elle constitue le **pont conceptuel**
entre l’analyse temporelle
et l’analyse fréquentielle.

---

## 4.11 Pourquoi le régime sinusoïdal suffit

Tout signal réel peut être décomposé
en une somme de sinusoïdes (série de Fourier).

Connaître la réponse du système
à chaque fréquence
permet donc de prédire sa réponse
à n’importe quel signal.

C’est cette propriété qui justifie
l’importance centrale
du régime sinusoïdal forcé.

---

## 4.12 Transition vers les diagrammes de Bode

La fonction de transfert fournit :

- le module,
- la phase,

en fonction de la fréquence.

Il devient alors naturel de représenter
ces grandeurs de manière graphique
afin de lire rapidement
le comportement du système.

Le chapitre suivant introduira
les **diagrammes de Bode**,
outil central de l’ingénieur.
