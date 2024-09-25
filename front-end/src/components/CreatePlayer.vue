<template>
  <div>
    <h2>Créer un joueur</h2>
    <form @submit.prevent="createPlayer">
      <div>
        <label for="name">Nom :</label>
        <input type="text" v-model="player.name" required />
      </div>
      <div>
        <label for="password">Mot de passe :</label>
        <input type="password" v-model="player.password" required />
      </div>
      <button type="submit">Créer le joueur</button>
    </form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      player: {
        name: '',
        password: ''
      }
    };
  },
  methods: {
    async createPlayer() {
      try {
        const response = await fetch('https://localhost:7111/api/home/register', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            PlayerName: this.player.name,  // Changer 'name' en 'PlayerName'
            Password: this.player.password // Correspond bien au modèle du back-end
          })
        });
        if (response.ok) {
          alert('Joueur créé avec succès');
          this.player.name = '';
          this.player.password = '';
        } else {
          const errorData = await response.json();
          alert('Erreur lors de la création du joueur: ' + errorData.title);
        }
      } catch (error) {
        console.error('Erreur:', error);
        alert('Erreur lors de la communication avec le serveur');
      }
    }
  }
};
</script>
