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
        BoardSlotId: attackerSlotIndex,
        TargetBoardSlotId: targetIndex
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
        console.log("Réponse du backend pour l'attaque:", result);

        // Logs avant la mise à jour des HP
        console.log("HP avant mise à jour - Attaquant:", this.cardsOnBoard[attackerSlotIndex]?.health);
        console.log("HP avant mise à jour - Défenseur:", this.cardsOnBoard[targetIndex]?.health);

        // Mettre à jour la carte attaquante
        if (result.attackingCard && typeof result.attackingCard.health !== 'undefined') {
          const updatedAttacker = this.cardsOnBoard[attackerSlotIndex];
          updatedAttacker.health = result.attackingCard.health;  // Utiliser 'health' en minuscule
          console.log("HP après mise à jour - Attaquant:", updatedAttacker.health);
          this.$emit('card-updated', { card: updatedAttacker, slotIndex: attackerSlotIndex });
        }

        // Mettre à jour la carte défenseur
        if (result.defendingCard && typeof result.defendingCard.health !== 'undefined') {
          const updatedDefender = this.cardsOnBoard[targetIndex];
          updatedDefender.health = result.defendingCard.health;  // Utiliser 'health' en minuscule
          console.log("HP après mise à jour - Défenseur:", updatedDefender.health);
          this.$emit('card-updated', { card: updatedDefender, slotIndex: targetIndex });
        }

        // Indiquer que la carte attaquante a attaqué ce tour
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
      const totalSlots = 8;
      const boardHalf = totalSlots / 2;

      if (this.currentPlayerTurn === this.player1Id) {
        const targetIndex = attackerIndex - boardHalf;
        return targetIndex >= 0 ? targetIndex : null;
      } else {
        const targetIndex = attackerIndex + boardHalf;
        return targetIndex < totalSlots ? targetIndex : null;
      }
    },

    onDrop(event, i) {
      const cardData = event.dataTransfer.getData('card');
      if (!cardData) {
        alert("Erreur de transfert de carte.");
        return;
      }

      const card = JSON.parse(cardData);
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
