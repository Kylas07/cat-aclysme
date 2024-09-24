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
          const response = await fetch('LE BACK-END API', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.player)
          });
          if (response.ok) {
            alert('Joueur créé avec succès');
            this.player.name = '';
            this.player.password = '';
          } else {
            alert('Erreur lors de la création du joueur');
          }
        } catch (error) {
          console.error('Erreur:', error);
        }
      }
    }
  };
  </script>
  