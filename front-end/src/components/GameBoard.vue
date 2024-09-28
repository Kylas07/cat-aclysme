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
      <!-- Deck et main du joueur 2 -->
      <PlayerDeck :cardsLeft="player2DeckSize" />
      <CardsOnBoard 
        :cardsOnBoard="cardsOnBoard" 
        :gameId="gameId"
        @card-dropped="handleCardDrop"
      />
      <PlayerDeck :cardsLeft="player1DeckSize" />
    </div>

    <!-- Main du joueur 1 -->
        <PlayerHand 
      :playerHand="player1Hand" 
      :isPlayerTurn="currentPlayerTurn === 1"
      @card-dropped="handleCardDrop"
    />

    <PlayerHand 
      :playerHand="player2Hand" 
      :isPlayerTurn="currentPlayerTurn === 2"
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
    player2HP: Number,
    player1Id: Number,
    player2Id: Number,
    currentPlayerId: Number
  },
  data() {
    return {
      player1Hand: [], // Cartes en main du joueur 1
      player2Hand: [], // Cartes en main du joueur 2
      cardsOnBoard: [], // Cartes sur le plateau de jeu
      opponentHandSize: 5, // Nombre de cartes dans la main de l'adversaire
      player1DeckSize: 0, // Cartes restantes dans le deck du joueur 1
      player2DeckSize: 0 // Cartes restantes dans le deck du joueur 2
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
      async startGame() {
    try {
      const requestBody = {
        Player1Pseudo: this.player1.name,
        Player2Pseudo: this.player2.name
      };
      
      const response = await fetch('https://localhost:7111/api/game/start', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
      });
      
      if (response.ok) {
        const data = await response.json();
        const game = data.game;

        // Cartes du joueur 1
        const player1Deck = data.player1GameDeck;
        const player1Hand = player1Deck.filter((card, index) => index < 5); // Les 5 premières cartes pour la main
        const player1DeckLeft = player1Deck.filter((card, index) => index >= 5); // Le reste des cartes pour le deck

        // Cartes du joueur 2
        const player2Deck = data.player2GameDeck;
        const player2Hand = player2Deck.filter((card, index) => index < 5); // Les 5 premières cartes pour la main
        const player2DeckLeft = player2Deck.filter((card, index) => index >= 5); // Le reste des cartes pour le deck

        // Mise à jour des decks et des mains
        this.player1Hand = player1Hand; // Main de 5 cartes
        this.player2Hand = player2Hand; // Main de 5 cartes

        this.player1DeckSize = player1DeckLeft.length; // Cartes restantes dans le deck
        this.player2DeckSize = player2DeckLeft.length; // Cartes restantes dans le deck

        this.$emit('start-game', game.gameId, game.playerTurn, game.player1HP, game.player2HP, game.player.playerId, game.player_1.playerId, game.turnCount);
      } else {
        const errorData = await response.json();
        alert(`Erreur lors du lancement de la partie: ${errorData.message}`);
      }
    } catch (error) {
      alert('Erreur lors de la communication avec le serveur');
    }
  },

  }
};
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
