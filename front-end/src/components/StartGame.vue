<template>
  <div>
    <h2>Lancer une partie</h2>
    <button @click="startGame">Lancer la partie</button>
  </div>
</template>

<script>
export default {
  props: {
    player1Pseudo: {
      type: String,
      required: true
    },
    player2Pseudo: {
      type: String,
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
            Player1Pseudo: this.player1Pseudo,
            Player2Pseudo: this.player2Pseudo
          })
        });
        if (response.ok) {
          const data = await response.json();
          alert(`Partie lancée avec succès. ID de la partie : ${data.gameId}`);

          // Stocker le gameId pour appeler plus tard l'état du jeu
          this.$emit('game-started', data.gameId);
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
