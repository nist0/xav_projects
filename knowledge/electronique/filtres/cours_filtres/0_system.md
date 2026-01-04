# Chapitre 0 — Qu’est-ce qu’un système ?

## 0.1 Pourquoi commencer par cette question

Avant de parler de filtres, d’équations différentielles ou de diagrammes de Bode,
il est indispensable de répondre à une question plus fondamentale :

**qu’est-ce qu’un système, au sens physique et ingénierie du terme ?**

Beaucoup de difficultés rencontrées en électronique ne viennent pas d’un manque de calcul,
mais d’une compréhension incomplète de ce que l’on modélise réellement.
Ce chapitre pose donc le vocabulaire, les concepts et les limites
qui serviront de cadre à tout le cours.

---

## 0.2 Définition générale d’un système

### 0.2.1 Définition formelle

Un **système** est un objet (physique ou abstrait) qui :

- reçoit une ou plusieurs **entrées**,
- produit une ou plusieurs **sorties**,
- selon des lois internes déterminées.

On peut représenter cette idée de manière abstraite par :

- une entrée \( x(t) \),
- une sortie \( y(t) \),
- une relation (explicite ou implicite) entre les deux.

Cette relation peut être :

- algébrique,
- différentielle,
- intégrale,
- ou une combinaison de ces formes.

---

### 0.2.2 Interprétation physique

Physiquement, un système est un **intermédiaire** entre une cause et un effet.

Il ne crée pas l’information :

- il la transforme,
- la retarde,
- l’atténue,
- l’amplifie,
- ou la déforme.

Un système n’est jamais instantané par nature :
il a toujours des **limitations physiques**,
même si elles sont parfois négligeables.

---

## 0.3 Entrée, sortie et causalité

### 0.3.1 Entrée et sortie

- L’**entrée** est ce que l’on impose au système.
- La **sortie** est ce que l’on observe.

En électronique :

- l’entrée est souvent une tension ou un courant,
- la sortie est également une tension ou un courant,

mais leur relation dépend entièrement du système.

---

### 0.3.2 Notion de causalité

Un système est dit **causal** si :

> la sortie à un instant donné dépend uniquement
> des entrées passées et présentes,
> jamais des entrées futures.

Tous les systèmes physiques réels sont causaux.

Cette contrainte est fondamentale :
elle impose des limites fortes sur ce qu’un système peut faire,
notamment en termes de filtrage et de prédiction.

---

## 0.4 Notion d’état et de mémoire

### 0.4.1 État d’un système

L’**état** d’un système est l’ensemble minimal d’informations
nécessaires pour décrire son comportement futur,
connaissant les entrées.

Mathématiquement, l’état est souvent représenté
par une ou plusieurs variables internes.

---

### 0.4.2 Mémoire d’un système

Un système possède une **mémoire** si sa sortie dépend
non seulement de l’entrée instantanée,
mais aussi de ce qui s’est produit auparavant.

En électronique :

- un condensateur “se souvient” de sa tension,
- une inductance “se souvient” de son courant.

Cette mémoire est toujours liée à un **stockage d’énergie**.

---

### 0.4.3 Interprétation concrète

Un système sans mémoire réagit immédiatement,
comme un interrupteur idéal.

Un système avec mémoire réagit avec un délai,
exactement comme un objet ayant de l’inertie.

Cette inertie est au cœur des filtres.

---

## 0.5 Modélisation des systèmes physiques

### 0.5.1 Pourquoi modéliser

La réalité est trop complexe pour être manipulée directement.
La modélisation permet de :

- prédire le comportement,
- concevoir des systèmes,
- comprendre les limites,
- comparer des solutions.

Un modèle est toujours une **approximation contrôlée**.

---

### 0.5.2 Modèles idéaux

En électronique, on utilise souvent :

- résistances idéales,
- condensateurs idéaux,
- inductances idéales.

Ces modèles supposent :

- absence de pertes parasites,
- linéarité parfaite,
- comportement invariant dans le temps.

Ils permettent une première compréhension,
mais ne décrivent jamais complètement le réel.

---

### 0.5.3 Modèles réels

Les composants réels présentent :

- des tolérances,
- des pertes,
- des comportements non idéaux à haute fréquence,
- des limites thermiques et physiques.

La modélisation doit donc évoluer
au fur et à mesure que l’on s’approche du réel.

---

## 0.6 Systèmes linéaires et non linéaires

### 0.6.1 Linéarité

Un système est **linéaire** s’il vérifie :

- le principe de superposition,
- l’homogénéité.

Autrement dit :

- la réponse à une somme est la somme des réponses,
- la réponse à un signal amplifié est amplifiée proportionnellement.

Les filtres étudiés dans ce cours sont,
dans un premier temps,
considérés comme linéaires.

---

### 0.6.2 Non-linéarité

Les non-linéarités apparaissent lorsque :

- les composants saturent,
- les hypothèses idéales ne sont plus valables,
- les amplitudes deviennent trop grandes.

Ces effets seront abordés plus tard,
lorsque les bases linéaires seront solides.

---

## 0.7 Systèmes dynamiques et dépendance temporelle

### 0.7.1 Dépendance au temps

Un système est **dynamique**
lorsque sa sortie dépend de l’évolution temporelle de l’entrée.

Les systèmes dynamiques sont décrits
par des équations différentielles.

---

### 0.7.2 Ordre du système (introduction)

L’**ordre** d’un système correspond :

- au nombre de variables d’état indépendantes,
- donc au nombre de stockages d’énergie indépendants.

Cette notion sera approfondie au chapitre suivant,
car elle détermine profondément le comportement du système.

---

## 0.8 Ce que l’ingénieur doit toujours garder à l’esprit

Avant même de calculer, un ingénieur doit se poser les questions suivantes :

- le système est-il causal ?
- possède-t-il une mémoire ?
- combien d’énergies sont stockées ?
- quelles sont les limites physiques ?
- le modèle utilisé est-il adapté au phénomène étudié ?

Ces questions guident le choix :

- des modèles,
- des équations,
- des outils d’analyse (temps ou fréquence).

---

## 0.9 Transition vers la notion d’ordre

Tout ce qui a été introduit dans ce chapitre
conduit naturellement à la question suivante :

> **comment classer les systèmes selon leur complexité dynamique ?**

Cette classification repose sur une notion clé :
**l’ordre du système**.

Le chapitre suivant répondra à cette question
en montrant que l’ordre n’est pas qu’un concept mathématique,
mais une signature physique profonde du comportement du système.
