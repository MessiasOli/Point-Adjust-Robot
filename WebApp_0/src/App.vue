<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="wrapper">
    <!-- <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/about">About</RouterLink>
    </nav> -->
    <RouterView />
  </div>
</template>

<style scoped>
.wrapper{
  display: grid;
  /* grid-template-columns: 100px calc(100% - 100px); */
}

nav{
  display: grid;
  flex-direction: column;
}

</style>
