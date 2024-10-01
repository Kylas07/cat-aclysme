<template>
  <div>
    <h2>Créer un joueur</h2>
    <form @submit.prevent="createPlayer">
      <div>
        <v-text-field label="Pseudo Joueur 1" prepend-icon="mdi-cat" variant="outlined"  v-model="player.name" required ></v-text-field>
      </div>
      <div>
      <v-text-field
      label="Password Joueur 1"
      prepend-icon="mdi-cat"
      :type="showPassword ? 'text' : 'password'"
      variant="outlined"
      v-model="player.password"
      required
    >
      </v-text-field>
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
