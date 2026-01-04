# Chapitre 18 — Tests, exercices et problèmes corrigés (synthèse et entraînement)

## 18.1 Objectif du chapitre

Ce chapitre a pour objectif de :

- consolider les notions fondamentales vues dans tout le cours,
- entraîner la lecture qualitative avant le calcul,
- développer des réflexes d’ingénieur,
- relier équations, Bode, réponses temporelles et réalité physique.

Les exercices sont organisés selon une progression volontaire :

1. compréhension conceptuelle,
2. lecture qualitative,
3. calcul dirigé,
4. problèmes de synthèse,
5. diagnostics réalistes.

---

## 18.2 Test 1 — Ordre d’un système (fondamental)

### Énoncé

On considère les systèmes suivants :

1. un circuit RC série,
2. un circuit RLC série,
3. un filtre actif à AOP avec deux condensateurs.

**Questions :**

- déterminer l’ordre de chaque système,
- justifier sans écrire d’équation différentielle.

---

### Correction commentée

- RC série :
  - un seul stockage d’énergie (condensateur),
  - **ordre 1**.

- RLC série :
  - stockage électrique (C) + magnétique (L),
  - **ordre 2**.

- filtre actif à deux condensateurs :
  - deux mémoires dynamiques,
  - **ordre 2**, même sans inductance réelle.

👉 **Réflexe clé** :  
l’ordre se lit d’abord dans le **nombre de stockages d’énergie**, pas dans la complexité du schéma.

---

## 18.3 Test 2 — Oscillation possible ou impossible

### Énoncé

Peut-on observer une oscillation :

1. dans un RC ?
2. dans un RLC sur-amorti ?
3. dans un AOP avec marge de phase de 20° ?

---

### Correction commentée

1. RC :
   - impossible,
   - un seul stockage d’énergie.

2. RLC sur-amorti :
   - pas d’oscillation,
   - deux stockages mais pertes dominantes.

3. AOP avec 20° de marge :
   - oscillations probables,
   - comportement sous-amorti proche de l’instabilité.

👉 **Idée clé** :  
oscillation = échange d’énergie + amortissement insuffisant.

---

## 18.4 Test 3 — Lecture qualitative d’un diagramme de Bode

### Énoncé

Un diagramme de Bode présente :

- pente 0 dB à basse fréquence,
- pente −40 dB/décade à haute fréquence,
- pic de résonance,
- phase tendant vers −180°.

**Questions :**

- ordre du système ?
- nature de l’amortissement ?
- risque principal ?

---

### Correction commentée

- pente −40 dB/décade → **second ordre**,
- pic de résonance → **sous-amorti**,
- phase → −180° → **risque d’instabilité en boucle**.

👉 **Lecture ingénieur** :  
ce système est performant mais fragile.

---

## 18.5 Exercice 1 — Calcul guidé (premier ordre)

### Énoncé

On souhaite un filtre RC passe-bas de coupure :

- \( f_c = 1\,\text{kHz} \)

1. calculer \( \tau \),
2. proposer un couple \( R, C \),
3. donner le temps pour atteindre 95 %.

---

### Correction

1.  

\[
\tau = \frac{1}{2\pi f_c}
\approx 159\,\mu s
\]

2.  

Choix possible :

- \( C = 100\,\text{nF} \)
- \( R \approx 1.6\,k\Omega \)

3.  

95 % atteint vers :
\[
t \approx 3\tau \approx 480\,\mu s
\]

👉 **Réflexe** :  
le calcul donne aussi une **intuition temporelle immédiate**.

---

## 18.6 Exercice 2 — Second ordre et paramètres canoniques

### Énoncé

Un système du second ordre a :

- \( \omega_0 = 10^4\,\text{rad/s} \)
- \( \zeta = 0.5 \)

1. déterminer \( Q \),
2. qualifier le régime temporel,
3. prédire la forme du Bode.

---

### Correction

1.  

\[
Q = \frac{1}{2\zeta} = 1
\]

2.  
- régime sous-amorti,
- oscillations modérées.

3.  
- pas de pic de résonance marqué,
- transition douce,
- pente −40 dB/décade après coupure.

👉 **Lien clé** :  
\( \zeta \leftrightarrow Q \leftrightarrow Bode \leftrightarrow temps.

---

## 18.7 Problème de synthèse — Design complet

### Énoncé

Concevoir un filtre passe-bas du second ordre :

- \( f_c = 2\,\text{kHz} \)
- réponse sans dépassement,
- technologie AOP.

---

### Correction structurée (esquisse)

1. Choix :
   - \( Q = 0.707 \) (Butterworth)
2. Topologie :
   - Sallen-Key
3. Calcul :
   - choix de \( C \),
   - calcul de \( R \)
4. Vérification :
   - Bode plat,
   - phase maîtrisée,
   - bonne marge de phase.

👉 **Important** :  
le choix de \( Q \) est **avant** le calcul.

---

## 18.8 Problème terrain — Diagnostic EMI / stabilité

### Énoncé

Un filtre actif fonctionne en simulation mais :

- oscille sur PCB,
- bruit HF en sortie,
- sensibilité à la charge.

---

### Correction (raisonnement ingénieur)

Hypothèses probables :

- pôles parasites PCB,
- charge capacitive,
- marge de phase réduite.

Actions correctives :

- résistance série en sortie,
- condensateur de compensation,
- découplage local renforcé,
- réduction de la bande passante.

👉 **Message clé** :  
le schéma n’est jamais toute l’histoire.

---

## 18.9 Test final — Vrai / Faux (lecture ingénieur)

| Affirmation | Verdict | Justification |
| --- | --- | --- |
| Un RC peut résonner | Faux | 1 stockage |
| Plus de Q = toujours mieux | Faux | instabilité |
| −3 dB est arbitraire | Faux | demi-puissance |
| EMI = bruit externe | Faux | souvent interne |
| AOP idéal = stable | Faux | modèle incomplet |

---

## 18.10 Ce que ce chapitre doit avoir ancré

À l’issue de ce chapitre, le lecteur doit savoir que :

- lire avant de calculer,
- identifier l’ordre avant de simuler,
- relier temps et fréquence,
- anticiper l’instabilité,
- penser énergie, pas seulement tension.

---

## 18.11 Conclusion générale du cours

Ce cours a montré que :

- les filtres sont des systèmes dynamiques,
- le premier ordre est robuste mais limité,
- le second ordre est puissant mais exigeant,
- les AOP obéissent aux mêmes lois que les RLC,
- la stabilité et l’EMI sont des problèmes d’énergie et de dynamique.

L’objectif n’était pas de fournir des recettes,
mais de construire des **outils mentaux durables**,
utilisables toute une vie d’ingénieur.
