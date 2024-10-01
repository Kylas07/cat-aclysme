<template>
  <div class="game-board">
    <div class="board-top">
      <PlayerAgainstHand :opponentHandSize="opponentHandSize" />

      <GameInfo 
        :gameId="gameId"
        :currentTurn="currentTurn"
        :turnCount="turnCount" 
        :player1HP="player1HP" 
        :player2HP="player2HP"
        @end-turn="endTurn"
        :current-player-turn="currentPlayerTurn"
      />
    </div>

    <div class="game-decks">
      <PlayerDeck :cardsLeft="player2DeckSize" />
      
      <!-- Plateau avec transition spécifique aux cartes -->
      <div class="cards-on-board-container">
        <CardsOnBoard 
          v-bind:style="[currentPlayerTurn === player1Id ? stylePlayer1 : stylePlayer2]"
          :cardsOnBoard="cardsOnBoard" 
          :gameId="gameId"
          :player1Id="player1Id"
          :player2Id="player2Id"
          :currentTurn="currentTurn" 
          :currentPlayerTurn="currentPlayerTurn"
          :isFlipped="currentPlayerTurn !== player1Id"
          @card-dropped="handleCardDrop"
        />
      </div>

      <PlayerDeck :cardsLeft="player1DeckSize" />
    </div>

    <!-- Main du joueur actif seulement -->
    <PlayerHand 
      :playerHand="playerHand" 
      :gameId="gameId"
      :isPlayerTurn="currentPlayerTurn === player1Id || currentPlayerTurn === player2Id"
      @card-dropped="handleCardDrop"
      :currentPlayerTurn="currentPlayerTurn"
      :player1Id="player1Id"
      :player2Id="player2Id"
    />
  </div>
</template>

<script>
import PlayerHand from './PlayerHand.vue';
import PlayerAgainstHand from './PlayerAgainstHand.vue';
import CardsOnBoard from './CardsOnBoard.vue';
import GameInfo from './GameInfo.vue';
import PlayerDeck from './PlayerDeck.vue';

