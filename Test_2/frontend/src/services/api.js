import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:8080'

const api = axios.create({
  baseURL: `${API_BASE_URL}/api`,
  headers: {
    'Content-Type': 'application/json'
  }
})

export const requirementService = {
  async analyzeRequirement(requirementText) {
    const response = await api.post('/requirement/analyze', {
      requirementText
    })
    return response.data
  },

  async getAllRequirements() {
    const response = await api.get('/requirement')
    return response.data
  },

  async getRequirementById(id) {
    const response = await api.get(`/requirement/${id}`)
    return response.data
  }
}

export default api

