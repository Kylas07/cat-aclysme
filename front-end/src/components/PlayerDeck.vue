<template>
     <div :class="['player-deck', playerId === 1 ? 'player-1-deck' : 'player-2-deck']">
      <div class="stack-deck">
        <img src="/image/catnado.png" alt="Deck image" class="deck-image"/>
        <img src="/image/catnado.png" alt="Deck image" class="deck-image"/>
        <img src="/image/catnado.png" alt="Deck image" class="deck-image"/>
      </div>
      <p>{{ cardsLeft }} cartes restantes</p>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      cardsLeft: Number,
      playerId: {
      type: Number,
      required: true
      }
    }
  }
  </script>
  
  <style scoped>
  .playerdeck {
    text-align: center;
  }
    .player-1-deck {
    background-color: blue; /* Couleur pour le joueur 1 */
    padding: 1rem;
    color:white;
  }

  .player-2-deck {
    background-color: red; /* Couleur pour le joueur 2 */
    padding: 1rem;
    color:white;
  }
  .stack-deck {
    display: grid;
    grid-template-columns: 4px 4px 1fr;
  }
  .deck-image {
    width: 128px;
    height: 200px;
    -o-object-fit: cover;
    object-fit: cover;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    text-align: center;
    padding: 10px;
    background-color: white;
  }
  </style>
  