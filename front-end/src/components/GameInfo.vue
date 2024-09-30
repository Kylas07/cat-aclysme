<template>
  <div class="game-info">
    <p>Game ID: {{ gameId }}</p>
    <p>Turn: {{ currentTurn }}</p>
    <p>Player 1 : {{ player1HP }} ❤️</p>
    <p>Player 2 : {{ player2HP }} ❤️</p>
    <button @click="$emit('end-turn')">Fin de tour</button>  <!-- Lever un événement -->
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
  width: 180px;
    border: 10px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    text-align: center;
    padding: 10px;
    background-color: white;
}
</style>
