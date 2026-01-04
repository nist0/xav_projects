# Chapitre 16 — Stabilité et compensation des systèmes du second ordre

## 16.1 Pourquoi la stabilité est un problème d’ingénieur (et pas de mathématicien)

Dans un monde idéal, une fois un système conçu et calculé,
son comportement serait définitivement fixé.

Dans le monde réel :

- les composants ont des tolérances,
- la température varie,
- la charge change,
- le PCB ajoute des pôles parasites,
- l’environnement électromagnétique perturbe le système.

👉 La stabilité n’est donc **jamais un état acquis**,
mais une **propriété à garantir avec des marges**.

Un système instable :

- n’est pas seulement “incorrect”,
- il est **inutilisable**, voire **destructif**.

---

## 16.2 Définition rigoureuse de la stabilité

### 16.2.1 Stabilité BIBO (rappel)

Un système est dit **BIBO stable** si :

> toute entrée bornée produit une sortie bornée.

Cette définition est utile mais incomplète :
elle ne dit rien sur la dynamique interne du système.

---

### 16.2.2 Stabilité dynamique (point de vue physique)

On étudie la **réponse libre** du système
(c’est-à-dire en l’absence d’entrée).

Trois cas sont possibles :

- la réponse décroît → système stable,
- la réponse oscille sans décroître → stabilité limite,
- la réponse diverge → système instable.

Ces comportements sont **entièrement déterminés par les pôles**.

---

## 16.3 Stabilité et position des pôles (clé universelle)

### 16.3.1 Règle fondamentale

Dans le plan complexe :

- pôle à partie réelle négative → décroissance exponentielle,
- pôle à partie réelle nulle → oscillation permanente,
- pôle à partie réelle positive → divergence.

👉 **La stabilité dépend uniquement du signe de la partie réelle des pôles.**

---

### 16.3.2 Cas du second ordre

Pour un système du second ordre canonique :

$$
p_{1,2}
= -\zeta \omega_0
\pm j \omega_0 \sqrt{1 - \zeta^2}
$$

- \( \zeta > 0 \) : système stable,
- \( \zeta = 0 \) : oscillateur pur,
- \( \zeta < 0 \) : instabilité exponentielle.

La valeur de \( \zeta \) contrôle :

- la rapidité,
- le dépassement,
- la robustesse.

---

## 16.4 Interprétation physique de l’instabilité

### 16.4.1 Vision énergétique

Un système instable est un système où :

- l’énergie injectée n’est pas dissipée,
- les mécanismes de rétroaction deviennent positifs,
- les oscillations s’auto-entretiennent.

En électronique :

- un AOP instable **amplifie son propre bruit**.

---

### 16.4.2 Analogie mécanique

- amortissement positif → frottements → stabilité,
- amortissement nul → oscillation,
- amortissement négatif → moteur → divergence.

👉 Un système instable est un système **qui crée artificiellement de l’énergie dynamique**.

---

## 16.5 Stabilité en boucle fermée

### 16.5.1 Rappel fondamental

La fonction de transfert en boucle fermée est :

$$
H(j\omega)
= \frac{G(j\omega)}{1 + G(j\omega) H_r(j\omega)}
$$

La stabilité dépend du **dénominateur** :

$$
1 + G(j\omega) H_r(j\omega)
$$

C’est la dynamique de la **boucle**, pas du composant seul.

---

### 16.5.2 Rôle de la phase

Une boucle devient instable si :

- le gain est ≥ 1,
- et la phase vaut −180°.

👉 Condition de Barkhausen (forme fréquentielle).

---

## 16.6 Marge de phase : notion centrale

### 16.6.1 Définition rigoureuse

La **marge de phase** est :

> l’écart angulaire entre la phase réelle du système
> et −180° à la fréquence où le gain vaut 0 dB.

Elle mesure la **distance à l’instabilité**.

---

### 16.6.2 Lien avec le second ordre

| Marge de phase | Interprétation |
| --- | --- |
| > 60° | Système très amorti |
| 45–60° | Bon compromis |
| 30–45° | Oscillations visibles |
| < 30° | Instabilité probable |

👉 La marge de phase est l’équivalent fréquentiel de \( \zeta \).

---

## 16.7 Origine des instabilités en pratique

### 16.7.1 Pôles non anticipés

- capacités parasites,
- inductances de pistes,
- charges capacitives,
- câbles, connecteurs.

👉 Le schéma ne montre **pas tout**.

---

### 16.7.2 AOP mal adapté

- GBW insuffisant,
- compensation interne inadaptée,
- montage hors domaine de stabilité.

Un AOP n’est **pas universel**.

---

## 16.8 Compensation : principe général

### 16.8.1 Objectif de la compensation

Compensation =  
👉 **repositionner les pôles et zéros**
pour garantir une marge suffisante.

---

### 16.8.2 Compensation dominante

- ajout d’un pôle bas,
- réduction de la bande passante,
- stabilité assurée.

Très robuste, mais lente.

---

### 16.8.3 Compensation par zéro

- ajout d’un zéro pour corriger la phase,
- amélioration des marges,
- conception plus délicate.

---

## 16.9 Lecture ingénieur avant toute correction

Avant d’ajouter un condensateur “au hasard” :

1. identifier les pôles dominants,
2. tracer mentalement le Bode,
3. estimer la marge de phase,
4. choisir la stratégie de compensation.

👉 La stabilité est **une démarche**, pas une rustine.

---

## 16.10 Ce que ce chapitre doit avoir ancré

À ce stade, le lecteur doit comprendre que :

- la stabilité est un problème **dynamique**,
- tout second ordre peut devenir instable,
- la marge de phase est la grandeur clé,
- les AOP obéissent aux mêmes lois que les RLC.

---

## 16.11 Transition maîtrisée

Même un système stable peut être inutilisable
s’il est perturbé par son environnement électromagnétique.

👉 Le chapitre suivant approfondira :
**EMI / CEM et filtrage réel sur carte**.
