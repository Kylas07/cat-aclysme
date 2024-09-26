<template>
  <div>
    <AuthPage v-if="!isGameStarted" @start-game="launchGame" />
    <GameBoard v-if="isGameStarted" 
      :gameId="gameId" 
      :currentTurn="currentTurn" 
      :player1HP="player1HP" 
      :player2HP="player2HP" />
  </div>
</template>

<script>
import AuthPage from './components/AuthPage.vue';
import GameBoard from './components/GameBoard.vue';

export default {
  data() {
    return {
      isGameStarted: false,
      gameId: null,       // Ajouter la variable pour stocker l'ID de la partie
      currentTurn: null,  // Ajouter la variable pour stocker le tour actuel
      player1HP: null,    // Ajouter la variable pour stocker les HP du joueur 1
      player2HP: null     // Ajouter la variable pour stocker les HP du joueur 2
    };
  },
  methods: {
    launchGame(gameId, currentTurn, player1HP, player2HP, turnCount) 
    {
      // Mettre à jour le state avec les informations reçues de l'API
      this.isGameStarted = true;
      this.gameId = gameId;
      this.currentTurn = currentTurn;
      this.player1HP = player1HP;
      this.player2HP = player2HP;
      this.turnCount = turnCount;
    },
    updateTurn() 
    {
    // Inverser le tour : passer au joueur 2 si c'est le joueur 1, ou vice versa
    this.currentTurn = this.currentTurn === 1 ? 2 : 1;
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
