<template>
  <div class="cards-on-board">
    <div v-for="(card, index) in boardSlots" :key="index" class="card-slot">
      <!-- Afficher la carte seulement si elle existe dans cardsOnBoard -->
      <CardComponent v-if="card" :card="card" />
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
      // On veut 8 emplacements (4 pour chaque joueur), remplis ou non
      const emptySlots = Array(8 - this.cardsOnBoard.length).fill(null);
      return [...this.cardsOnBoard, ...emptySlots]; 
    }
  }
}
</script>

<style scoped>
.cards-on-board {
  display: grid;
  grid-template-columns: repeat(4, 1fr); /* 4 colonnes */
  grid-gap: 10px;
}

.card-slot {
  width: 100px;  /* Ajuste en fonction de la taille de tes cartes */
  height: 150px; /* Ajuste en fonction de la taille de tes cartes */
  background-color: #e0e0e0; /* Couleur des emplacements vides */
  display: flex;
  justify-content: center;
  align-items: center;
  border: 2px solid #ccc;
}
</style>
