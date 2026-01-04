# Chapitre 13 — Applications numériques complètes des systèmes du second ordre

## 13.1 Objectif du chapitre

Ce chapitre a pour objectif de montrer, **pas à pas**, comment :

- partir d’un cahier des charges réaliste,
- choisir une architecture du second ordre,
- calculer les composants (R, L, C),
- vérifier le comportement :
  - en régime temporel,
  - en régime fréquentiel,
- interpréter les résultats comme un ingénieur,
- relier le modèle mathématique à une carte réelle.

Ce chapitre constitue un **point d’aboutissement** :
tout ce qui précède y est mobilisé.

---

## 13.2 Cahier des charges de départ

+ \frac{R}{L} j\omega
+ \frac{1}{LC}}
- type de filtre : **passe-bande**
- fréquence centrale :  

  \( f_0 = 10\,\text{kHz} \)

- bande passante modérée
- pas d’instabilité
- composants discrets standards
- comportement temporel maîtrisé (pas d’oscillations excessives)

---

## 13.3 Choix de l’architecture

### 13.3.1 Pourquoi un second ordre

Un filtre passe-bande **nécessite** :

- un rejet des basses fréquences,
- un rejet des hautes fréquences,
- une transmission sélective autour de \( f_0 \).

Un premier ordre est structurellement incapable de produire ce comportement.

---

### 13.3.2 Architecture retenue

On choisit :

- un **RLC série**,
- sortie prise aux bornes de la résistance.

Avantages :

- simplicité,
- lecture directe du courant,
- lien clair avec le facteur de qualité.

---

## 13.4 Modèle théorique du filtre

### 13.4.1 Fonction de transfert

Pour un RLC série avec sortie sur \( R \) :

$$
H(j\omega)
= \frac{R}
{R + j\omega L + \frac{1}{j\omega C}}
$$

Après mise sous forme canonique :

$$
H(j\omega)
= \frac{j\omega \frac{R}{L}}
{(j\omega)^2
+ \frac{R}{L} j\omega
\; + \frac{R}{L} j\omega
\; + \frac{1}{LC}}
$$

---

### 13.4.2 Identification des paramètres canoniques

On identifie :

$$
\omega_0 = \frac{1}{\sqrt{LC}}
\qquad
\zeta = \frac{R}{2}\sqrt{\frac{C}{L}}
\qquad
Q = \frac{1}{2\zeta}
$$

---

## 13.5 Choix de la pulsation propre

On fixe :

$$
f_0 = 10\,\text{kHz}
\qquad
\omega_0 = 2\pi f_0 \approx 62\,800\ \text{rad/s}
$$

---

## 13.6 Choix du facteur de qualité

### 13.6.1 Compromis recherché

On souhaite :

- une sélectivité visible,
- sans oscillations excessives.

On choisit :

$$
Q = 5
\qquad
\zeta = 0.1
$$

Ce choix est typique :

- filtrage réel,
- bonne lisibilité Bode,
- stabilité acceptable.

---

## 13.7 Choix des composants L et C

### 13.7.1 Stratégie pratique

En électronique réelle :

- on choisit souvent **C en premier**,
- puis on déduit L.

Choisissons :

$$
C = 10\,\text{nF}
$$

---

### 13.7.2 Calcul de L

À partir de :

$$
\omega_0 = \frac{1}{\sqrt{LC}}
$$

On obtient :

$$
L = \frac{1}{\omega_0^2 C}
$$

Numériquement :

$$
L = \frac{1}{(62\,800)^2 \times 10^{-8}}
\approx 25\,\text{mH}
$$

Valeur réaliste mais déjà volumineuse :
première alerte ingénieur.

---

## 13.8 Calcul de la résistance R

À partir de :

$$
\zeta = \frac{R}{2}\sqrt{\frac{C}{L}}
$$

On isole \( R \) :

$$
R = 2\zeta \sqrt{\frac{L}{C}}
$$

Numériquement :

$$
R \approx 2 \times 0.1 \times \sqrt{\frac{25 \times 10^{-3}}{10^{-8}}}
\approx 316\ \Omega
$$

Valeur normalisée :

- \( R = 330\,\Omega \)

---

## 13.9 Vérification fréquentielle

### 13.9.1 Fréquence centrale

La fréquence maximale de gain est très proche de \( f_0 \)
(pour \( Q \) modéré).

---

### 13.9.2 Bande passante

Pour un passe-bande du second ordre :

$$
\Delta f = \frac{f_0}{Q}
$$

Ici :

$$
\Delta f = \frac{10\,000}{5} = 2\,000\ \text{Hz}
$$

Le filtre laisse donc passer :

- environ 9 kHz à 11 kHz.

---

### 13.9.3 Lecture du diagramme de Bode

On observe :

- pente +20 dB/décade à basse fréquence,
- pic autour de \( f_0 \),
- pente −20 dB/décade à haute fréquence.

Signature typique d’un passe-bande du second ordre.

---

## 13.10 Vérification temporelle

### 13.10.1 Réponse à une impulsion

Un passe-bande réagit à une impulsion par :

- une oscillation amortie,
- à la fréquence proche de \( f_0 \).

La durée de cette oscillation dépend directement de \( Q \).

---

### 13.10.2 Temps d’établissement

Avec \( Q = 5 \) :

- oscillations visibles,
- mais extinction rapide.

Ce comportement est acceptable
pour de nombreuses applications analogiques.

---

## 13.11 Lecture ingénieur critique

### 13.11.1 Points positifs

- comportement conforme à la théorie,
- sélectivité correcte,
- architecture simple.

---

### 13.11.2 Limitations réelles

- inductance volumineuse,
- résistance série réelle de la bobine,
- dérive thermique possible,
- sensibilité aux tolérances.

👉 **Conclusion ingénieur** :  
ce filtre est pédagogiquement parfait,  
mais technologiquement discutable à 10 kHz.

---

## 13.12 Enseignements fondamentaux

Ce que cet exemple montre clairement :

- le second ordre est puissant mais exigeant,
- le facteur de qualité est un levier critique,
- le choix des composants conditionne tout,
- les limites apparaissent vite sans AOP.

---

## 13.13 Transition naturelle

Cette application numérique montre
pourquoi, en pratique :

- on remplace souvent les RLC passifs,
- par des filtres actifs à AOP,
- qui reproduisent un second ordre

  sans inductance réelle.

👉 Le chapitre suivant pourra donc être :
**Filtres du second ordre à AOP**
ou
**Lien RLC ↔ modèles d’AOP (second ordre déguisé)**.
