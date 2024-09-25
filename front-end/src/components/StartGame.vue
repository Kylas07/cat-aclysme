<template>
  <div>
    <h2>Lancer une partie</h2>
    <button @click="startGame">Lancer la partie</button>
  </div>
</template>

<script>
export default {
  props: {
    player1Id: {
      type: Number,
      required: true
    },
    player2Id: {
      type: Number,
      required: true
    }
  },
  methods: {
    async startGame() {
      try {
        const response = await fetch('https://localhost:7111/api/game/start', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            Player1Id: this.player1Id,  // Passer les ID des joueurs
            Player2Id: this.player2Id
          })
        });
        if (response.ok) {
          const data = await response.json();
          alert(`Partie lancée avec succès. ID de la partie : ${data.gameId}`);
        } else {
          const errorData = await response.json();
          alert(`Erreur lors du lancement de la partie: ${errorData.message}`);
        }
      } catch (error) {
        console.error('Erreur:', error);
        alert('Erreur lors de la communication avec le serveur');
      }
    }
  }
};
</script>
