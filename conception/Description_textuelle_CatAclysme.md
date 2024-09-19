# **Cat-Aclysme** - Documentation de l'Application

## 🐾 **Description Générale**

**Cat-Aclysme** est un jeu de cartes stratégique en 1v1, développé avec **C#** pour la logique de jeu et **Vue.js** pour l'interface utilisateur. Dans un univers post-apocalyptique peuplé de chats armés, deux joueurs s'affrontent en utilisant des cartes représentant des chats guerriers, chacune avec ses propres caractéristiques d'attaque et de défense.

L'objectif du jeu est simple : réduire les points de vie de l'adversaire à zéro en utilisant des cartes stratégiquement placées sur un plateau à quatre cases.

## 🎮 **Caractéristiques Principales**

- **Système de jeu** : Chaque joueur dispose de 30 cartes dans son deck et commence la partie avec 300 points de vie (HP). Le joueur qui perd tous ses points de vie est éliminé.
  
- **Cartes** : Le jeu propose **30 cartes uniques**, chacune avec ses propres points de vie (HP), points d'attaque (ATK), et une illustration personnalisée dans un style doodle amusant.

- **Plateau de jeu** : Les cartes peuvent être placées sur l'une des 4 cases devant chaque joueur. Les cartes attaquent directement les cartes en face ou le joueur si aucune défense n'est présente.

- **Mécanismes simples** : Chaque carte posée sur le plateau ne peut attaquer qu'à partir du tour suivant. Le joueur pioche une carte à chaque tour et doit choisir judicieusement la carte à jouer pour défendre ou attaquer.

## 🔧 **Technologies Utilisées**

- **Backend** : Le cœur du jeu est développé en **C#**, gérant la logique de jeu, les règles, les attaques, et les interactions entre les cartes.
- **Frontend** : L'interface utilisateur est construite en **Vue.js**, offrant une expérience fluide et interactive pour jouer au jeu.
- **Base de Données** : Utilisation de **SQL Server** pour stocker les données des joueurs, des decks, des cartes et des parties.

## 💾 **Base de Données**

La base de données gère les informations essentielles du jeu, incluant :
- **Player** : Informations sur les joueurs (nom, points de vie, tours de jeu).
- **Deck** : Cartes possédées par chaque joueur dans son deck.
- **Cards** : Les caractéristiques de chaque carte (nom, points de vie, attaque, illustration, description).
- **Game** : Les informations sur chaque partie (joueurs, scores, etc.).

## 📈 **Fonctionnalités en Cours de Développement**

- **Système de combos** : Intégrer des effets spéciaux pour certaines combinaisons de cartes.
- **Mode multijoueur en ligne** : Permettre aux joueurs de s'affronter en temps réel via une interface en ligne.
- **Animations** : Ajouter des animations lors des attaques pour rendre les combats plus dynamiques.