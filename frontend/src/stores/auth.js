import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authService } from '../services/authService'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null)
  const token = ref(authService.getToken())
  const loading = ref(false)
  const error = ref(null)

  const isAuthenticated = computed(() => !!token.value)

  async function login(username, password) {
    loading.value = true
    error.value = null

    try {
      const response = await authService.login(username, password)
      token.value = response.accessToken
      user.value = { username }
      return true
    } catch (err) {
      error.value = err.response?.data?.error_description || 'Error de autenticación'
      throw err
    } finally {
      loading.value = false
    }
  }

  function logout() {
    authService.logout()
    token.value = null
    user.value = null
  }

  function checkAuth() {
    const savedToken = authService.getToken()
    if (savedToken) {
      token.value = savedToken
      // Optionally decode token to get user info
    }
  }

  return {
    user,
    token,
    loading,
    error,
    isAuthenticated,
    login,
    logout,
    checkAuth,
  }
})
