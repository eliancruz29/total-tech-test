<template>
  <div id="app-container">
    <AppHeader v-if="showHeader" />
    <main class="main-content">
      <router-view />
    </main>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from './stores/auth'
import AppHeader from './components/AppHeader.vue'

const route = useRoute()
const authStore = useAuthStore()

// Initialize auth check
authStore.checkAuth()

// Show header only when authenticated and not on login page
const showHeader = computed(() => {
  return authStore.isAuthenticated && route.path !== '/login'
})
</script>

<style scoped>
#app-container {
  width: 100%;
  min-height: 100vh;
  background-color: #f5f7fa;
}

.main-content {
  width: 100%;
  min-height: calc(100vh - 60px);
}
</style>
