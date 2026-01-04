# Chapitre 9 — Mesure et validation expérimentale

## 9.1 Pourquoi la mesure est une discipline à part entière

Mesurer un filtre ne consiste pas seulement à :

- brancher une sonde,
- lire une valeur.

Toute mesure est :

- une interaction avec le système,
- limitée par l’instrument,
- influencée par le montage.

Un ingénieur ne mesure jamais “la vérité”,
mais une **approximation instrumentée** du réel.

Comprendre cette approximation est essentiel
pour interpréter correctement les résultats.

---

## 9.2 Mesure temporelle à l’oscilloscope

### 9.2.1 Ce que mesure réellement un oscilloscope

Un oscilloscope mesure :

- une tension en fonction du temps,
- à travers une sonde non idéale,
- avec une bande passante finie.

Il ne mesure jamais :

- directement le courant,
- ni la fonction de transfert.

---

### 9.2.2 Bande passante de l’oscilloscope et de la sonde

Une sonde et un oscilloscope ont une bande passante limitée.

Règle pratique :

- pour mesurer correctement un signal,
- la bande passante de l’instrument doit être

  **au moins 5 à 10 fois supérieure**
  à la fréquence la plus élevée du signal.

Sinon :

- les fronts sont arrondis,
- les amplitudes sont sous-estimées.

---

## 9.3 Mesure de la réponse à échelon

### 9.3.1 Principe

Pour un filtre du premier ordre :

- on applique un échelon en entrée,
- on observe la montée ou la descente en sortie.

La forme de la courbe
contient toute l’information temporelle.

---

### 9.3.2 Extraction de la constante de temps

Méthode classique :

- repérer le temps pour atteindre 63 % de la valeur finale,
- ce temps est une estimation directe de \( \tau \).

Cette méthode est robuste
et très utilisée en pratique.

---

### 9.3.3 Sources d’erreur fréquentes

- échelon non idéal (temps de montée trop lent),
- bruit superposé,
- mauvaise référence de masse,
- influence de la sonde.

---

## 9.4 Mesure fréquentielle

### 9.4.1 Méthodes possibles

On peut mesurer la réponse fréquentielle :

- en balayant une sinusoïde (générateur BF),
- avec un analyseur de spectre,
- par transformée de Fourier d’un signal temporel.

Chaque méthode a ses avantages et ses limites.

---

### 9.4.2 Balayage sinusoïdal

Méthode conceptuellement simple :

- on applique une sinusoïde à une fréquence donnée,
- on mesure amplitude et phase en sortie,
- on recommence à une autre fréquence.

Avantage :

- très fidèle à la théorie.

Inconvénient :

- long et fastidieux.

---

## 9.5 Analyse fréquentielle par FFT

### 9.5.1 Principe

On applique un signal riche en fréquences
(pulse, bruit, signal périodique),
puis on calcule la FFT.

Cette méthode permet :

- d’obtenir rapidement une réponse fréquentielle,
- mais nécessite une interprétation rigoureuse.

---

### 9.5.2 Limites de la FFT

- résolution limitée par la durée d’acquisition,
- effets de fenêtre,
- aliasing si l’échantillonnage est mal choisi.

La FFT ne remplace pas la compréhension fréquentielle.

---

## 9.6 Mesure du gain et de la phase

### 9.6.1 Gain

Le gain est mesuré comme le rapport :

- amplitude de sortie / amplitude d’entrée.

Il est souvent exprimé en dB
pour être comparé au diagramme de Bode théorique.

---

### 9.6.2 Phase

La phase est plus délicate à mesurer :

- elle nécessite une référence temporelle commune,
- elle est sensible au bruit et au jitter.

Une erreur de phase est souvent
le premier signe d’un problème de mesure.

---

## 9.7 Effets de l’instrumentation sur le circuit

### 9.7.1 Charge de la sonde

Une sonde possède :

- une résistance d’entrée,
- une capacité d’entrée.

Elle peut modifier :

- la constante de temps,
- la fréquence de coupure.

Pour les filtres simples,
la sonde peut devenir un élément du circuit.

---

### 9.7.2 Masse et boucles

Une mauvaise référence de masse peut :

- introduire du bruit,
- créer des oscillations apparentes,
- fausser totalement la mesure.

La qualité de la masse est aussi importante
que le signal lui-même.

---

## 9.8 Confrontation théorie / mesure

### 9.8.1 Ce qui doit coïncider

- ordre du système,
- forme générale de la réponse,
- ordre de grandeur de \( \tau \),
- pente du Bode.

Une concordance parfaite n’est pas requise,
une cohérence globale l’est.

---

### 9.8.2 Ce qui peut différer

- fréquence exacte de coupure,
- valeur précise du gain,
- phase à haute fréquence.

Ces écarts sont normaux
et doivent être expliqués.

---

## 9.9 Démarche d’ingénieur face à un écart

Face à un écart théorie / mesure,
un ingénieur procède par étapes :

1. vérifier le modèle,
2. vérifier les composants,
3. vérifier le montage,
4. vérifier l’instrumentation,
5. seulement ensuite, conclure.

Cette démarche évite
les diagnostics hâtifs.

---

## 9.10 Transition vers les systèmes du second ordre

Les filtres du premier ordre sont :

- simples,
- robustes,
- prévisibles.

Mais ils sont aussi limités :

- pente faible,
- absence de sélectivité,
- pas de résonance.

Pour dépasser ces limites,
il faut introduire
un second stockage d’énergie.

Le chapitre suivant introduira
la **signification physique du second ordre**,
avant même d’aborder les circuits RLC.
