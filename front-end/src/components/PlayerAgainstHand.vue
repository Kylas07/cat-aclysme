<template>
    <div class="hand">
      <img v-for="n in opponentHandSize" :key="n" src="../assets/main_illu.png" alt="Card back" class="card-back"/>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      opponentHandSize: Number
    }
  }
  </script>
  
  <style scoped>
  .hand {
    display: flex;
    justify-content: center;
    gap: 10px;
  }
  
  .card-back {
    width: 80px;
    height: 120px; /* Taille du dos des cartes */
  }
  </style>
  