<template>
    <div class="playerdeck">
      <img :src="deckImage" alt="Deck image" class="deck-image"/>
      <p>{{ cardsLeft }} cards left</p>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      cardsLeft: Number,
      deckImage: {
        type: String,
        default: '../assets/logo.png'  // Image du deck
      }
    }
  }
  </script>
  
  <style scoped>
  .playerdeck {
    text-align: center;
  }
  
  .deck-image {
    width: 100px;
  }
  </style>
  