<template>
  <div>
    <AuthPage v-if="!isGameStarted" @start-game="launchGame" />
    <GameBoard v-if="isGameStarted" 
      :gameId="gameId" 
      :currentTurn="currentTurn" 
      :player1HP="player1HP" 
      :player2HP="player2HP"
      :player1Id="player1Id"
      :player2Id="player2Id"
      :currentPlayerId="currentPlayerId"
      @update-turn="updateTurn"
    />
  </div>
</template>

<script>
import AuthPage from './components/AuthPage.vue';
import GameBoard from './components/GameBoard.vue';

export default {
  data() {
    return {
      isGameStarted: false,
      gameId: null,
      currentTurn: null,  // Tour actuel (ID du joueur qui joue)
      player1HP: null,
      player2HP: null,
      player1Id: null,    // ID du joueur 1
      player2Id: null,    // ID du joueur 2
      currentPlayerId: null  // ID du joueur en cours de jeu
    };
  },
  methods: {
    async launchGame(gameId, currentTurn, player1HP, player2HP, player1Id, player2Id, turnCount) {
    this.isGameStarted = true;
    this.gameId = gameId;
    this.currentTurn = currentTurn;
    this.player1HP = player1HP;
    this.player2HP = player2HP;
    this.player1Id = player1Id;
    this.player2Id = player2Id;
    this.turnCount = turnCount;
    this.currentPlayerId = currentTurn;
  },
  //   async initializeDeck(playerId) {
  //   console.log("ça lance la méthode");
  //   try {
  //     const response = await fetch('https://localhost:7111/api/game/initialize-deck', {
  //       method: 'POST',
  //       headers: {
  //         'Content-Type': 'application/json',
  //       },
  //       body: JSON.stringify({
  //         playerId: playerId,
  //       }),
  //     });

  //     if (!response.ok) {
  //       const errorData = await response.json();
  //       throw new Error(`Erreur ${response.status}: ${errorData.message}`);
  //     }

  //     const data = await response.json();
  //     console.log(`Deck du joueur ${playerId} initialisé avec succès:`, data.deck);

  //   } catch (error) {
  //     console.error(`Erreur lors de l'initialisation du deck pour le joueur ${playerId}:`, error.message);
  //   }
  // },
  //   async drawInitialCards(playerId) {
  //   try {
  //     const response = await fetch('https://localhost:7111/api/game/draw-initial-cards', {
  //       method: 'POST',
  //       headers: {
  //         'Content-Type': 'application/json',
  //       },
  //       body: JSON.stringify({
  //         gameId: this.gameId,
  //         playerId: playerId,
  //       }),
  //     });

  //     if (!response.ok) {
  //       const errorData = await response.json();
  //       throw new Error(`Erreur ${response.status}: ${errorData.message}`);
  //     }

  //     const data = await response.json();
  //     console.log(`5 cartes piochées pour le joueur ${playerId}:`, data);

  //     // Mettre à jour la main du joueur
  //     if (playerId === this.player1Id) {
  //       this.playerHand = data.cards;  // Assurez-vous que l'API renvoie les cartes dans `data.cards`
  //     }

  //   } catch (error) {
  //     console.error(`Erreur lors de la pioche des cartes pour le joueur ${playerId}:`, error.message);
  //   }
  // },
    updateTurn() {
      // Inverser le tour entre le joueur 1 et le joueur 2
      this.currentPlayerId = this.currentPlayerId === this.player1Id ? this.player2Id : this.player1Id;
      this.currentTurn = this.currentPlayerId;
      console.log("Changement de tour, on est le ", this.currentTurn , " c'est au tour de", this.currentPlayerId)
    }
  },
  components: {
    AuthPage,
    GameBoard
  }
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
