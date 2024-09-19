<template>
    <div class="game-board">
      <PlayerDeck :cardsLeft="player2DeckSize" />
      <PlayerAgainstHand :opponentHandSize="opponentHandSize" />
    
      <GameInfo 
        :currentTurn="currentTurn" 
        :player1HP="player1HP" 
        :player2HP="player2HP" 
      />
      <CardsOnBoard :cardsOnBoard="cardsOnBoard" 
      @card-dropped="handleCardDrop"
      />
      
      <PlayerHand :playerHand="playerHand" 
      @card-dropped="handleCardDrop"
      />
      <PlayerDeck :cardsLeft="player1DeckSize" />
  </div>
  </template>
  
  <script>
import PlayerHand from './PlayerHand.vue';
import PlayerAgainstHand from './PlayerAgainstHand.vue';
import CardsOnBoard from './CardsOnBoard.vue';
import GameInfo from './GameInfo.vue';
import PlayerDeck from './PlayerDeck.vue';
  
export default {
  data() {
    return {
      currentTurn: 1,
      player1HP: 100,
      player2HP: 100,
      // Main du joueur 1 (cartes fictives pour test)
      playerHand: [
        {
          cardId: 1,
          name: "Catnado",
          health: 10,
          attack: 7,
          image: require('@/assets/Catnado.png'),
          description: "A powerful dragon slayer."
        },
        {
          cardId: 2,
          name: "Catnado",
          health: 8,
          attack: 5,
          image: require('@/assets/Catnado.png'),
          description: "Meow."
        },
        {
          cardId: 3,
          name: "Catnado",
          health: 6,
          attack: 9,
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
  methods: {
    handleCardDrop(card) {
      this.cardsOnBoard.push(card);
      this.playerHand = this.playerHand.filter(c => c.cardId !== card.cardId);
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
  </style>
  