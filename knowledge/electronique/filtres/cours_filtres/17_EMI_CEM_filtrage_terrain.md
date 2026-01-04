# Chapitre 17 — EMI / CEM et filtrage réel (vision ingénieur terrain)

## 17.1 Pourquoi EMI / CEM n’est pas un “à-côté”

Dans un cours théorique, les filtres servent à :

- façonner un signal,
- sélectionner une bande,
- rejeter du bruit abstrait.

Dans un système réel :

- le bruit est **omniprésent**,
- il est **créé par le système lui-même**,
- il se propage par des chemins non prévus,
- il transforme un filtre idéal en **source de problèmes**.

👉 L’EMI / CEM n’est pas une spécialité exotique :  
c’est la **suite naturelle** de la théorie des systèmes dynamiques.

---

## 17.2 Définitions claires (sans jargon inutile)

### 17.2.1 EMI — Electromagnetic Interference

L’EMI désigne :

- toute perturbation électromagnétique indésirable,
- générée par un équipement,
- subie par un autre (ou par lui-même).

---

### 17.2.2 CEM — Compatibilité électromagnétique

La CEM est la capacité d’un système à :

- fonctionner correctement dans son environnement,
- **sans perturber** les autres,
- **sans être perturbé**.

👉 EMI = phénomène  
👉 CEM = objectif de conception

---

## 17.3 Origine physique des perturbations

### 17.3.1 La vraie cause : les variations rapides

Toute EMI sérieuse provient de :

- fronts rapides,
- dérivées élevées \( \frac{dv}{dt}, \frac{di}{dt} \),
- discontinuités d’impédance.

Un système **lent** rayonne peu.  
Un système **rapide** rayonne beaucoup.

---

### 17.3.2 Sources typiques

- alimentations à découpage,
- horloges numériques,
- moteurs,
- commutations MOSFET,
- AOP saturant ou oscillant.

👉 Le bruit n’est pas un accident :  
il est **produit par la fonctionnalité même du système**.

---

## 17.4 Les trois chemins de propagation (fondamental)

Toute perturbation suit **toujours** l’un (ou plusieurs) de ces chemins :

1. **Conduction** (par les fils)
2. **Couplage capacitif** (champ électrique)
3. **Couplage inductif** (champ magnétique)

Si tu n’identifies pas le chemin,  
tu filtres **au hasard**.

---

## 17.5 Pourquoi les schémas deviennent faux à haute fréquence

### 17.5.1 Effondrement du modèle filaire

À haute fréquence :

- un fil n’est plus un fil,
- une masse n’est plus un point,
- un condensateur n’est plus capacitif.

👉 Le **schéma électrique cesse d’être suffisant**.

---

### 17.5.2 Apparition d’ordres cachés

Chaque piste ajoute :

- inductance,
- résistance,
- capacité parasite.

Un RC peut devenir :

- un RLC,
- puis un système à plusieurs résonances.

---

## 17.6 Rôle réel du filtrage en EMI

### 17.6.1 Ce que le filtrage peut faire

Un filtre réel sert à :

- ralentir les fronts,
- amortir les résonances,
- casser les pics spectraux,
- réduire le rayonnement.

---

### 17.6.2 Ce que le filtrage ne peut PAS faire

Un filtre ne peut pas :

- supprimer une mauvaise architecture,
- corriger un routage médiocre,
- compenser une boucle instable.

👉 **Le filtrage est une rustine intelligente**, pas un miracle.

---

## 17.7 Filtres EMI usuels (lecture physique)

### 17.7.1 RC d’amortissement (snubber)

- dissipe l’énergie parasite,
- réduit \( Q \),
- extrêmement robuste.

Utilisé pour :

- tuer une résonance,
- stabiliser une boucle,
- protéger un composant.

---

### 17.7.2 LC / π filters

- efficaces sur une bande ciblée,
- sensibles aux tolérances,
- peuvent **créer des résonances** s’ils sont mal amortis.

👉 Un LC sans résistance est **dangereux**.

---

### 17.7.3 Ferrites

- résistances dépendantes de la fréquence,
- très efficaces en HF,
- comportement non linéaire.

👉 Une ferrite est un **amortisseur fréquentiel**, pas une inductance.

---

## 17.8 Le PCB comme composant du filtre

### 17.8.1 Boucles de courant

La surface de boucle détermine :

- le rayonnement,
- la sensibilité au bruit.

👉 **Boucle grande = antenne**

---

### 17.8.2 Plans de masse

Un bon plan de masse :

- réduit l’inductance,
- fournit un retour de courant propre,
- améliore toutes les mesures.

Un mauvais plan :

- détruit le filtrage théorique.

---

## 17.9 Découplage : le filtre le plus mal compris

### 17.9.1 Rôle réel

Un condensateur de découplage sert à :

- fournir localement du courant rapide,
- empêcher le bruit de circuler.

Ce n’est PAS un simple “réservoir”.

---

### 17.9.2 Placement > valeur

Un mauvais placement annule :

- la valeur du condensateur,
- le calcul théorique.

👉 **1 cm de piste peut ruiner 100 nF**.

---

## 17.10 Lien avec stabilité et second ordre

Un mauvais filtrage peut :

- introduire des pôles parasites,
- réduire la marge de phase,
- rendre instable un AOP parfaitement stable sur le papier.

EMI ↔ stabilité ↔ dynamique  
👉 même combat.

---

## 17.11 Démarche ingénieur EMI / CEM

Face à un problème EMI :

1. identifier la source,
2. identifier le chemin,
3. ralentir / amortir à la source,
4. filtrer localement,
5. seulement ensuite, filtrer globalement.

👉 On **ne filtre jamais à l’aveugle**.

---

## 17.12 Ce que ce chapitre doit laisser comme réflexes

À ce stade, le lecteur doit savoir que :

- le bruit est structurel,
- le filtrage est une gestion d’énergie,
- le PCB est un composant actif,
- la CEM se pense dès la conception,
- les lois vues sur RC / RLC sont toujours valables.

---

## 17.13 Transition finale

Comprendre et concevoir ne suffit pas :
il faut **s’entraîner**, se tromper, corriger.

👉 Le chapitre suivant propose :
**tests, exercices et problèmes corrigés**
pour ancrer définitivement les concepts.
