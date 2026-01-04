# Chapitre 12 — Systèmes du second ordre : vision unifiée (temps, fréquence, physique)

## 12.1 Pourquoi un chapitre unificateur

Les systèmes du second ordre apparaissent :

- en électronique (RLC, AOP),
- en mécanique (masse–ressort–amortisseur),
- en automatique (asservissements),
- en RF (sélectivité, résonance).

Ce chapitre a pour but de montrer que :
> derrière des contextes très différents,
> **la même structure mathématique et physique** est à l’œuvre.

L’objectif n’est plus seulement de savoir calculer,
mais de **reconnaître immédiatement un second ordre**
et d’anticiper son comportement.

---

## 12.2 Forme canonique universelle du second ordre

### 12.2.1 Équation différentielle canonique

Tout système linéaire du second ordre peut s’écrire sous la forme :

$$
\frac{d^2 y(t)}{dt^2}
+ 2\zeta \omega_0 \frac{dy(t)}{dt}
+ \omega_0^2 y(t)
= K \omega_0^2 x(t)
$$

où :

- \( \omega_0 \) est la pulsation propre,
- \( \zeta \) est le coefficient d’amortissement,
- \( K \) est le gain statique.

Cette écriture est **universelle**.

---

### 12.2.2 Ce que dit cette équation, physiquement

- le terme en \( y \) représente un **rappel vers l’équilibre**,
- le terme en \( \frac{dy}{dt} \) représente les **pertes**,
- le terme en \( \frac{d^2y}{dt^2} \) représente l’**inertie**.

C’est exactement la structure :

- d’un pendule,
- d’un circuit RLC,
- d’un système asservi.

---

## 12.3 Origine physique profonde du second ordre

### 12.3.1 Deux stockages d’énergie indépendants

Un second ordre implique **deux formes d’énergie stockée** :

- électrique + magnétique (RLC),
- potentielle + cinétique (mécanique),
- intégration double (automatique).

Cette dualité permet :

- un échange d’énergie,
- donc une oscillation.

---

### 12.3.2 Pourquoi le premier ordre ne peut pas osciller

Avec un seul stockage :

- l’énergie ne peut que décroître,
- aucun va-et-vient n’est possible.

L’oscillation n’est pas un artefact mathématique :
c’est une **signature énergétique**.

---

## 12.4 Lecture mécanique équivalente (image mentale clé)

### 12.4.1 Analogie masse–ressort–amortisseur

| Électronique | Mécanique |
| --- | --- |
| Inductance \( L \) | Masse |
| Condensateur \( C \) | Ressort |
| Résistance \( R \) | Amortisseur |

Cette analogie permet :

- d’intuitionner immédiatement le comportement,
- même sans écrire une seule équation.

---

### 12.4.2 Image mentale durable

Un système du second ordre est un objet qui :

- peut dépasser sa position d’équilibre,
- revenir,
- osciller,
- puis se stabiliser.

Tout dépend de **l’amortissement**.

---

## 12.5 Rôle fondamental de l’amortissement

### 12.5.1 Coefficient d’amortissement \( \zeta \)

Le paramètre \( \zeta \) compare :

- les pertes,
- à la capacité d’oscillation naturelle.

Il contrôle **la forme entière de la réponse**.

---

### 12.5.2 Régimes temporels

| Régime | Condition | Comportement |
| --- | --- | --- |
| Sur-amorti | \( \zeta > 1 \) | Lent, sans oscillation |
| Critique | \( \zeta = 1 \) | Retour le plus rapide |
| Sous-amorti | \( \zeta < 1 \) | Oscillations amorties |

---

## 12.6 Réponses temporelles typiques

### 12.6.1 Sous-amorti

- dépassement,
- oscillations,
- temps d’établissement long si \( Q \) élevé.

Très fréquent en électronique.

---

### 12.6.2 Amortissement critique

- aucun dépassement,
- rapidité maximale,
- souvent recherché en asservissement.

---

### 12.6.3 Sur-amorti

- très robuste,
- mais lent,
- peu utilisé pour filtrer.

---

## 12.7 Lecture fréquentielle unifiée

### 12.7.1 Fonction de transfert type

La fonction de transfert s’écrit :

$$
H(j\omega)
= \frac{K \omega_0^2}
{(j\omega)^2 + 2\zeta\omega_0 j\omega + \omega_0^2}
$$

Le dénominateur est un **polynôme du second degré**.

---

### 12.7.2 Pôles et dynamique

- deux pôles complexes conjugués si \( \zeta < 1 \),
- deux pôles réels si \( \zeta \ge 1 \).

Les pôles contiennent **toute l’information dynamique**.

---

## 12.8 Résonance et facteur de qualité

### 12.8.1 Apparition de la résonance

Si l’amortissement est faible :

- le système amplifie une bande étroite de fréquences,
- un pic apparaît sur le module du Bode.

C’est la **résonance**.

---

### 12.8.2 Facteur de qualité \( Q \)

On définit :

$$
Q = \frac{1}{2\zeta}
$$

Le facteur de qualité mesure :

- la finesse de la résonance,
- la durée des oscillations,
- la sensibilité aux variations.

---

## 12.9 Compromis fondamentaux du second ordre

Augmenter \( Q \) :

- améliore la sélectivité,
- augmente le dépassement,
- réduit la robustesse.

Diminuer \( Q \) :

- stabilise,
- mais dégrade la discrimination fréquentielle.

Il n’existe **aucun réglage parfait**.

---

## 12.10 Lecture ingénieur avant tout calcul

Face à un schéma ou un système :

1. y a-t-il deux stockages d’énergie ?
2. le système peut-il osciller ?
3. le niveau d’amortissement est-il acceptable ?
4. le risque de résonance est-il maîtrisé ?

Ces questions précèdent toute équation.

---

## 12.11 Pourquoi ce chapitre est central

Ce chapitre permet de :

- comprendre RLC, AOP, asservissements,
- lire un Bode sans calcul,
- anticiper les risques d’instabilité,
- faire le lien entre domaines physiques.

C’est l’un des chapitres
les plus structurants du cours.

---

## 12.12 Transition naturelle

Maintenant que la vision unifiée du second ordre est claire,
il devient naturel de :

- refaire des **applications numériques complètes**,
- relire les montages RLC sous cet angle,
- comprendre les modèles d’AOP comme des seconds ordres déguisés.

👉 Le chapitre suivant pourra donc être :
**Applications numériques complètes du second ordre**
ou
**Lien avec AOP et systèmes asservis**.
