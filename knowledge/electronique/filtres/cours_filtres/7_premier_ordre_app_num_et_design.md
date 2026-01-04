# Chapitre 7 — Applications numériques et design de filtres du premier ordre

## 7.1 Objectif du chapitre

Ce chapitre a pour objectif de transformer :

- une spécification fonctionnelle,
- en un montage concret,
- avec des composants réels,
- vérifié à la fois dans le temps et en fréquence.

Il s’agit ici de faire le lien définitif entre :
équations ⇄ diagrammes de Bode ⇄ composants ⇄ mesures.

---

## 7.2 De la spécification au modèle

### 7.2.1 Exemple de spécification typique

On se donne la spécification suivante :

- signal utile jusqu’à : 1 kHz  
- atténuation souhaitée au-delà  
- réponse stable, sans oscillation  
- simplicité et robustesse privilégiées  

Cette spécification appelle naturellement
un **filtre passe-bas du premier ordre**.

---

### 7.2.2 Choix du type de filtre

Avant toute équation, un ingénieur décide :

- premier ordre plutôt que second ordre (simplicité, stabilité),
- RC plutôt que LR (coût, encombrement, facilité).

Ce choix fixe déjà :

- la pente maximale,
- le type de retard,
- les limitations irréductibles.

---

## 7.3 Choix de la fréquence de coupure

### 7.3.1 Définition de la coupure

On choisit :

$$
f_c = 1\,\text{kHz}
\qquad
\omega_c = 2\pi f_c
$$

Ce choix est un compromis :

- assez bas pour filtrer le bruit,
- assez haut pour ne pas dégrader le signal utile.

---

### 7.3.2 Calcul de la constante de temps

On utilise la relation démontrée précédemment :

$$
\tau = \frac{1}{\omega_c} = \frac{1}{2\pi f_c}
$$

Numériquement :

$$
\tau \approx 159\,\mu\text{s}
$$

Cette valeur donne déjà une intuition temporelle :
le système réagit sur une échelle de quelques centaines de microsecondes.

---

## 7.4 Choix des composants R et C

### 7.4.1 Stratégie pratique

En pratique :

- on choisit souvent le condensateur en premier,
- puis on ajuste la résistance.

Raisons :

- valeurs normalisées,
- disponibilité,
- comportement fréquentiel du composant.

---

### 7.4.2 Choix du condensateur

Choisissons une valeur courante :

$$
C = 100\,\text{nF}
$$

Valeur :

- facile à trouver,
- stable,
- bien adaptée aux basses et moyennes fréquences.

---

### 7.4.3 Calcul de la résistance

On calcule :

$$
R = \frac{\tau}{C}
$$

Numériquement :

$$
R = \frac{159 \times 10^{-6}}{100 \times 10^{-9}}
= 1590\,\Omega
$$

Valeur normalisée la plus proche :

- \( R = 1.6\,\text{k}\Omega \)

---

## 7.5 Vérification temporelle

### 7.5.1 Réponse théorique à échelon

Pour une entrée échelon \( U_0 \) :

$$
u_s(t) = U_0\left(1 - e^{-t/\tau}\right)
$$

---

### 7.5.2 Points caractéristiques

| Temps | Valeur |
| --- | --- |
| \( t = \tau \) | 63 % |
| \( t = 3\tau \) | 95 % |
| \( t = 5\tau \) | 99 % |

Avec \( \tau = 159\,\mu\text{s} \),
le régime permanent est atteint en moins de 1 ms.

---

### 7.5.3 Lecture ingénieur

Avant même de mesurer, on sait que :

- la montée sera douce,
- il n’y aura aucun dépassement,
- le retard restera limité.

Ces prédictions doivent être confirmées à l’oscilloscope.

---

## 7.6 Vérification fréquentielle

### 7.6.1 Gain à la coupure

À \( f_c = 1\,\text{kHz} \) :

- gain ≈ −3 dB
- amplitude ≈ 0.707

---

### 7.6.2 Phase à la coupure

À la coupure :

$$
\varphi = -45^\circ
$$

Ce déphasage correspond
à un retard temporel significatif,
mais encore acceptable dans de nombreuses applications.

---

### 7.6.3 Lecture du diagramme de Bode

Sur le diagramme :

- plateau à basse fréquence,
- pente de −20 dB/décade,
- transition douce autour de \( f_c \).

Ce comportement confirme
la nature du premier ordre.

---

## 7.7 Sensibilité aux tolérances

### 7.7.1 Tolérances typiques

- résistances : ±1 % à ±5 %
- condensateurs : ±5 % à ±20 %

La constante de temps dépend du produit \( RC \),
donc les erreurs se combinent.

---

### 7.7.2 Impact sur la coupure

Une variation de ±10 % sur \( RC \)
entraîne une variation équivalente sur \( f_c \).

Pour un premier ordre,
cette sensibilité reste acceptable.

---

## 7.8 Compromis réels et limites

Un filtre du premier ordre :

- ne coupe jamais brutalement,
- introduit toujours un retard,
- ne peut pas être sélectif.

Ces limites doivent être acceptées
ou compensées par une architecture plus complexe.

---

## 7.9 Lecture ingénieur globale

À ce stade, un ingénieur doit être capable de :

- justifier chaque choix de composant,
- prédire le comportement avant montage,
- comprendre toute divergence mesure / théorie.

Le filtre n’est plus un objet abstrait :
c’est un compromis physique assumé.

---

## 7.10 Transition vers le réel non idéal

Tout ce qui a été vu suppose :

- des composants idéaux,
- des connexions parfaites,
- aucune perturbation externe.

Dans la réalité :

- les composants ont des parasites,
- les pistes ont une inductance,
- les signaux rayonnent.

Le chapitre suivant abordera
le passage du **modèle idéal**
au **composant réel**.
