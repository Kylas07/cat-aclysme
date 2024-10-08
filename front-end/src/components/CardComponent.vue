<template>
    <div class="CardComponent"
    :gameId="gameId"
    :isOnBoard="false"
    @dragstart="onDragStart"
    :draggable="isDraggable">
      <img class="card-image" :src="card.image" :alt="card.name" />
      <div class="card-details">
        <div>       
          <h3 class="card-name">{{ card.name }}</h3>
          <p class="card-description">{{ card.description }}</p>
        </div>
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
    },
    isOnBoard: {
      type: Boolean,
      required: true
    },
    gameId: { 
      type: Number,
      required: true
    }
  },
  computed: {
    isDraggable() {
      return !this.isOnBoard; // La carte n'est pas déplaçable si elle est sur le plateau
    }
  },
  methods: {

    onDragStart(event) {
      if (!this.isDraggable) {
        alert("Cette carte est déjà placée sur le plateau et ne peut pas être déplacée !");
        event.preventDefault(); // Empêche l'action de drag
        return;
      }
      
      // Si la carte est draggable, on procède avec le drag
      event.dataTransfer.setData('card', JSON.stringify(this.card));
    }
  }
}

  </script>
  
  <style scoped>
  .CardComponent {
    width: 11rem;
    height: 19rem;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    text-align: center;
    padding: 10px;
    background-color: white;
    display: grid;
    grid-template-rows: 1fr 1fr;
    place-items: center;
}
  
  .card-image {
    width: 100%;
    height: auto;
    border-radius: 8px;
  }

  .card-name {
    font-size: 18px;
    font-weight: bold;
  }
  
  .card-description {
    font-size: 10px;
    color: #666;
    margin: 5px 0;
  }
  .card-details {
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
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
  .cards-on-board .card-name  {
  display:none;
}
.cards-on-board .card-description{
  display:none;
}
.cards-on-board img{
  width: 5rem;
}
  </style>
  