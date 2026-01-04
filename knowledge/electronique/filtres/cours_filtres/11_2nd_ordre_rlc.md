# Chapitre 11 — Systèmes RLC et filtres du second ordre

## 11.1 Objectif du chapitre

Ce chapitre a pour but de relier :

- la signification physique du second ordre,
- les circuits RLC concrets,
- les équations différentielles associées,
- les réponses temporelles et fréquentielles,
- et les filtres passe-bas, passe-haut, passe-bande et coupe-bande.

À la fin de ce chapitre, le lecteur doit être capable de :

- reconnaître un second ordre dans un schéma,
- écrire son équation différentielle,
- prévoir qualitativement son comportement,
- comprendre le rôle de chaque composant,
- interpréter correctement résonance et facteur de qualité.

---

## 11.2 Rappel : stockages d’énergie dans un RLC

Un circuit RLC contient :

- une résistance \( R \) → dissipation,
- une inductance \( L \) → stockage magnétique,
- un condensateur \( C \) → stockage électrique.

La présence simultanée de \( L \) et \( C \)
introduit **deux variables d’état indépendantes** :
le système est nécessairement du **second ordre**.

---

## 11.3 Circuit RLC série — équation différentielle

### 11.3.1 Description du montage

On considère un RLC série soumis à une tension d’entrée \( u_e(t) \).

Les grandeurs internes sont :

- le courant \( i(t) \),
- la tension aux bornes du condensateur \( u_C(t) \).

---

### 11.3.2 Application de la loi des mailles

On écrit :

$$
u_e(t) = u_R(t) + u_L(t) + u_C(t)
$$

En utilisant les lois constitutives :

$$
u_e(t) = R i(t) + L \frac{di(t)}{dt} + u_C(t)
$$

Or :

$$
i(t) = C \frac{du_C(t)}{dt}
$$

En substituant :

$$
LC \frac{d^2 u_C(t)}{dt^2}

+ RC \frac{du_C(t)}{dt}
+ u_C(t)

= u_e(t)
$$

---

## 11.4 Mise sous forme canonique

On écrit l’équation sous la forme standard :

$$
\frac{d^2 u_C}{dt^2}

+ 2\zeta\omega_0 \frac{du_C}{dt}
+ \omega_0^2 u_C

= \omega_0^2 u_e
$$

avec :

$$
\omega_0 = \frac{1}{\sqrt{LC}}
\qquad
\zeta = \frac{R}{2}\sqrt{\frac{C}{L}}
$$

Ces deux paramètres caractérisent complètement
la dynamique du système.

---

## 11.5 Pulsation propre \( \omega_0 \)

### 11.5.1 Signification physique

La pulsation propre est la fréquence naturelle
à laquelle le système oscillerait
en l’absence totale de dissipation.

Elle dépend uniquement de \( L \) et \( C \).

---

### 11.5.2 Image mentale

L’inductance et le condensateur
s’échangent l’énergie :

- électrique → magnétique → électrique,

comme un pendule échange énergie potentielle et cinétique.

---

## 11.6 Amortissement et facteur de qualité

### 11.6.1 Coefficient d’amortissement \( \zeta \)

Le coefficient \( \zeta \) mesure
l’importance relative des pertes.

- \( \zeta > 1 \) : sur-amorti
- \( \zeta = 1 \) : amortissement critique
- \( \zeta < 1 \) : sous-amorti

---

### 11.6.2 Facteur de qualité \( Q \)

On définit :

$$
Q = \frac{1}{2\zeta}
$$

Le facteur de qualité mesure :

- la sélectivité fréquentielle,
- la “pureté” de la résonance,
- la durée des oscillations.

---

## 11.7 Réponse temporelle du RLC

### 11.7.1 Régime sur-amorti

- pas d’oscillation,
- retour lent à l’équilibre,
- rarement utilisé en filtrage.

---

### 11.7.2 Régime critique

- retour le plus rapide sans oscillation,
- compromis entre rapidité et stabilité.

---

### 11.7.3 Régime sous-amorti

- oscillations amorties,
- dépassement,
- durée dépendante de \( Q \).

Ce régime est central en électronique et en RF.

---

## 11.8 Fonction de transfert du RLC série

### 11.8.1 Exemple : tension aux bornes du condensateur

La fonction de transfert s’écrit :

$$
H(j\omega)
= \frac{\omega_0^2}{(j\omega)^2

+ 2\zeta\omega_0 j\omega
+ \omega_0^2}

$$

---

### 11.8.2 Interprétation

Le dénominateur de degré 2
introduit :

- deux pôles,
- une phase pouvant atteindre −180°,
- une pente maximale de −40 dB/décade.

---

## 11.9 Diagrammes de Bode d’un second ordre

### 11.9.1 Module

- plateau à basse fréquence,
- pic de résonance possible si \( Q > 1/\sqrt{2} \),
- pente raide après la coupure.

---

### 11.9.2 Phase

- transition progressive,
- passage par −90° autour de la résonance,
- asymptote à −180°.

---

## 11.10 Types de filtres RLC

### 11.10.1 Passe-bas

- sortie sur le condensateur,
- atténuation des hautes fréquences.

---

### 11.10.2 Passe-haut

- sortie sur l’inductance,
- atténuation des basses fréquences.

---

### 11.10.3 Passe-bande

- sortie sur la résistance,
- sélection d’une bande étroite autour de \( \omega_0 \).

---

### 11.10.4 Coupe-bande (notch)

- rejet autour de la résonance,
- utile pour supprimer une fréquence parasite.

---

## 11.11 Sensibilité et limites pratiques

Les filtres du second ordre sont :

- plus sélectifs,
- mais plus sensibles :
  - aux tolérances,
  - aux pertes parasites,
  - aux variations de température.

Un \( Q \) trop élevé
peut rendre le système instable.

---

## 11.12 Lecture ingénieur globale

Un ingénieur doit être capable de dire :

- pourquoi un RLC est du second ordre,
- quel régime temporel il produira,
- s’il est adapté ou non à l’application,
- quels risques il introduit.

Le second ordre est un outil puissant,
mais exige une maîtrise réelle.

---

## 11.13 Transition vers les signaux réels

Jusqu’ici, les analyses ont porté
sur des excitations idéales.

Les signaux réels :

- sont non sinusoïdaux,
- contiennent du bruit,
- présentent des fronts rapides.

Le chapitre suivant analysera
le **comportement des filtres face aux signaux réels**
et les compromis temps / fréquence associés.
