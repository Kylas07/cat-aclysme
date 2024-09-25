<template>
  <div class="game-info">
    <p>Turn: {{ currentTurn }}</p>
    <p>Player 1 : {{ player1HP }} ❤️</p>
    <p>Player 2 : {{ player2HP }} ❤️</p>
  </div>
</template>

<script>
export default {
  props: {
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
