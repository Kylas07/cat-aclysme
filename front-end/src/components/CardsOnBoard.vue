<template>
  <div class="cards-on-board">
    <div 
      v-for="(card, i) in boardSlots" 
      :key="i" 
      class="card-slot"
      @click="attack(card, i)" 
      @drop="onDrop($event, i)"
      @dragover.prevent
      v-bind:style="[currentPlayerTurn === player1Id ? stylePlayer1 : stylePlayer2]"
    >
      <CardComponent 
        v-if="card" 
        :card="card" 
        :isOnBoard="true" 
        :gameId="gameId"
        :slotIndex="i"
        @card-attacked="handleCardAttack(card, i)"
      />
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
    cardsOnBoard: Array,
    gameId: Number,
    currentTurn: Number,
    currentPlayerTurn: Number,
    player1Id: Number,
    player2Id: Number,
  },
  data() {
    return {
      stylePlayer1: {
        rotate: '0deg'
      },
      stylePlayer2: {
        rotate: '180deg'
      }
    };
  },
  computed: {
    // Remplit les emplacements vides pour afficher 8 cases de jeu
    boardSlots() {
      const emptySlots = Array(8 - this.cardsOnBoard.length).fill(null);
      return [...this.cardsOnBoard, ...emptySlots]; 
    }
  },
  methods: {
    async attack(card, attackerSlotIndex) {
  if (!card || !this.canAttack(card)) {
    alert("Cette carte ne peut pas attaquer.");
    return;
  }

  // Récupérer la cible en face selon la position de l'attaquant
  const targetIndex = this.getTargetIndex(attackerSlotIndex);
  if (targetIndex === null || !this.boardSlots[targetIndex]) {
    alert("Aucune cible en face !");
    return;
  }

  // Émettre un événement au parent pour notifier que la carte a attaqué
  this.$emit('card-attacked', card);

  console.log({
    GameId: this.gameId,
    PlayerId: this.currentPlayerTurn,
    BoardSlotId: attackerSlotIndex,
    TargetBoardSlotId: targetIndex
  });

  // Envoyer la requête d'attaque au serveur
  const response = await fetch('https://localhost:7111/api/game/attack', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      GameId: this.gameId,
      PlayerId: this.currentPlayerTurn,
      BoardSlotId: attackerSlotIndex,
      TargetBoardSlotId: targetIndex
    })
  });

  const result = await response.json();
  if (response.ok) {
    console.log("Attaque réussie", result);
    this.$emit('card-updated', { ...card, hasAttackedThisTurn: true });
  } else {
    console.log("Erreur d'attaque", result.message);
    alert(result.message);
  }
},

    canAttack(card) {
      return card.isPlacedPreviousTurn && !card.hasAttackedThisTurn && card.ownerId === this.currentPlayerTurn;
    },

    getTargetIndex(attackerIndex) {
      if (this.currentPlayerTurn === this.player1Id) {
        return attackerIndex + 4 < this.cardsOnBoard.length ? attackerIndex + 4 : null;
      } else {
        return attackerIndex - 4 >= 0 ? attackerIndex - 4 : null;
      }
    },

    onDrop(event, i) {
      const cardData = event.dataTransfer.getData('card');
      if (!cardData) {
        console.error("Aucune donnée de carte trouvée lors du drag-and-drop !");
        alert("Erreur de transfert de carte.");
        return;
      }

      const card = JSON.parse(cardData);
      const isPlayerOne = this.$parent.currentPlayerTurn === this.$parent.player1Id;

      if ((isPlayerOne && i < 4) || (!isPlayerOne && i >= 4)) {
        alert("Vous ne pouvez pas placer une carte ici !");
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
