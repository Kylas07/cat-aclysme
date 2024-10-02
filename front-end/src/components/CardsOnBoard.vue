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

    const targetIndex = this.getTargetIndex(attackerSlotIndex);
    if (targetIndex === null || !this.boardSlots[targetIndex]) {
        alert("Aucune cible en face !");
        return;
    }

    const payload = {
        GameId: this.gameId,
        PlayerId: this.currentPlayerTurn,
        BoardSlotId: attackerSlotIndex,  // Utilisation directe
        TargetBoardSlotId: targetIndex   // Utilisation directe
    };

    console.log("Payload envoyé au backend :", payload);

    const response = await fetch('https://localhost:7111/api/game/attack', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
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
        console.log("Conditions for attacking:");
        console.log("isPlacedPreviousTurn:", card.isPlacedPreviousTurn);
        console.log("hasAttackedThisTurn:", card.hasAttackedThisTurn);
        console.log("ownerId:", card.ownerId);
        console.log("currentPlayerTurn:", this.currentPlayerTurn);
        console.log("ownerId matches currentPlayerTurn:", card.ownerId === this.currentPlayerTurn);

        return card.isPlacedPreviousTurn && !card.hasAttackedThisTurn && card.ownerId === this.currentPlayerTurn;
    },

    getTargetIndex(attackerIndex) {
      const totalSlots = 8; // Nombre total d'emplacements sur le plateau
      const boardHalf = totalSlots / 2; // La moitié du plateau

      console.log(`Attacker Index: ${attackerIndex}`);
      console.log(`Current Player Turn: ${this.currentPlayerTurn}`);
      console.log(`Player 1 ID: ${this.player1Id}, Player 2 ID: ${this.player2Id}`);

      if (this.currentPlayerTurn === this.player1Id) {
        // Pour le joueur 1, la cible est 4 cases plus tôt (en face)
        const targetIndex = attackerIndex - boardHalf;
        console.log(`Calculated target index for Player 1: ${targetIndex}`);
        if (targetIndex >= 0 && this.boardSlots[targetIndex]) {
          console.log(`Target found at index ${targetIndex}:`, this.boardSlots[targetIndex]);
          return targetIndex;
        } else {
          console.log(`No valid target at index ${targetIndex}`);
        }
      } else {
        // Pour le joueur 2, la cible est 4 cases plus loin (en face)
        const targetIndex = attackerIndex + boardHalf;
        console.log(`Calculated target index for Player 2: ${targetIndex}`);
        if (targetIndex < totalSlots && this.boardSlots[targetIndex]) {
          console.log(`Target found at index ${targetIndex}:`, this.boardSlots[targetIndex]);
          return targetIndex;
        } else {
          console.log(`No valid target at index ${targetIndex}`);
        }
      }

      // Aucun indice valide trouvé
      console.log("No target found, returning null.");
      return null;
    },

    onDrop(event, i) {
      console.log(`Carte déposée sur la case avec l'index : ${i}`); // Log pour afficher l'index de la case

      const cardData = event.dataTransfer.getData('card');
      if (!cardData) {
        console.error("Aucune donnée de carte trouvée lors du drag-and-drop !");
        alert("Erreur de transfert de carte.");
        return;
      }

      const card = JSON.parse(cardData);

      // Assigner le joueur actuel comme propriétaire de la carte
      card.ownerId = this.currentPlayerTurn;

      const isPlayerOne = this.$parent.currentPlayerTurn === this.$parent.player1Id;

      if ((isPlayerOne && i < 4) || (!isPlayerOne && i >= 4)) {
        alert("Vous ne pouvez pas placer une carte ici !");
        return;
      }

      this.$emit('card-dropped', { card, index: i });
    },
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