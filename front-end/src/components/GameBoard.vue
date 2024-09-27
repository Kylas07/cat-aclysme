<template>
    <div class="game-board">
      <PlayerAgainstHand :opponentHandSize="opponentHandSize" />
    
      <GameInfo 
        :gameId="gameId"
        :currentTurn="currentTurn" 
        :player1HP="player1HP" 
        :player2HP="player2HP" 
      />
      <div class="game-decks">
        <PlayerDeck :cardsLeft="player2DeckSize" />
        <CardsOnBoard 
          :cardsOnBoard="cardsOnBoard" 
          :gameId="gameId"
          @card-dropped="handleCardDrop"
        />
        <PlayerDeck :cardsLeft="player1DeckSize" />
      </div>
      <PlayerHand 
      :playerHand="playerHand" 
      :isPlayerTurn="currentPlayerTurn === 1"
      @card-dropped="handleCardDrop"
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
    gameId: Number,
    currentTurn: Number,
    player1HP: Number,
    player2HP: Number,
    player1Id: Number, // Ajoutez l'ID du joueur 1
    player2Id: Number,  // Ajoutez l'ID du joueur 2
    currentPlayerId: Number
  },
  data() {
    return {
      // Main du joueur 1 (cartes fictives pour test)
      playerHand: [],
      // Cartes sur le plateau de jeu
      cardsOnBoard: [
        {
          cardId: 4,
          name: "Catnado",
          health: 15,
          attack: 4,
          image: require('@/assets/Catnado.png'),
          description: "Meow."
        },
        {
          cardId: 5,
          name: "Catnado",
          health: 5,
          attack: 6,
          image: require('@/assets/Catnado.png'),
          description: "Meow."
        }
      ],
      opponentHandSize: 5, // Nombre de cartes dans la main de l'adversaire
      player1DeckSize: 15, // Cartes restantes dans le deck du joueur 1
      player2DeckSize: 18 // Cartes restantes dans le deck du joueur 2
    };
  },
  components: {
    PlayerHand,
    PlayerAgainstHand,
    CardsOnBoard,
    GameInfo,
    PlayerDeck
  },
  computed: {
    isPlayerTurn() {
      // Comparer l'ID du joueur actuel avec currentTurn
      return (this.currentTurn === this.player1Id && this.isPlayer1) || 
            (this.currentTurn === this.player2Id && !this.isPlayer1);
    }
  },
  methods: {
    async handleCardDrop({ card, index }) {
      console.log("Carte envoyée :", card);
      const response = await fetch('https://localhost:7111/api/game/play-card', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          GameId: this.gameId, 
          PlayerId: this.currentPlayerId,
          CardId: card.cardId, 
          BoardSlotIndex: index
        })
      });
      const data = await response.json();
    if (response.ok) {
        console.log("Réponse de l'API :", data);
        // Mets à jour directement l'élément dans cardsOnBoard
        this.cardsOnBoard[index] = card;
        // Retire la carte de la main du joueur
        this.playerHand = this.playerHand.filter(c => c.cardId !== card.cardId);
        console.log("Le joueur", this.currentPlayerId, "pose une carte.");
      } else {
      console.log("Erreur de placement", data.message);
      console.log("La carte de ", this.currentPlayerId, "n'a pas pu être posée", "à l'emplacement ", index);
      alert(data.message);
    }
  },
  nextTurn() {
    this.$emit('update-turn');
  }
}
}
</script>

  
  <style scoped>
  .game-board {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  .game-decks {
    display: flex;
    align-items: center;
    gap: 100px;
  }
  </style>
  