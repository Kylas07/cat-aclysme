<template>
  <div class="hand" v-bind:style="[currentPlayerTurn === player1Id ? stylePlayer1 : stylePlayer2]">
    <CardComponent v-for="card in playerHand" 
    :key="card.cardId" 
    :card="card"
    :gameId="gameId"
    :isOnBoard="false"
     />
  </div>
</template>

<script>
import CardComponent from './CardComponent.vue';

export default {
  props: {
    playerHand: Array,
    currentPlayerTurn: Number,
    player1Id: Number,
    player2Id: Number,
    gameId: { 
      type: Number,
      required: true
    }
  },
  components: {
    CardComponent
  },
  data() {
    return {
      stylePlayer1: {
        border: '5px solid red'
      },
      stylePlayer2: {
        border: '5px solid blue'
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