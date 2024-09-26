<template>
    <div class="game-board">
      <PlayerAgainstHand :opponentHandSize="opponentHandSize" />
    
      <GameInfo 
        :gameId="gameId"
        :currentTurn="currentTurn" 
        :player1HP="player1HP" 
        :player2HP="player2HP" 
      />
      <div class="game-decks">
        <PlayerDeck :cardsLeft="player2DeckSize" />
        <CardsOnBoard :cardsOnBoard="cardsOnBoard" 
        @card-dropped="handleCardDrop"
        />
        <PlayerDeck :cardsLeft="player1DeckSize" />
      </div>
      <PlayerHand 
      :playerHand="playerHand" 
      :isPlayerTurn="currentPlayerTurn === 1"
      @card-dropped="handleCardDrop"
      />
  </div>
  </template>
  
  <script>
import PlayerHand from './PlayerHand.vue';
import PlayerAgainstHand from './PlayerAgainstHand.vue';
import CardsOnBoard from './CardsOnBoard.vue';
import GameInfo from './GameInfo.vue';
import PlayerDeck from './PlayerDeck.vue';
  
export default {
  props: {
    gameId: Number,
    currentTurn: Number,
    player1HP: Number,
    player2HP: Number
  },
  data() {
    return {
      // Main du joueur 1 (cartes fictives pour test)
      playerHand: [
        {
          cardId: 1,
          name: "Catnado",
          health: 10,
          attack: 7,
          player: 1,
          image: require('@/assets/Catnado.png'),
          description: "Meow."
        },
        {
          cardId: 2,
          name: "Catnado",
          health: 8,
          attack: 5,
          player: 1,
          image: require('@/assets/Catnado.png'),
          description: "Meow."
        },
        {
          cardId: 3,
          name: "Catnado",
          health: 6,
          attack: 9,
          player: 1,
          image: require('@/assets/Catnado.png'),
          description: "Meow."
        }
      ],
      // Cartes sur le plateau de jeu
      cardsOnBoard: [
        {
          cardId: 4,
          name: "Catnado",
          health: 15,
          attack: 4,
          image: require('@/assets/Catnado.png'),
          description: "Meow."
        },
        {
          cardId: 5,
          name: "Catnado",
          health: 5,
          attack: 6,
          image: require('@/assets/Catnado.png'),
          description: "Meow."
        }
      ],
      opponentHandSize: 5, // Nombre de cartes dans la main de l'adversaire
      player1DeckSize: 15, // Cartes restantes dans le deck du joueur 1
      player2DeckSize: 18 // Cartes restantes dans le deck du joueur 2
    };
  },
  components: {
    PlayerHand,
    PlayerAgainstHand,
    CardsOnBoard,
    GameInfo,
    PlayerDeck
  },
  computed: {
    isPlayerTurn() {
      // Si c'est le tour du joueur 1 et que ce composant est le joueur 1, il peut jouer
      // Sinon, si c'est le tour du joueur 2 et que c'est le joueur 2, il peut jouer
      return (this.currentTurn === 1 && this.isPlayer1) || (this.currentTurn === 2 && !this.isPlayer1);
    }
  },
  methods: {
  handleCardDrop({ card, index }) {
    if (this.isPlayerTurn) {
      // Mets à jour directement l'élément dans cardsOnBoard
      this.cardsOnBoard[index] = card;
      // Retire la carte de la main du joueur
      this.playerHand = this.playerHand.filter(c => c.cardId !== card.cardId);
    } else {
      console.log("Ce n'est pas votre tour !");
    }
  },
  nextTurn() {
    this.$emit('update-turn');
  }
}
}
</script>

  
  <style scoped>
  .game-board {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  .game-decks {
    display: flex;
    align-items: center;
    gap: 100px;
  }
  </style>
  