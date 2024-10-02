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
      :player1Name="player1Name"
      :player2Name="player2Name"
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
      player1Name:null,
      player2Name:null,
      player1HP: null,
      player2HP: null,
      player1Id: null,    // ID du joueur 1
      player2Id: null,    // ID du joueur 2
      currentPlayerId: null  // ID du joueur en cours de jeu
    };
  },
  methods: {
    async launchGame(gameId, currentTurn, player1HP, player2HP, player1Id, player2Id, turnCount, player1Name, player2Name) {
    console.log("Démarrage du jeu avec les paramètres :", { gameId, currentTurn, player1HP, player2HP, player1Id, player2Id, player1Name, player2Name });
    this.isGameStarted = true;
    this.gameId = gameId;
    this.currentTurn = currentTurn;
    this.player1HP = player1HP;
    this.player2HP = player2HP;
    this.player1Id = player1Id;
    this.player2Id = player2Id;
    this.player1Name = player1Name;
    this.player2Name = player2Name;
    this.turnCount = turnCount;
    this.currentPlayerId = currentTurn;
  },
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
  margin: auto;
  height: 100vh;
  align-content: center;
  text-align: center;
  color: #2c3e50;
}
</style>
