<template>
  <div>
    <h1>Bienvenue</h1>

    <!-- Bouton pour basculer entre la connexion et la création de joueur -->
    <button v-if="!showCreatePlayer" @click="showCreatePlayer = true">Créer un compte</button>
    <button v-else @click="showCreatePlayer = false">Retour à la connexion</button>

    <!-- Formulaire de connexion -->
    <div v-if="!showCreatePlayer">
      <!-- Formulaire pour le premier joueur -->
      <div v-if="!isPlayer1LoggedIn">
        <h2>Connexion du Joueur 1</h2>
        <form @submit.prevent="handleSubmitPlayer1">
          <div>
            <label for="name1">Pseudo Joueur 1 :</label>
            <input type="text" v-model="player1.name" required />
          </div>
          <div>
            <label for="password1">Mot de passe :</label>
            <input type="password" v-model="player1.password" required />
          </div>
          <button type="submit">Se connecter</button>
        </form>
      </div>

      <!-- Formulaire pour le deuxième joueur -->
      <div v-else-if="!isPlayer2LoggedIn">
        <h2>Connexion du Joueur 2</h2>
        <form @submit.prevent="handleSubmitPlayer2">
          <div>
            <label for="name2">Pseudo Joueur 2 :</label>
            <input type="text" v-model="player2.name" required />
          </div>
          <div>
            <label for="password2">Mot de passe :</label>
            <input type="password" v-model="player2.password" required />
          </div>
          <button type="submit">Se connecter</button>
        </form>
      </div>

      <!-- Lancer la partie si les deux joueurs sont connectés -->
      <div v-else>
        <h2>Joueurs connectés :</h2>
        <p>Joueur 1 : {{ player1.name }}</p>
        <p>Joueur 2 : {{ player2.name }}</p>
        <button @click="startGame">Lancer une partie</button>
      </div>
    </div>

    <!-- Formulaire de création de joueur -->
    <div v-if="showCreatePlayer">
      <CreatePlayer />
    </div>
  </div>
</template>

<script>
import CreatePlayer from './CreatePlayer.vue';

export default {
  data() {
    return {
      showCreatePlayer: false, // Flag pour afficher la création de compte ou la connexion
      isPlayer1LoggedIn: false,
      isPlayer2LoggedIn: false,
      player1: {
        name: '',
        password: ''
      },
      player2: {
        name: '',
        password: ''
      }
    };
  },
  components: {
    CreatePlayer
  },
  methods: {
    async handleSubmitPlayer1() {
      try {
        const response = await this.loginPlayer(this.player1);
        const data = await response.json();
        if (response.ok) {
          this.isPlayer1LoggedIn = true;
          this.player1.id = data.playerId;
          alert(data.message || "Joueur 1 connecté avec succès !");
          this.checkPlayers(); // Vérifiez si les deux joueurs sont connectés
        } else {
          alert(data.message || "Connexion Joueur 1 échouée. Veuillez vérifier vos identifiants.");
        }
      } catch (error) {
        console.error("Erreur lors de la connexion du Joueur 1:", error);
      }
    },
    async handleSubmitPlayer2() {
      try {
        const response = await this.loginPlayer(this.player2);
        const data = await response.json();
        if (response.ok) {
          this.isPlayer2LoggedIn = true;
          this.player2.id = data.playerId;
          alert(data.message || "Joueur 2 connecté avec succès !");
          this.checkPlayers(); // Vérifiez si les deux joueurs sont connectés
        } else {
          alert(data.message || "Connexion Joueur 2 échouée. Veuillez vérifier vos identifiants.");
        }
      } catch (error) {
        console.error("Erreur lors de la connexion du Joueur 2:", error);
      }
    },
    async loginPlayer(player) {
      const response = await fetch('https://localhost:7111/api/home/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          PlayerName: player.name,
          Password: player.password
        })
      });
      return response;
    },
    checkPlayers() {
      // Vérifiez si les deux joueurs sont connectés
      if (this.isPlayer1LoggedIn && this.isPlayer2LoggedIn) {
        this.startGame();
      }
    },
    async startGame() {
      console.log('coucou');
          try {
        const requestBody = {
          Player1Pseudo: this.player1.name,
          Player2Pseudo: this.player2.name
        };
        console.log('Request Body:', requestBody); // Ajoutez cette ligne
        
        const response = await fetch('https://localhost:7111/api/game/start', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(requestBody)
        });
        if (response.ok) {
          const data = await response.json();
          this.$emit('start-game', data.gameId, data.currentTurn, data.player1HP, data.player2HP, data.turnCount); 
          alert(`Partie lancée avec succès. ID de la partie : ${data.gameId}`);
        } else {
          const errorData = await response.json();
          console.error('Error Response:', errorData);
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
