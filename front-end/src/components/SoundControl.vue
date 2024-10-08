<template>
  <div>
    <v-btn @click="toggleMute" color="primary">
      {{ isMuted ? 'Jouer la playlist' : 'Mettre en sourdine' }}
    </v-btn>
    <v-btn @click="nextTrack" color="secondary">
      Chanson suivante
    </v-btn>
  </div>
</template>

<script>
export default {
  data() {
    return {
      isMuted: true, // La musique commence en sourdine
      audio: null,   // Référence à l'élément audio
      currentTrackIndex: 0, // Index de la chanson actuelle
      tracks: [        // Liste des pistes audio
        require('@/assets/sounds/Meow.mp3'),
        require('@/assets/sounds/Meow2.mp3'),
      ],
    };
  },
  methods: {
    startMusic() {
      if (!this.audio) {
        this.audio = new Audio(this.tracks[this.currentTrackIndex]);
        this.audio.loop = true; // Pour jouer en boucle
        this.audio.muted = this.isMuted; // Met la musique en sourdine par défaut
      }
      this.audio.play().catch(error => {
        console.error('Erreur lors de la lecture de la musique :', error);
      });
    },
    toggleMute() {
      this.isMuted = !this.isMuted; // Change l'état de mute
      if (this.audio) {
        this.audio.muted = this.isMuted; // Mute ou unmute la musique
      } else {
        this.startMusic(); // Démarre la musique si elle n'est pas encore en cours
      }
    },
    nextTrack() {
      if (this.audio) {
        this.currentTrackIndex = (this.currentTrackIndex + 1) % this.tracks.length;
        this.audio.src = this.tracks[this.currentTrackIndex];
        this.audio.play().catch(error => {
          console.error('Erreur lors de la lecture de la musique :', error);
        });
      }
    },
  },
};
</script>

<style scoped>
/* Ajoute des styles supplémentaires ici si nécessaire */
</style>
