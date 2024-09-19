<template>
    <div class="CardComponent">
      <img class="card-image" :src="card.image" :alt="card.name" />
      <div class="card-details">
        <h3 class="card-name">{{ card.name }}</h3>
        <p class="card-description">{{ card.description }}</p>
        <div class="card-stats">
          <span class="card-health">❤️ {{ card.health }}</span>
          <span class="card-attack">⚔️ {{ card.attack }}</span>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'CardComponent',
    props: {
      card: {
        type: Object,
        required: true
      }
    }
  }
  </script>
  
  <style scoped>
  .CardComponent {
    width: 150px;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    text-align: center;
    margin: 10px;
    padding: 10px;
    background-color: white;
  }
  
  .card-image {
    width: 100%;
    height: auto;
    border-radius: 8px;
  }
  
  .card-details {
    margin-top: 10px;
  }
  
  .card-name {
    font-size: 18px;
    font-weight: bold;
  }
  
  .card-description {
    font-size: 14px;
    color: #666;
    margin: 5px 0;
  }
  
  .card-stats {
    display: flex;
    justify-content: space-between;
    font-size: 14px;
  }
  
  .card-health, .card-attack {
    font-weight: bold;
    padding: 5px;
  }
  </style>
  