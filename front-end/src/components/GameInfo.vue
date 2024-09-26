<template>
  <div class="game-info">
    <p>Game ID: {{ gameId }}</p>
    <p>Turn: {{ currentTurn }}</p>
    <p>Player 1 : {{ player1HP }} ❤️</p>
    <p>Player 2 : {{ player2HP }} ❤️</p>
  </div>
</template>

<script>
export default {
  props: {
    gameId: Number,
    currentTurn: Number,
    player1HP: Number,
    player2HP: Number
  }
}
</script>

<style scoped>
.game-info {
  text-align: center;
  margin-bottom: 20px;
}
</style>
