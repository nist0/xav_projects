# Organigramme : Articulation SIGB / UNIMARC / Dewey / RAMEAU

Ce schéma te montre clairement comment les quatre éléments se relient entre eux lors du catalogage et du fonctionnement d’une bibliothèque.

---

## 🗺️ Schéma général (version texte + Mermaid)

```mermaid
graph TD;
    A[SIGB<br>(Système Intégré de Gestion de Bibliothèque)] --> B[Notice UNIMARC<br>(Format de catalogage)]
    B --> C[Dewey<br>(Classification / Cote)\nZone 686]
    B --> D[RAMEAU<br>(Indexation matière)\nZone 606]
    C --> E[Mise en rayon<br>Classement physique]
    D --> F[Recherche thématique<br>OPAC / Catalogue]
```

---

## 🧱 Détail des relations

### 1. **SIGB**

* Le logiciel central : Koha, PMB, Orphée...
* Il contient toutes les notices.
* C’est lui qui permet d’utiliser les formats et outils suivants.

### 2. **UNIMARC** (dans le SIGB)

* Structure officielle de la notice.
* Zones principales : 200 (titre), 210 (éditeur), 330 (résumé), **606 (RAMEAU)**, **686 (Dewey)**.
* Sert de base pour intégrer RAMEAU et Dewey.

### 3. **RAMEAU** (zone 606)

* Vocabulaire contrôlé des sujets.
* Décrit le contenu intellectuel.
* Permet la recherche thématique.

### 4. **Dewey** (zone 686)

* Classification décimale.
* Sert à construire la cote.
* Permet le rangement logique en rayon.

---

## ✅ Comment utiliser ce document

- Pour une **vue globale rapide**, commence par le schéma Mermaid.
- Pour les **détails concrets**, relis ensuite `rapport_entre_notions.md` dans le même dossier.
- Tu peux aussi t’en servir comme support visuel en entretien (schéma que tu expliques à l’oral).