export default {
  props: {
    gameId: {  
      type: Number,
      required: true
    },
    currentTurn: Number,
    player1HP: Number,
    player2HP: Number,
    player1Id: Number,
    player2Id: Number,
    currentPlayerId: Number,
    
  },
  data() {
    return {
      turnCount:1,
      currentPlayerTurn: this.currentPlayerId,  // Démarre avec le joueur courant
      playerHand: [],
      cardsOnBoard: [],
      opponentHandSize: 5, // Nombre de cartes dans la main de l'adversaire
      player1DeckSize: 25, // Cartes restantes dans le deck du joueur 1
      player2DeckSize: 25,  // Cartes restantes dans le deck du joueur 2
      stylePlayer1: {
        rotate: '0deg'
      },
      stylePlayer2: {
        rotate: '180deg'
      }
    };
  },
  components: {
    PlayerHand,
    PlayerAgainstHand,
    CardsOnBoard,
    GameInfo,
    PlayerDeck
  },
  methods: {
    async endTurn() {
      try {
        const response = await fetch('https://localhost:7111/api/game/end-turn', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            GameId: this.gameId,
            PlayerId: this.currentPlayerTurn
          })
        });

        const data = await response.json();
        if (response.ok) {
          console.log("Tour terminé :", data);

          // Alterner le joueur
          this.currentPlayerTurn = this.currentPlayerTurn === this.player1Id ? this.player2Id : this.player1Id;

          this.cardsOnBoard.forEach(card => {
        if (card) {
          card.isPlacedPreviousTurn = true;   // La carte peut attaquer lors du prochain tour
          card.hasAttackedThisTurn = false;   // Réinitialiser la capacité d'attaque
        }
      });

          // Charger la main du joueur suivant
          await this.loadPlayerHand(this.currentPlayerTurn);
          this.turnCount += 1;
        } else {
          alert(data.message);
        }
      } catch (error) {
        console.error("Erreur lors du passage de tour :", error);
      }
    },

    async loadPlayerHand(playerId) {
      try {
        const response = await fetch(`https://localhost:7111/api/game/deck/${this.gameId}/${playerId}`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json'
          }
        });

        if (response.ok) {
          const data = await response.json();
          if (data.values && data.values.$values && Array.isArray(data.values.$values)) {
            this.playerHand = data.values.$values
              .filter(card => card.cardState === 1)
              .map(card => ({
                cardId: card.cardId,
                name: card.card.name,
                health: card.card.health,
                attack: card.card.attack,
                image: `/image/${card.card.image}`,
                description: card.card.description
              }));
            console.log("Main du joueur :", this.playerHand);
          } else {
            console.error('$values est undefined ou n\'est pas un tableau.', data);
          }
        } else {
          console.error('Erreur lors de la récupération du deck:', response.status, response.statusText);
        }
      } catch (error) {
        console.error('Erreur lors de la communication avec le serveur:', error);
      }
    },

    async handleCardDrop({ card, index }) {
    // Restreindre le placement des cartes en fonction du joueur
    if (this.currentPlayerTurn === this.player1Id && index < 4) {
      // Le joueur 1 essaie de poser sur les cases du haut, ce qui est interdit
      alert("Le joueur 1 ne peut poser que sur les 4 cases du bas !");
      return;
    }
    
    if (this.currentPlayerTurn === this.player2Id && index >= 4) {
      // Le joueur 2 essaie de poser sur les cases du bas, ce qui est interdit
      alert("Le joueur 2 ne peut poser que sur les 4 cases du haut !");
      return;
    }

    // Si le placement est valide, envoyer la requête pour poser la carte
    console.log("Carte envoyée :", card);
    const response = await fetch('https://localhost:7111/api/game/play-card', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        GameId: this.gameId, 
        PlayerId: this.currentPlayerTurn,
        CardId: card.cardId, 
        BoardSlotIndex: index
      })
    });

    const data = await response.json();
    if (response.ok) {
      console.log("Réponse de l'API :", data);
      // Mettre à jour directement l'élément dans cardsOnBoard
      this.cardsOnBoard[index] = card;
      // Retirer la carte de la main du joueur
      this.playerHand = this.playerHand.filter(c => c.cardId !== card.cardId);
      console.log("Le joueur", this.currentPlayerTurn, "pose une carte.");
    } else {
      console.log("Erreur de placement", data.message);
      alert(data.message);
    }
  },
  
    async loadTurnCount() {
      const response = await fetch('https://localhost:7111/api/game/${this.gameId}');
      const data = await response.json();

      this.turnCount = data.game.turnCount;
    },
  nextTurn() {
      this.$emit('update-turn');
    },

  },
  async mounted() {
    console.log("Méthode mounted appelée");
    await this.loadPlayerHand(this.currentPlayerTurn);
  }
}
</script>

<style scoped>
.game-board {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.cards-on-board-container {
  display: flex;
  justify-content: center;
  perspective: 1000px; /* Perspective pour l'effet 3D */
}

.cards-on-board {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  grid-gap: 10px;
  transition: transform 0.6s ease; /* Transition pour le basculement des cartes */
  transform-style: preserve-3d;
}

.game-board.flipped .cards-on-board {
  transform: rotateX(180deg); /* Bascule uniquement les cartes sur le plateau */
}

.board-top {
  display: flex;
  flex-direction: row-reverse;
  padding: 1rem;
  align-items: center;
}

.game-decks {
  display: flex;
  align-items: center;
  gap: 100px;
  padding: 1rem;
}

.card {
  width: 100px;
  height: 150px;
  background-color: white;
  border: 1px solid #ccc;
  display: flex;
  justify-content: center;
  align-items: center;
  transition: transform 0.6s ease; /* Transition pour l'animation de la carte */
}

.flipped .card {
  transform: rotateY(180deg);
}
</style>
