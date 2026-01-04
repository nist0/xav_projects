# Chapitre 15 — Applications numériques complètes : filtres actifs à AOP (second ordre)

## 15.1 Objectif du chapitre

Ce chapitre a pour objectif de montrer, de manière **rigoureuse et concrète**, comment :

- concevoir un filtre du second ordre à AOP,
- partir d’un cahier des charges réaliste,
- choisir une topologie adaptée,
- calculer précisément tous les composants,
- vérifier le comportement :
  - temporel,
  - fréquentiel,
- interpréter les résultats en ingénieur,
- comprendre les limites réelles liées à l’AOP.

Ce chapitre est le **pendant actif** du chapitre précédent sur les RLC passifs.

---

## 15.2 Pourquoi utiliser des filtres actifs

Les filtres RLC passifs présentent plusieurs limitations pratiques :

- inductances volumineuses,
- pertes importantes,
- faible précision,
- sensibilité mécanique et thermique.

Les filtres actifs permettent de :

- remplacer les inductances par des résistances et condensateurs,
- ajuster précisément \( Q \) et \( \omega_0 \),
- obtenir des gains,
- intégrer facilement le filtrage sur PCB.

👉 En pratique, **la majorité des filtres analogiques modernes sont actifs**.

---

## 15.3 Choix de la topologie : filtre passe-bande de Rauch (multiple feedback)

### 15.3.1 Justification du choix

Nous choisissons la topologie **passe-bande à rétroaction multiple (MFB)** :

- filtre du second ordre réel,
- bonne maîtrise de \( Q \),
- très utilisé en instrumentation et audio,
- directement comparable au RLC série passe-bande.

---

### 15.3.2 Schéma conceptuel (description)

Le montage comprend :

- un AOP en configuration inverseuse,
- deux condensateurs,
- trois résistances,
- une boucle de rétroaction complexe.

Ce réseau externe introduit un **second stockage d’énergie effectif**.

---

## 15.4 Cahier des charges numérique

On reprend volontairement une spécification proche du passif,
pour comparer directement :

- type : **passe-bande**
- fréquence centrale :  

  \( f_0 = 10\,\text{kHz} \)

- facteur de qualité :  

  \( Q = 5 \)

- gain en bande :  

  \( G = 1 \)

- AOP réel, stable en gain unité

---

## 15.5 Modèle théorique du filtre MFB

### 15.5.1 Fonction de transfert canonique

La fonction de transfert d’un passe-bande du second ordre s’écrit :

$$
H(j\omega)
= \frac{K \frac{j\omega}{\omega_0}}
{1 + \frac{j\omega}{Q\omega_0}
+ \left(\frac{j\omega}{\omega_0}\right)^2}
$$

où :

- \( \omega_0 = 2\pi f_0 \),
- \( Q \) est le facteur de qualité,
- \( K \) est le gain en bande.

---

## 15.6 Stratégie de calcul des composants

### 15.6.1 Méthode ingénieur

En pratique :

1. on fixe \( C_1 = C_2 = C \),
2. on choisit une valeur normalisée,
3. on calcule les résistances,
4. on ajuste légèrement si nécessaire.

Cette méthode réduit les erreurs et facilite l’implantation.

---

## 15.7 Choix des condensateurs

On choisit :

$$
C_1 = C_2 = 10\,\text{nF}
$$

Valeur :

- stable,
- facilement disponible,
- adaptée à 10 kHz.

---

## 15.8 Calcul des résistances (filtre MFB passe-bande)

Pour la topologie MFB standard, on utilise :

$$
R_2 = \frac{Q}{\omega_0 C}
$$

$$
R_3 = \frac{Q}{\omega_0 C (2Q^2 - 1)}
$$

$$
R_1 = \frac{2Q}{\omega_0 C}
$$

---

### 15.8.1 Calcul numérique

Avec :

- \( \omega_0 = 62\,800\ \text{rad/s} \)
- \( Q = 5 \)
- \( C = 10\,\text{nF} \)

On obtient :

- \( R_2 \approx 7.96\,\text{k}\Omega \)
- \( R_1 \approx 15.9\,\text{k}\Omega \)
- \( R_3 \approx 0.84\,\text{k}\Omega \)

Valeurs normalisées retenues :

- \( R_1 = 16\,\text{k}\Omega \)
- \( R_2 = 8.2\,\text{k}\Omega \)
- \( R_3 = 820\,\Omega \)

---

## 15.9 Vérification fréquentielle

### 15.9.1 Fréquence centrale

Le pic de gain apparaît à :

$$
f_0 \approx 10\,\text{kHz}
$$

Conforme au cahier des charges.

---

### 15.9.2 Bande passante

$$
\Delta f = \frac{f_0}{Q} = 2\,\text{kHz}
$$

Bande passante :

- environ 9 kHz à 11 kHz.

---

### 15.9.3 Lecture du diagramme de Bode

On observe :

- pente +20 dB/décade en basse fréquence,
- pic centré sur \( f_0 \),
- pente −20 dB/décade en haute fréquence,
- phase traversant −90° à \( f_0 \).

Signature claire d’un **second ordre passe-bande**.

---

## 15.10 Vérification temporelle

### 15.10.1 Réponse à une impulsion

À une impulsion :

- oscillation amortie visible,
- fréquence d’oscillation proche de \( f_0 \),
- extinction rapide (Q maîtrisé).

---

### 15.10.2 Réponse à un échelon

La réponse à échelon présente :

- un dépassement,
- des oscillations amorties,
- un temps d’établissement lié à \( Q \).

👉 Exactement le comportement prédit par la théorie du second ordre.

---

## 15.11 Contraintes liées à l’AOP réel

### 15.11.1 Produit gain–bande (GBW)

Pour un filtre à 10 kHz et \( Q = 5 \) :

- la fréquence interne de l’AOP doit être

  **au moins 20 à 50 fois supérieure** à \( f_0 \).

Sinon :

- perte de \( Q \),
- décalage de \( f_0 \),
- phase dégradée.

---

### 15.11.2 Choix réaliste d’AOP

AOP adaptés :

- TL072
- NE5532
- OPA2134
- LM358 (limite)

Le choix de l’AOP est **partie intégrante du design**.

---

## 15.12 Comparaison avec le filtre RLC passif

| Critère | RLC passif | Filtre actif |
| --- | --- | --- |
| Inductance | Oui | Non |
| Taille | Grande | Faible |
| Réglage de Q | Difficile | Précis |
| Gain | ≤ 1 | Ajustable |
| Sensibilité | Forte | Modérée |

👉 En pratique : **le filtre actif gagne presque toujours**.

---

## 15.13 Lecture ingénieur globale

Ce chapitre montre que :

- un filtre actif est un second ordre complet,
- l’AOP remplace efficacement L,
- la stabilité dépend directement de la dynamique,
- les marges de phase restent cruciales.

Un filtre actif **n’échappe jamais** aux lois du second ordre.

---

## 15.14 Transition naturelle

À ce stade, le lecteur comprend :

- filtres passifs et actifs,
- premier et second ordre,
- temps et fréquence,
- stabilité et oscillations.

La suite logique du cours peut être :

- **stabilité avancée et compensation**
- **EMI / CEM et filtrage réel**
- **tests corrigés et problèmes de synthèse**
