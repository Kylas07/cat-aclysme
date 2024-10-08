

## **Documentation front-end**

### **1. GameBoard.vue**

Le composant principal qui gère l'affichage et la logique du jeu. Il affiche et coordonne les différents composants du plateau de jeu, la main du joueur, et les informations de la partie.

#### **Props :**

- **cardsOnBoard** (Array) : Liste des cartes actuellement jouées sur le plateau.
- **playerHand** (Array) : Liste des cartes présentes dans la main du joueur actif.
- **currentTurn** (Number) : Le numéro du tour en cours.
- **player1HP** (Number) : Points de vie du joueur 1.
- **player2HP** (Number) : Points de vie du joueur 2.

#### **Composants Utilisés :**

- **PlayerHand** : Affiche les cartes dans la main du joueur actif.
- **PlayerAgainstHand** : Affiche le dos des cartes de la main de l'adversaire.
- **CardsOnBoard** : Affiche les cartes jouées sur le plateau.
- **GameInfo** : Affiche les informations de la partie, telles que le tour en cours et les points de vie des joueurs.
- **PlayerDeck** : Affiche le nombre de cartes restantes dans le deck des joueurs.

#### **Méthodes :**

- **handleCardDrop(card)** : Gère l'événement où une carte est jouée depuis la main vers le plateau.
  - **Paramètre :**
    - `card` : La carte jouée par le joueur.
  - **Logique :**
    - Vérifie que c'est bien le tour du joueur avant de permettre de déposer une carte.
    - Déplace la carte de la main vers le plateau de jeu (mise à jour de `cardsOnBoard` et suppression de `playerHand`).
    - Si une pose par tour est autorisée, la méthode **nextTurn** est appelée.

- **nextTurn()** : Change de tour, en alternant entre les deux joueurs (joueur 1 et joueur 2).

#### **Exemple :**

```vue
<GameBoard
  :cardsOnBoard="cardsOnBoard"
  :playerHand="playerHand"
  :currentTurn="currentTurn"
  :player1HP="player1HP"
  :player2HP="player2HP"
  @card-dropped="handleCardDrop"
/>
```

---

### **2. PlayerHand.vue**

Composant qui affiche les cartes dans la main du joueur. Chaque carte est rendue via le composant **CardComponent**.

#### **Props :**

- **playerHand** (Array) : Un tableau contenant les cartes présentes dans la main du joueur actif.

#### **Composants Utilisés :**

- **CardComponent** : Composant qui affiche chaque carte.

#### **Exemple :**

```vue
<PlayerHand :playerHand="playerHand" />
```

---

### **3. PlayerAgainstHand.vue**

Affiche le dos des cartes de la main de l'adversaire.

#### **Props :**

- **opponentHandSize** (Number) : Le nombre de cartes dans la main de l'adversaire.

#### **Exemple :**

```vue
<PlayerAgainstHand :opponentHandSize="5" />
```

---

### **4. CardsOnBoard.vue**

Affiche les cartes qui ont été jouées sur le plateau. Les cartes peuvent être déposées via le drag-and-drop et seulement sur les cases appartenant au joueur actif selon les règles du jeu cf [lien]

#### **Props :**

- **cardsOnBoard** (Array) : Liste des cartes actuellement sur le plateau.

#### **Computed Properties :**

- **boardSlots** : Calcule le nombre d'emplacements vides sur le plateau. Il crée un tableau contenant à la fois les cartes jouées et des emplacements vides si le plateau n'est pas plein.

#### **Méthodes :**

- **onDrop(event)** : Gère l'événement lorsqu'une carte est déposée sur le plateau via le drag-and-drop.
  - **Paramètre :**
    - `event` : Contient les informations sur la carte qui a été déplacée.
  - **Logique :**
    - Utilise l'événement pour extraire les données de la carte et émet l'événement `card-dropped` pour le parent (GameBoard.vue).

#### **Exemple :**

```vue
<CardsOnBoard
  :cardsOnBoard="cardsOnBoard"
  @card-dropped="handleCardDrop"
/>
```

---

### **5. GameInfo.vue**

Affiche les informations principales du jeu, comme les points de vie des joueurs et le numéro du tour actuel.

#### **Props :**

- **currentTurn** (Number) : Le tour en cours.
- **player1HP** (Number) : Points de vie du joueur 1.
- **player2HP** (Number) : Points de vie du joueur 2.

#### **Exemple :**

```vue
<GameInfo
  :currentTurn="currentTurn"
  :player1HP="player1HP"
  :player2HP="player2HP"
/>
```

---

### **6. PlayerDeck.vue**

Affiche les informations relatives au deck du joueur, incluant le nombre de cartes restantes et une image représentant le deck.

#### **Props :**

- **cardsLeft** (Number) : Nombre de cartes restantes dans le deck du joueur.
- **deckImage** (String) : Image du deck. Par défaut, une image est utilisée si aucune n'est fournie.

#### **Exemple :**

```vue
<PlayerDeck :cardsLeft="player1DeckSize" />
```

---

### **7. CardComponent.vue**

Composant qui affiche les détails d'une carte individuelle. Utilisé dans la main du joueur ou sur le plateau.

#### **Props :**

- **card** (Object) : Un objet contenant les informations sur la carte (nom, points de vie, attaque, image, description).

#### **Méthodes :**

- **onDragStart(event)** : Permet à la carte d'être déplacée via le drag-and-drop. Lorsqu'une carte est glissée, elle stocke ses informations dans l'événement.

#### **Exemple :**

```vue
<CardComponent :card="card" />
```

---

## **Interactions entre les composants**

- **PlayerHand.vue** : Permet au joueur de faire glisser une carte depuis sa main vers le plateau de jeu. Lorsque la carte est déposée sur le plateau (dans **CardsOnBoard.vue**), elle est retirée de la main et ajoutée au plateau.

- **CardsOnBoard.vue** : Gère l'affichage des cartes jouées sur le plateau et accepte des cartes via le drag-and-drop.

- **GameBoard.vue** : Coordonne le flux de données entre la main du joueur, le plateau de jeu et les informations sur l'état de la partie.

---

### **Améliorations futures possibles**

1. **Validation des actions :** Ajouter une validation plus rigoureuse pour vérifier que le joueur ne joue qu'à son tour et empêcher toute action si ce n'est pas le cas.

2. **Animations :** Ajouter des animations lors du drag-and-drop des cartes pour améliorer l'expérience utilisateur.

3. **Gestion de la partie côté serveur :** Actuellement, la logique de jeu est gérée localement sur le front-end. Il pourrait être utile de déporter certaines actions critiques sur le serveur pour sécuriser le déroulement du jeu (vérification des tours, validation des actions, mise à jour des points de vie, etc.).

---

### **Exemple d'initialisation d'une partie**

```vue
<GameBoard
  :cardsOnBoard="cardsOnBoard"
  :playerHand="playerHand"
  :currentTurn="currentTurn"
  :player1HP="player1HP"
  :player2HP="player2HP"
  @card-dropped="handleCardDrop"
/>
```

---





FAUT CONTINUER