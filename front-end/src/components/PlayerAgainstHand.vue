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
    width: 128px;
    height: 200px;
    object-fit: cover;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    text-align: center;
    padding: 10px;
    background-color: white;
  }
  </style>
  