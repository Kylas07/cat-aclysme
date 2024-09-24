<template>
    <div>
      <h2>Lancer une partie</h2>
      <button @click="startGame">Lancer la partie</button>
    </div>
  </template>
  
  <script>
  export default {
    methods: {
      async startGame() {
        try {
          const response = await fetch('LE BACK-END ENCORE UNE FOIS', {
            method: 'POST'
          });
          if (response.ok) {
            alert('Partie lancée avec succès');
          } else {
            alert('Erreur lors du lancement de la partie');
          }
        } catch (error) {
          console.error('Erreur:', error);
        }
      }
    }
  };
  </script>
  