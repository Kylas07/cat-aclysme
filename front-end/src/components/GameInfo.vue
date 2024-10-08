<template>
  <div class="game-info">
    <p>Game ID: {{ gameId }}</p>
    <p>Turn: {{ turnCount }}</p>
    <p> {{ player1Name }} : {{ player1HP }} ❤️</p>
    <p>{{ player2Name }}: {{ player2HP }} ❤️</p>
    <button @click="$emit('end-turn')">Fin de tour</button>  <!-- Lever un événement -->
  </div>
</template>

<script>
export default {
  props: {
    gameId: Number,
    currentTurn: Number,
    player1HP: Number,
    player2HP: Number,
    turnCount: Number,
    player1Name: String,
    player2Name: String
    
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
