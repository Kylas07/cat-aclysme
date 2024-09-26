<template>
  <div>
    <AuthPage v-if="!isGameStarted" @start-game="launchGame" />
    <GameBoard v-if="isGameStarted" />
  </div>
</template>

<script>
import AuthPage from './components/AuthPage.vue';
import GameBoard from './components/GameBoard.vue';

export default {
  data() {
    return {
      isGameStarted: false,
      player1Pseudo: '', // Ajoutez cette ligne pour stocker le pseudo du joueur 1
      player2Pseudo: ''  // Ajoutez cette ligne pour stocker le pseudo du joueur 2
    };
  },
  methods: {
    launchGame() {
      this.isGameStarted = true;  // Passe à true lorsque l'événement est reçu
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
