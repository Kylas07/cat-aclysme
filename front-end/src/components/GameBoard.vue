<template>
    <div class="game-board">
      <PlayerDeck :cardsLeft="player2DeckSize" />
      <PlayerAgainstHand :opponentHandSize="opponentHandSize" />
    
      <GameInfo 
        :currentTurn="currentTurn" 
        :player1HP="player1HP" 
        :player2HP="player2HP" 
      />
      <CardsOnBoard :cardsOnBoard="cardsOnBoard" />
      
      <PlayerHand :playerHand="playerHand" />
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
          name: "Dragon Slayer",
          health: 10,
          attack: 7,
          image: require('@/assets/Catnado.png'),
          description: "A powerful dragon slayer."
        },
        {
          cardId: 2,
          name: "Forest Guardian",
          health: 8,
          attack: 5,
          image: require('@/assets/Catnado.png'),
          description: "Protector of the enchanted forest."
        },
        {
          cardId: 3,
          name: "Fire Elemental",
          health: 6,
          attack: 9,
          image: require('@/assets/Catnado.png'),
          description: "A fierce elemental of fire."
        }
      ],
      // Cartes sur le plateau de jeu
      cardsOnBoard: [
        {
          cardId: 4,
          name: "Mountain Giant",
          health: 15,
          attack: 4,
          image: require('@/assets/Catnado.png'),
          description: "A giant that dominates the battlefield."
        },
        {
          cardId: 5,
          name: "Water Nymph",
          health: 5,
          attack: 6,
          image: require('@/assets/Catnado.png'),
          description: "A mystical nymph with control over water."
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
  