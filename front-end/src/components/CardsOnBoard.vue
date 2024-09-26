<template>
  <div class="cards-on-board">
    <div 
      v-for="(card, i) in boardSlots" 
      :key="i" 
      class="card-slot" 
      @drop="onDrop($event, i)"
      @dragover.prevent
    >
      <CardComponent v-if="card" 
      :card="card" 
      :isOnBoard="true" 
      @card-attacked="handleCardAttack(card, i)"/>
    </div>
  </div>
</template>


<script>
import CardComponent from './CardComponent.vue';

export default {
  components: {
    CardComponent
  },
  props: {
    cardsOnBoard: {
      type: Array,
      required: true
    }
  },
  computed: {
    boardSlots() {
      const emptySlots = Array(8 - this.cardsOnBoard.length).fill(null);
      return [...this.cardsOnBoard, ...emptySlots]; 
    }
  },
  methods: {
    onDrop(event, i) {
    const card = JSON.parse(event.dataTransfer.getData('card'));
    const isPlayerOne = this.$parent.currentPlayerTurn === 1;

    // Restreint l'accès à la moitié du plateau
    if ((isPlayerOne && i < 4) || (!isPlayerOne && i >= 4)) {
      console.log("Vous ne pouvez pas placer une carte ici !");
      alert("Vous ne pouvez pas placer une carte ici !"); // Alerte en cas d'emplacement invalide
      return;
    }

    this.$emit('card-dropped', { card, index: i });
    }
  }
}
</script>

<style scoped>
.cards-on-board {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  grid-gap: 10px;
}

.card-slot {
  width: 200px; 
  height: 350px;
  background-color: #e0e0e0;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 2px solid #ccc;
}
</style>
