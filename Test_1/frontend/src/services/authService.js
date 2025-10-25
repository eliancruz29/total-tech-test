import apiClient from './api'

export const authService = {
  /**
   * Login with username and password
   * @param {string} username - User email
   * @param {string} password - User password
   * @returns {Promise} Token response
   */
  async login(username, password) {
    const params = new URLSearchParams()
    params.append('grant_type', 'password')
    params.append('username', username)
    params.append('password', password)

    const response = await apiClient.get('/oauth/token', {
      params: {
        grant_type: 'password',
        username,
        password,
      },
    })

    if (response.data.accessToken) {
      localStorage.setItem('access_token', response.data.accessToken)
      localStorage.setItem('token_type', response.data.tokenType)
      localStorage.setItem('expires_in', response.data.expiresIn)
    }

    return response.data
  },

  /**
   * Logout user
   */
  logout() {
    localStorage.removeItem('access_token')
    localStorage.removeItem('token_type')
    localStorage.removeItem('expires_in')
    localStorage.removeItem('user')
  },

  /**
   * Check if user is authenticated
   * @returns {boolean}
   */
  isAuthenticated() {
    return !!localStorage.getItem('access_token')
  },

  /**
   * Get current access token
   * @returns {string|null}
   */
  getToken() {
    return localStorage.getItem('access_token')
  },
}
