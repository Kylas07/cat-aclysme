<template>
    <div class="playerdeck">
      <div class="stack-deck">
        <img :src="deckImage" alt="Deck image" class="deck-image"/>
        <img :src="deckImage" alt="Deck image" class="deck-image"/>
        <img :src="deckImage" alt="Deck image" class="deck-image"/>
      </div>
      <p>{{ cardsLeft }} cards left</p>
    </div>
  </template>
  
  <script>
  import defaultDeckImage from '../assets/main_illu.png';

  export default {
    props: {
      cardsLeft: Number,
      deckImage: {
        type: String,
        default: defaultDeckImage 
      }
    }
  }
  </script>
  
  <style scoped>
  .playerdeck {
    text-align: center;
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
  