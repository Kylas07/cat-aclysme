<template>
  <div class="hand">
    <CardComponent v-for="card in playerHand" :key="card.cardId" :card="card"
    v-bind:style="[currentPlayerTurn === player1Id ? stylePlayer1 : stylePlayer2]" />
  </div>
</template>

<script>
import CardComponent from './CardComponent.vue';

export default {
  props: {
    playerHand: Array,
    currentPlayerTurn: Number,
    player1Id: Number,
    player2Id: Number
  },
  components: {
    CardComponent
  },
  data() {
    return {
      stylePlayer1: {
        border: '5px solid blue'
      },
      stylePlayer2: {
        border: '5px solid red'
      }
    }
  }
}
</script>

<style scoped>
.hand {
  display: flex;
  justify-content: center;
  gap: 10px;
}
</style>