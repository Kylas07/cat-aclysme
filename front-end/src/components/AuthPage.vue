<template>
  <div class="conteneur-login">
    <h1>Bienvenue</h1>

    <!-- Formulaire de connexion -->
    <div class="login-player" v-if="!showCreatePlayer">
      <!-- Formulaire pour le premier joueur -->
      <div class="logplayer1" v-if="!isPlayer1LoggedIn">
        <h2>Connexion du Joueur 1</h2>
        <form @submit.prevent="handleSubmitPlayer1">
          <div>
            <v-text-field label="Pseudo Joueur 1" prepend-icon="mdi-cat" variant="outlined"  v-model="player1.name" required ></v-text-field>
          </div>
          <div>
            <v-text-field
                label="Password Joueur 1"
                prepend-icon="mdi-cat"
                :type="showPassword ? 'text' : 'password'"
                variant="outlined"
                v-model="player1.password"
                required
              >
            </v-text-field>
          </div>
          <v-btn type="submit">Se connecter</v-btn>
        </form>
      </div>

      <!-- Formulaire pour le deuxième joueur -->
      <div class="logplayer2"  v-else-if="!isPlayer2LoggedIn">
        <h2>Connexion du Joueur 2</h2>
        <form @submit.prevent="handleSubmitPlayer2">
          <div>
            <v-text-field label="Pseudo Joueur 2" prepend-icon="mdi-cat" variant="outlined"  v-model="player2.name" required ></v-text-field>
          </div>
          <div>
            <v-text-field                 
                label="Password Joueur 2"
                prepend-icon="mdi-cat"
                :type="showPassword ? 'text' : 'password'"
                variant="outlined"
                v-model="player2.password"
                required
              >
            </v-text-field>
          </div>
          <v-btn type="submit">Se connecter</v-btn>
        </form>
      </div>

      <!-- Lancer la partie si les deux joueurs sont connectés -->
      <!-- <div v-else>
        <h2>Joueurs connectés :</h2>
        <p>Joueur 1 : {{ player1.name }}</p>
        <p>Joueur 2 : {{ player2.name }}</p>
        <button @click="startGame">Lancer une partie</button>
      </div> -->
    </div>

    <!-- Formulaire de création de joueur -->
    <div class="login-player" v-if="showCreatePlayer">
      <CreatePlayer />
    </div>
    <!-- Bouton pour basculer entre la connexion et la création de joueur -->
    <v-btn size="x-large" prepend-icon="mdi-cat" append-icon="mdi-cat" variant="outlined" v-if="!showCreatePlayer" @click="showCreatePlayer = true">
      Créer un compte
    </v-btn>
    <v-btn size="x-large" prepend-icon="mdi-cat" append-icon="mdi-cat" variant="outlined" v-else @click="showCreatePlayer = false">
      Retour à la connexion
    </v-btn>
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
            const game = data.game; // Récupère l'objet game
            this.$emit(
              'start-game', 
              game.gameId, 
              game.playerTurn, 
              game.player1HP, 
              game.player2HP, 
              game.player.playerId, 
              game.player_1.playerId, 
              game.turnCount,
              game.player.name,
              game.player_1.name
          ); 
            alert(`Partie lancée avec succès. ID de la partie : ${game.gameId}, ${game.player_1.name}`);
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

<style>
.login-player .v-icon {
  font-size:5rem;
  color:var(--player);
}
.v-text-field input {
  font-size: 3em;
}
.v-label.v-field-label {
  font-size: 3rem;
}
.conteneur-login h1 {
  font-size: 4rem;
  text-transform: uppercase;
}
.login-player {
  width: 60%;
  margin: 2rem;
}

.login-player > div {
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  text-align: center;
  padding: 10px;
  background-color: white;
  display: flex;
  flex-direction: column;
  gap: 3rem;
}
.createplayer {
  --player:gray;
  border: 10px solid var(--player);
}
.logplayer1 {
  --player:blue;
  border: 10px solid var(--player);
}
.logplayer2 {
  --player:red;
  border: 10px solid var(--player);
}
.conteneur-login {
    display: flex;
    flex-direction: column;
    align-items: center;
}
.mdi-cat::before {
  color:var(--player);
}
</style>