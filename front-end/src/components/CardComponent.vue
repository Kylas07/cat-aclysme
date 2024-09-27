<template>
    <div class="CardComponent"
    @click="attack" 
    @dragstart="onDragStart"
    :draggable="isDraggable">
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
    async attack(targetCard) {
    const response = await fetch('https://localhost:7111/api/game/attack', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        GameId: this.gameId,  // ID de la partie
        PlayerId: this.card.player,    // Le joueur qui attaque
        BoardSlotId: this.card.boardSlotId, // Emplacement de la carte attaquante
        TargetBoardSlotId: targetCard.boardSlotId // Emplacement de la carte cible
      })
    });

    const result = await response.json();
    if (response.ok) {
      console.log("Attaque réussie", result);
      // Mettre à jour l'interface selon la réponse
    } else {
      console.log("Erreur d'attaque", result.message);
      console.log(this.gameId);
      alert(result.message);
    }
  },
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
    width: 180px;
    height: 280px;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    text-align: center;
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
  