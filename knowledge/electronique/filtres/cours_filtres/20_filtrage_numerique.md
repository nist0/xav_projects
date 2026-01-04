# Chapitre 20 — Filtrage numérique : de l’analogique échantillonné au DSP

## 20.1 Pourquoi le filtrage numérique n’est pas une autre discipline

Le filtrage numérique est souvent présenté comme :

- une discipline mathématique,
- basée sur des algorithmes,
- détachée de l’électronique analogique.

C’est une **illusion pédagogique**.

En réalité :
> le filtrage numérique est la **transposition discrète**
> des mêmes lois physiques vues en analogique.

Les notions de :

- bande passante,
- ordre,
- pôles,
- zéros,
- stabilité,
- résonance,

**ne disparaissent pas** :  
elles changent de support.

---

## 20.2 Le signal numérique : une réalité physique discrétisée

### 20.2.1 Échantillonnage : définition

L’échantillonnage consiste à :

- observer un signal continu,
- à intervalles réguliers \( T_e \),
- pour produire une suite discrète.

On définit :
\[
f_e = \frac{1}{T_e}
\]

---

### 20.2.2 Interprétation physique

Un convertisseur analogique–numérique :

- “fige” le signal à des instants donnés,
- introduit une mémoire temporelle,
- transforme un système continu en système à états discrets.

👉 Le temps devient un **indice**, pas une variable continue.

---

## 20.3 Théorème de Shannon–Nyquist (vision ingénieur)

### 20.3.1 Énoncé

Un signal de bande limitée à \( f_{max} \)
peut être parfaitement reconstruit si :

\[
f_e > 2 f_{max}
\]

---

### 20.3.2 Lecture physique

- au-delà de \( f_e/2 \), le système est aveugle,
- les hautes fréquences se replient,
- le bruit devient signal.

👉 Le repliement spectral (aliasing) est une **instabilité fréquentielle**.

---

## 20.4 Nécessité du filtrage analogique avant le numérique

### 20.4.1 Filtre anti-repliement

Avant tout ADC :

- un filtre analogique est indispensable,
- souvent du second ou du troisième ordre.

Sans lui :

- le DSP travaille sur des données corrompues,
- aucun algorithme ne peut corriger l’erreur.

---

### 20.4.2 Continuité avec les chapitres précédents

Le filtre anti-aliasing est :

- un filtre RLC ou actif,
- dimensionné exactement comme précédemment.

👉 **Le numérique dépend de l’analogique.**

---

## 20.5 Représentation fréquentielle discrète

### 20.5.1 Fréquence normalisée

En DSP, on travaille avec :
\[
\Omega = \frac{2\pi f}{f_e}
\]

Le spectre est périodique de période \( 2\pi \).

---

### 20.5.2 Image mentale

Le spectre discret est comme :

- un cylindre enroulé,
- où les hautes fréquences reviennent par l’arrière.

👉 D’où la nécessité absolue du filtrage préalable.

---

## 20.6 Filtres numériques : analogies directes

### 20.6.1 Filtre numérique du premier ordre

Équation typique :
\[
y[n] = a\,y[n-1] + b\,x[n]
\]

- mémoire simple,
- comportement identique à un RC discret,
- stabilité si \( |a| < 1 \).

---

### 20.6.2 Filtre numérique du second ordre (biquad)

Forme générale :
\[
y[n] = a_1 y[n-1] + a_2 y[n-2]

+ b_0 x[n] + b_1 x[n-1] + b_2 x[n-2]

\]

👉 C’est l’équivalent discret d’un **RLC**.

---

## 20.7 Pôles, zéros et stabilité en DSP

### 20.7.1 Différence clé avec l’analogique

- analogique : stabilité si pôles à gauche,
- numérique : stabilité si pôles **dans le cercle unité**.

---

### 20.7.2 Lecture géométrique

Le cercle unité joue le rôle :

- de l’axe imaginaire en continu.

Plus un pôle est proche du cercle :

- plus le système est résonant,
- plus le Q est élevé.

---

## 20.8 FIR vs IIR : lecture système

### 20.8.1 Filtres FIR

- pas de rétroaction,
- toujours stables,
- souvent d’ordre élevé.

👉 Analogie : RC empilés.

---

### 20.8.2 Filtres IIR

- rétroaction présente,
- faible ordre,
- résonance possible.

👉 Analogie : RLC / filtres actifs.

---

## 20.9 Réponse temporelle et fréquentielle

### 20.9.1 Réponse à un échelon

- FIR : réponse finie,
- IIR : décroissance exponentielle ou oscillante.

---

### 20.9.2 Diagrammes de Bode numériques

- mêmes pentes,
- mêmes compromis,
- lecture identique… **avec un autre axe**.

---

## 20.10 Contraintes numériques réelles

### 20.10.1 Quantification

- bruit ajouté,
- limite de dynamique,
- saturation possible.

Un DSP mal dimensionné **oscille numériquement**.

---

### 20.10.2 Temps de calcul

Un filtre numérique :

- consomme du temps CPU,
- introduit un délai,
- impacte la stabilité globale.

---

## 20.11 Lecture ingénieur globale

Un ingénieur système se demande toujours :

1. où filtrer (analogique / numérique) ?
2. quel ordre minimal ?
3. quel impact sur la latence ?
4. quelle stabilité globale ?

👉 Le filtrage est **hybride par nature**.

---

## 20.12 Transition naturelle vers le DSP

Le DSP n’est pas :

- une boîte noire mathématique,
- mais une **machine à implémenter des systèmes dynamiques**.

👉 Prochain chapitre :
**DSP : vision système, limites physiques et algorithmiques**.
