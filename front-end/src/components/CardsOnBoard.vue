<template>
  <div class="cards-on-board">
    <div 
      v-for="(card, i) in boardSlots" 
      :key="i" 
      class="card-slot"
      @click="handleCardClick(card, i)"
      @drop="onDrop($event, i)"
      @dragover.prevent
      v-bind:style="[currentPlayerTurn === player1Id ? stylePlayer1 : stylePlayer2]"
    >
      <CardComponent 
        v-if="card" 
        :card="card" 
        :isOnBoard="true" 
        :gameId="gameId"
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
        // Gère le clic sur une carte
        handleCardClick(card, index) {
      // Assurez-vous que la carte appartient au joueur actif et qu'elle peut attaquer
      if (card && this.canAttack(card)) {
        const targetIndex = this.getTargetIndex(index);
        if (targetIndex !== null) {
          this.initiateAttack(card, index, targetIndex);
        } else {
          alert("Aucune cible en face !");
        }
      } else {
        alert("Cette carte ne peut pas attaquer.");
      }
    },

    // Vérifie si la carte peut attaquer
    canAttack(card) {
      return card.isPlacedPreviousTurn && !card.hasAttackedThisTurn && card.ownerId === this.currentPlayerTurn;
    },

    // Calcule l'index de la cible (carte en face ou joueur)
    getTargetIndex(attackerIndex) {
      if (this.currentPlayerTurn === this.player1Id) {
        // Le joueur 1 attaque les cartes en face (index 4 à 7)
        return attackerIndex + 4 < this.cardsOnBoard.length ? attackerIndex + 4 : null;
      } else {
        // Le joueur 2 attaque les cartes en face (index 0 à 3)
        return attackerIndex - 4 >= 0 ? attackerIndex - 4 : null;
      }
    },

    // Envoie la requête d'attaque au serveur
    async initiateAttack(card, attackerIndex, targetIndex) {
      try {
        const response = await fetch('https://localhost:7111/api/game/attack', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            GameId: this.gameId,
            PlayerId: this.currentPlayerTurn,
            AttackerIndex: attackerIndex,
            TargetIndex: targetIndex,
          }),
        });

        if (response.ok) {
          const data = await response.json();
          console.log("Attaque réussie :", data);
          // Mettre à jour l'état de la carte pour marquer qu'elle a attaqué ce tour
          card.hasAttackedThisTurn = true;
        } else {
          const errorData = await response.json();
          console.error("Erreur d'attaque :", errorData.message);
        }
      } catch (error) {
        console.error("Erreur lors de l'attaque :", error);
      }
    },
  
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
