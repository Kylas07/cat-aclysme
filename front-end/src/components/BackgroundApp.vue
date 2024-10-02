<template>
    <div class="backgroundApp">
      <div
      v-for="(icon, index) in icons"
      :key="index"
      :class="['mdi', 'mdi-cat', 'icon']"
      :style="{ color: icon.color, left: icon.left + '%', animationDuration: icon.duration + 's', animationDelay: icon.delay + 's' }"
    ></div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'BackgroundApp',
    data() {
    return {
      icons: Array.from({ length: 20 }, () => ({
        color: this.getRandomColor(),
        left: Math.random() * 100, // Positionnement horizontal aléatoire
        duration: 5 + Math.random() * 10, // Durée de l'animation entre 5 et 15 secondes
        delay: Math.random() * 5, // Délai avant le démarrage de l'animation (jusqu'à 5 secondes)
      })),
    };
  },
  methods: {
    getRandomColor() {
      const letters = '0123456789ABCDEF';
      let color = '#';
      for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
      }
      return color;
    },
  },
  }
  </script>
  
  <style scoped>
  .backgroundApp {
    position: absolute;
    top: -1rem;
    left: 0;
    width: 100%;
    height: 100%;
    overflow: hidden;
    z-index: -1;
  }
  
  .icon {
    position: absolute;
    font-size: 50px;
    color: rgba(0, 0, 0, 0.2);
    opacity:0;
    animation: slide infinite;
  }
  
  .icon:nth-child(1) { animation-duration: 8s; animation-delay: 0s; left: 10%; }
  .icon:nth-child(2) { animation-duration: 10s; animation-delay: 0s; left: 20%; }
  .icon:nth-child(3) { animation-duration: 7s; animation-delay: 0s; left: 30%; }
  .icon:nth-child(4) { animation-duration: 9s; animation-delay: 0s; left: 40%; }
  .icon:nth-child(5) { animation-duration: 11s; animation-delay: 1s; left: 50%; }
  .icon:nth-child(6) { animation-duration: 5s; animation-delay: 1s; left: 60%; }
  .icon:nth-child(7) { animation-duration: 14s; animation-delay: 1s; left: 70%; }
  .icon:nth-child(8) { animation-duration: 6s; animation-delay: 0s; left: 80%; }
  .icon:nth-child(9) { animation-duration: 8s; animation-delay: 0s; left: 90%; }
  .icon:nth-child(10) { animation-duration: 4s; animation-delay: 0s; left: 100%; }

  
  @keyframes slide {
    0% {
      top: -10%;
      opacity:1;
    }
    100% {
      top: 110%;
    }
  }
  </style>
  