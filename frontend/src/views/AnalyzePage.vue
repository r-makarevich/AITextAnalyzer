<template>
  <div class="analyze-page">
    <div class="row justify-content-center">
      <div class="col-lg-8">
        <div class="card shadow">
          <div class="card-header bg-primary text-white">
            <h4 class="mb-0">
              <i class="bi bi-pencil-square"></i> Analyze Text
            </h4>
          </div>
          <div class="card-body">
            <div class="mb-3">
              <label for="textInput" class="form-label">Enter text to analyze:</label>
              <textarea
                id="textInput"
                v-model="textContent"
                class="form-control"
                rows="8"
                placeholder="Type or paste your text here..."
                :disabled="loading"
              ></textarea>
            </div>
            
            <div class="d-grid gap-2">
              <button
                class="btn btn-primary btn-lg"
                @click="analyzeText"
                :disabled="!textContent.trim() || loading"
              >
                <span v-if="loading">
                  <span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                  Analyzing...
                </span>
                <span v-else>
                  <i class="bi bi-cpu"></i> Analyze
                </span>
              </button>
            </div>
            
            <!-- Error Alert -->
            <div v-if="error" class="alert alert-danger mt-3" role="alert">
              <i class="bi bi-exclamation-triangle-fill"></i> {{ error }}
            </div>
            
            <!-- Results -->
            <div v-if="result" class="mt-4">
              <h5 class="mb-3">Analysis Results:</h5>
              <div class="row g-3">
                <div class="col-md-6">
                  <div class="card border-info">
                    <div class="card-body text-center">
                      <h6 class="card-subtitle mb-2 text-muted">Detected Language</h6>
                      <h3 class="card-title text-info">
                        <i class="bi bi-translate"></i> {{ result.language }}
                      </h3>
                    </div>
                  </div>
                </div>
                <div class="col-md-6">
                  <div class="card" :class="getSentimentCardClass()">
                    <div class="card-body text-center">
                      <h6 class="card-subtitle mb-2 text-muted">Sentiment</h6>
                      <h3 class="card-title" :class="getSentimentTextClass()">
                        <i :class="getSentimentIcon()"></i> {{ result.sentimentText }}
                      </h3>
                    </div>
                  </div>
                </div>
              </div>
              
              <div class="card mt-3 bg-light">
                <div class="card-body">
                  <h6 class="card-subtitle mb-2 text-muted">Analyzed Text:</h6>
                  <p class="card-text">{{ result.content }}</p>
                  <small class="text-muted">
                    <i class="bi bi-clock"></i> {{ formatDateTime(result.createdAt) }}
                  </small>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import api from '../services/api'

export default {
  name: 'AnalyzePage',
  setup() {
    const textContent = ref('')
    const result = ref(null)
    const loading = ref(false)
    const error = ref(null)

    const analyzeText = async () => {
      if (!textContent.value.trim()) return
      
      loading.value = true
      error.value = null
      result.value = null
      
      try {
        const response = await api.analyzeText(textContent.value)
        result.value = response
        textContent.value = ''
      } catch (err) {
        error.value = err.response?.data?.message || 'Failed to analyze text. Please try again.'
        console.error('Error analyzing text:', err)
      } finally {
        loading.value = false
      }
    }

    const getSentimentCardClass = () => {
      if (!result.value) return ''
      switch (result.value.sentiment) {
        case 0: // Positive
          return 'border-success'
        case 1: // Negative
          return 'border-danger'
        default:
          return 'border-secondary'
      }
    }

    const getSentimentTextClass = () => {
      if (!result.value) return ''
      switch (result.value.sentiment) {
        case 0: // Positive
          return 'text-success'
        case 1: // Negative
          return 'text-danger'
        default:
          return 'text-secondary'
      }
    }

    const getSentimentIcon = () => {
      if (!result.value) return ''
      switch (result.value.sentiment) {
        case 0: // Positive
          return 'bi bi-emoji-smile-fill'
        case 1: // Negative
          return 'bi bi-emoji-frown-fill'
        default:
          return 'bi bi-emoji-neutral-fill'
      }
    }

    const formatDateTime = (dateString) => {
      const date = new Date(dateString)
      return date.toLocaleString()
    }

    return {
      textContent,
      result,
      loading,
      error,
      analyzeText,
      getSentimentCardClass,
      getSentimentTextClass,
      getSentimentIcon,
      formatDateTime
    }
  }
}
</script>

<style scoped>
.analyze-page {
  padding: 20px 0;
}

.card {
  border-radius: 10px;
}

.card-header {
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
}

textarea.form-control {
  resize: vertical;
}

.spinner-border-sm {
  width: 1rem;
  height: 1rem;
}
</style>
