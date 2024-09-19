<template>
  <div class="hand">
    <CardComponent v-for="card in playerHand" :key="card.cardId" :card="card" />
  </div>
</template>

<script>
import CardComponent from './CardComponent.vue';

export default {
  props: {
    playerHand: Array
  },
  components: {
    CardComponent
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