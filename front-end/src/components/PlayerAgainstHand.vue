<template>
  <div class="hand" @dragover.prevent>
    <img 
      v-for="n in opponentHandSize" 
      :key="n" 
      src="../assets/main_illu.png" 
      alt="Card back" 
      class="card-back" 
      draggable="false"
    />
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
    margin: 0 5rem;
  }
  
  .card-back {

    object-fit: cover;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    text-align: center;
    background-color: white;
  }
  .hand .card-back {
    width: 4rem;
    height: 6rem;
    padding: .5rem;
}
  </style>
  