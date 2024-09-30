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
      :gameId="gameId"
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
    gameId: Number,
    cardsOnBoard: {
      type: Array,
      required: true
    }
  },
  computed: {
    // Remplit les emplacements vides pour afficher 8 cases de jeu
    boardSlots() {
      const emptySlots = Array(8 - this.cardsOnBoard.length).fill(null);
      return [...this.cardsOnBoard, ...emptySlots]; 
    }
  },
  methods: {
    onDrop(event, i) {
  // Récupération des données transférées
  const cardData = event.dataTransfer.getData('card');
  
  if (!cardData) {
    console.error("Aucune donnée de carte trouvée lors du drag-and-drop !");
    alert("Erreur de transfert de carte.");
    return;
  }

  // Conversion en objet JavaScript
  const card = JSON.parse(cardData);

  // Comparer avec player1Id et player2Id
  const isPlayerOne = this.$parent.currentPlayerTurn === this.$parent.player1Id;

  console.log("Joueur courant : ", this.$parent.currentPlayerTurn);
  console.log("Player 1 ID : ", this.$parent.player1Id);
  console.log("Player 2 ID : ", this.$parent.player2Id);

  // Restriction d'accès aux emplacements en fonction du joueur
  // Si c'est le joueur 1, il doit poser sur les emplacements 4 à 7
  // Si c'est le joueur 2, il doit poser sur les emplacements 0 à 3
  if ((isPlayerOne && i < 4) || (!isPlayerOne && i >= 4)) {
    console.log("Vous ne pouvez pas placer une carte ici !");
    alert("Vous ne pouvez pas placer une carte ici !");
    return;
  }

  // Émission de l'événement pour indiquer que la carte a été déposée
  this.$emit('card-dropped', {
    card,
    index: i
  });
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
  width: 7rem;
  height: 10rem;
  background-color: #e0e0e0;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 2px solid #ccc;
}
.cards-on-board .CardComponent {
  width: 6rem;
  height: 9rem;
  padding: .5rem;
}

</style>
