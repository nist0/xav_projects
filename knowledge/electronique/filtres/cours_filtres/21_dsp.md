# Chapitre 21 — DSP : vision système, limites physiques et algorithmiques

## 21.1 Pourquoi le DSP doit être compris comme un système dynamique

Le DSP (Digital Signal Processing) est souvent enseigné comme :

- une suite d’algorithmes,
- des équations en z,
- des implémentations logicielles.

Cette vision est **incomplète**.

En réalité :
> un DSP est un **système dynamique discret**
> qui traite de l’énergie, du bruit, du temps et de la stabilité.

Les mêmes questions qu’en analogique se posent :

- ordre du système,
- pôles et zéros,
- stabilité,
- latence,
- compromis temps / fréquence.

---

## 21.2 Le DSP dans la chaîne physique réelle

### 21.2.1 Chaîne complète typique

Un système réel contient toujours :

1. capteur (monde physique),
2. conditionnement analogique,
3. filtre anti-repliement,
4. ADC,
5. DSP (filtrage, démodulation, décision),
6. DAC (éventuel),
7. étage de puissance ou de communication.

👉 Le DSP **n’est jamais seul**.

---

### 21.2.2 Conséquence majeure

Toute erreur ou approximation **avant** le DSP :

- est définitive,
- ne peut pas être corrigée algorithmiquement.

👉 Le DSP n’est pas un correcteur universel.

---

## 21.3 Ordre et mémoire en DSP

### 21.3.1 Mémoire explicite

Dans un DSP :

- chaque échantillon mémorisé est un état,
- un filtre d’ordre \( N \) possède \( N \) mémoires.

👉 L’ordre est **visible dans le code**.

---

### 21.3.2 Comparaison directe

| Domaine | Mémoire |
| --- | --- |
| RC analogique | Condensateur |
| RLC | C + L |
| Filtre IIR | registres |
| DSP global | RAM + pipeline |

Le DSP remplace les composants physiques
par de la mémoire et du calcul.

---

## 21.4 Stabilité numérique : une contrainte absolue

### 21.4.1 Stabilité théorique

Un filtre numérique est stable si :

- tous ses pôles sont **strictement à l’intérieur du cercle unité**.

C’est la traduction directe de la condition analogique.

---

### 21.4.2 Stabilité pratique

Même un filtre théoriquement stable peut :

- devenir instable par quantification,
- saturer,
- osciller numériquement.

👉 Le DSP peut **créer de l’instabilité sans bruit externe**.

---

## 21.5 Quantification : bruit et non-linéarité

### 21.5.1 Quantification des amplitudes

- chaque opération arrondit,
- le bruit de quantification est injecté partout,
- les boucles le réamplifient.

Un DSP est **fondamentalement non linéaire**.

---

### 21.5.2 Image mentale

La quantification agit comme :

- un frottement irrégulier,
- qui secoue le système à chaque pas.

👉 À fort Q, ce bruit devient dominant.

---

## 21.6 Latence : la grandeur oubliée

### 21.6.1 Origines de la latence

La latence provient de :

- l’échantillonnage,
- le filtrage,
- le pipeline,
- les buffers.

---

### 21.6.2 Conséquence système

La latence :

- dégrade les boucles de contrôle,
- limite les asservissements rapides,
- crée des problèmes de phase.

👉 En DSP, **la phase coûte du temps réel**.

---

## 21.7 DSP et contrôle : lien direct

### 21.7.1 Boucles numériques

Dans une boucle numérique :

- le DSP est dans la boucle,
- la latence est un pôle,
- l’échantillonnage est une contrainte.

Un système stable en analogique
peut devenir instable en numérique.

---

### 21.7.2 Lecture ingénieur

Toujours se demander :

- quelle est la fréquence de boucle ?
- combien d’échantillons de retard ?
- quelle marge de phase numérique ?

---

## 21.8 DSP en RF moderne (SDR)

### 21.8.1 Radio définie par logiciel

En SDR :

- le filtrage devient numérique,
- la sélectivité est algorithmique,
- mais les contraintes physiques restent.

Le DSP **ne supprime pas la RF** :
il la déplace.

---

### 21.8.2 Limites physiques

- bruit de phase des horloges,
- jitter ADC,
- dynamique limitée,
- consommation énergétique.

👉 Le DSP est puissant, mais pas gratuit.

---

## 21.9 Compromis fondamentaux en DSP

| Gain | Coût |
| --- | --- |
| Plus d’ordre | Plus de latence |
| Plus de Q | Moins de stabilité |
| Plus de précision | Plus de bruit numérique |
| Plus de flexibilité | Plus de consommation |

Il n’existe **aucun réglage parfait**.

---

## 21.10 Lecture ingénieur finale

Un ingénieur DSP expérimenté :

- raisonne en système,
- anticipe les limites physiques,
- choisit l’ordre minimal,
- combine analogique et numérique.

👉 Le DSP est un **outil**, pas une magie.

---

## 21.11 Conclusion générale de l’ouverture DSP / RF

Cette partie a montré que :

- RC, RLC, AOP, RF, DSP obéissent aux **mêmes lois**,
- l’ordre et la stabilité sont universels,
- le filtrage est une gestion de l’énergie dans le temps,
- le passage au numérique ne supprime aucune contrainte fondamentale.

Ce cours fournit une **vision unifiée**,
rarement enseignée telle quelle,
mais essentielle à tout ingénieur système moderne.
