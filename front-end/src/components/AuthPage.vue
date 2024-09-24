<template>
    <div>
      <h1>Bienvenue</h1>
      <div v-if="!isLoggedIn">
        <h2>{{ isLogin ? 'Connexion' : 'Inscription' }}</h2>
        <form @submit.prevent="handleSubmit">
          <div>
            <label for="name">Pseudo :</label>
            <input type="text" v-model="user.name" required />
          </div>
          <div v-if="!isLogin">
            <label for="password">Mot de passe :</label>
            <input type="password" v-model="user.password" required />
          </div>
          <button type="submit">{{ isLogin ? 'Se connecter' : 'S\'inscrire' }}</button>
        </form>
        <button @click="toggleForm">{{ isLogin ? 'Créer un compte' : 'Se connecter' }}</button>
      </div>
      
      <div v-else>
        <h2>Bienvenue {{ user.name }} !</h2>
        <button @click="startGame">Lancer une partie</button>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        isLogin: true,
        isLoggedIn: false,
        user: {
          name: '',
          password: ''
        }
      };
    },
    methods: {
      toggleForm() {
        this.isLogin = !this.isLogin;
      },
      handleSubmit() {
        // je laisse ceci en dur pour pouvoir continuer mes tests
        if (this.isLogin) {
          this.isLoggedIn = true;
        } else {
          this.isLoggedIn = true;
        }
      },
      startGame() {
        this.$emit('start-game');
      }
    }
  };
  </script>
  