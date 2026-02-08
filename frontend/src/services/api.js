import axios from 'axios'

const API_BASE_URL = 'http://localhost:5196/api'

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

export default {
  async analyzeText(content) {
    const response = await apiClient.post('/TextAnalysis/analyze', JSON.stringify(content), {
      headers: {
        'Content-Type': 'application/json'
      }
    })
    return response.data
  },

  async getAllTexts() {
    const response = await apiClient.get('/TextAnalysis/all')
    return response.data
  },

  async deleteText(id) {
    const response = await apiClient.delete(`/TextAnalysis/${id}`)
    return response.data
  }
}
