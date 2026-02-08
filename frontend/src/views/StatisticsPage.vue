<template>
  <div class="statistics-page">
    <div class="card shadow">
      <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
        <h4 class="mb-0">
          <i class="bi bi-bar-chart-fill"></i> Statistics & Analytics
        </h4>
        <button 
          class="btn btn-light btn-sm" 
          @click="loadStatistics"
          :disabled="loading"
        >
          <i class="bi bi-arrow-clockwise"></i> Refresh
        </button>
      </div>
      <div class="card-body">
        <!-- Loading State -->
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
          <p class="mt-2 text-muted">Loading statistics...</p>
        </div>

        <!-- Error State -->
        <div v-else-if="error" class="alert alert-danger" role="alert">
          <i class="bi bi-exclamation-triangle-fill"></i> {{ error }}
        </div>

        <!-- Empty State -->
        <div v-else-if="!analyses || analyses.length === 0" class="text-center py-5">
          <i class="bi bi-pie-chart" style="font-size: 4rem; color: #ccc;"></i>
          <p class="text-muted mt-3">No data available yet. Start analyzing text to see statistics!</p>
          <router-link to="/" class="btn btn-primary">
            <i class="bi bi-pencil-square"></i> Analyze Text
          </router-link>
        </div>

        <!-- Statistics Content -->
        <div v-else>
          <!-- Summary Cards -->
          <div class="row mb-4">
            <div class="col-md-3">
              <div class="card border-primary">
                <div class="card-body text-center">
                  <h6 class="card-subtitle mb-2 text-muted">Total Analyses</h6>
                  <h2 class="card-title text-primary mb-0">{{ totalAnalyses }}</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-success">
                <div class="card-body text-center">
                  <h6 class="card-subtitle mb-2 text-muted">Positive</h6>
                  <h2 class="card-title text-success mb-0">{{ sentimentCounts.positive }}</h2>
                  <small class="text-muted">{{ sentimentPercentages.positive }}%</small>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-danger">
                <div class="card-body text-center">
                  <h6 class="card-subtitle mb-2 text-muted">Negative</h6>
                  <h2 class="card-title text-danger mb-0">{{ sentimentCounts.negative }}</h2>
                  <small class="text-muted">{{ sentimentPercentages.negative }}%</small>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-info">
                <div class="card-body text-center">
                  <h6 class="card-subtitle mb-2 text-muted">Languages</h6>
                  <h2 class="card-title text-info mb-0">{{ uniqueLanguages }}</h2>
                </div>
              </div>
            </div>
          </div>

          <!-- Charts -->
          <div class="row">
            <!-- Sentiment Distribution Chart -->
            <div class="col-md-6 mb-4">
              <div class="card h-100">
                <div class="card-header bg-light">
                  <h5 class="mb-0">
                    <i class="bi bi-emoji-smile"></i> Sentiment Distribution
                  </h5>
                </div>
                <div class="card-body d-flex align-items-center justify-content-center">
                  <div style="max-width: 350px; max-height: 350px;">
                    <Doughnut :data="sentimentChartData" :options="chartOptions" />
                  </div>
                </div>
              </div>
            </div>

            <!-- Language Distribution Chart -->
            <div class="col-md-6 mb-4">
              <div class="card h-100">
                <div class="card-header bg-light">
                  <h5 class="mb-0">
                    <i class="bi bi-translate"></i> Language Distribution
                  </h5>
                </div>
                <div class="card-body d-flex align-items-center justify-content-center">
                  <div style="max-width: 350px; max-height: 350px;">
                    <Doughnut :data="languageChartData" :options="chartOptions" />
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Detailed Breakdown -->
          <div class="row">
            <div class="col-md-6">
              <div class="card">
                <div class="card-header bg-light">
                  <h5 class="mb-0">Sentiment Breakdown</h5>
                </div>
                <div class="card-body">
                  <div class="mb-3">
                    <div class="d-flex justify-content-between mb-1">
                      <span><i class="bi bi-emoji-smile-fill text-success"></i> Positive</span>
                      <strong>{{ sentimentPercentages.positive }}%</strong>
                    </div>
                    <div class="progress">
                      <div class="progress-bar bg-success" :style="{ width: sentimentPercentages.positive + '%' }"></div>
                    </div>
                  </div>
                  <div class="mb-3">
                    <div class="d-flex justify-content-between mb-1">
                      <span><i class="bi bi-emoji-frown-fill text-danger"></i> Negative</span>
                      <strong>{{ sentimentPercentages.negative }}%</strong>
                    </div>
                    <div class="progress">
                      <div class="progress-bar bg-danger" :style="{ width: sentimentPercentages.negative + '%' }"></div>
                    </div>
                  </div>
                  <div class="mb-0">
                    <div class="d-flex justify-content-between mb-1">
                      <span><i class="bi bi-emoji-neutral-fill text-secondary"></i> Neutral</span>
                      <strong>{{ sentimentPercentages.neutral }}%</strong>
                    </div>
                    <div class="progress">
                      <div class="progress-bar bg-secondary" :style="{ width: sentimentPercentages.neutral + '%' }"></div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-md-6">
              <div class="card">
                <div class="card-header bg-light">
                  <h5 class="mb-0">Language Breakdown</h5>
                </div>
                <div class="card-body">
                  <div v-for="(count, language) in languageCounts" :key="language" class="mb-3">
                    <div class="d-flex justify-content-between mb-1">
                      <span><i class="bi bi-flag-fill text-info"></i> {{ language }}</span>
                      <strong>{{ getLanguagePercentage(count) }}%</strong>
                    </div>
                    <div class="progress">
                      <div class="progress-bar bg-info" :style="{ width: getLanguagePercentage(count) + '%' }"></div>
                    </div>
                  </div>
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
import { ref, computed, onMounted } from 'vue'
import { Doughnut } from 'vue-chartjs'
import {
  Chart as ChartJS,
  ArcElement,
  Tooltip,
  Legend
} from 'chart.js'
import api from '../services/api'

ChartJS.register(ArcElement, Tooltip, Legend)

export default {
  name: 'StatisticsPage',
  components: {
    Doughnut
  },
  setup() {
    const analyses = ref([])
    const loading = ref(false)
    const error = ref(null)

    const loadStatistics = async () => {
      loading.value = true
      error.value = null
      
      try {
        const response = await api.getAllTexts()
        analyses.value = response
      } catch (err) {
        error.value = err.response?.data?.message || 'Failed to load statistics. Please try again.'
        console.error('Error loading statistics:', err)
      } finally {
        loading.value = false
      }
    }

    const totalAnalyses = computed(() => analyses.value.length)

    const sentimentCounts = computed(() => {
      const counts = { positive: 0, negative: 0, neutral: 0 }
      analyses.value.forEach(analysis => {
        switch (analysis.sentiment) {
          case 0:
            counts.positive++
            break
          case 1:
            counts.negative++
            break
          default:
            counts.neutral++
        }
      })
      return counts
    })

    const sentimentPercentages = computed(() => {
      const total = totalAnalyses.value
      if (total === 0) return { positive: 0, negative: 0, neutral: 0 }
      
      return {
        positive: Math.round((sentimentCounts.value.positive / total) * 100),
        negative: Math.round((sentimentCounts.value.negative / total) * 100),
        neutral: Math.round((sentimentCounts.value.neutral / total) * 100)
      }
    })

    const languageCounts = computed(() => {
      const counts = {}
      analyses.value.forEach(analysis => {
        const lang = analysis.language
        counts[lang] = (counts[lang] || 0) + 1
      })
      // Sort by count descending
      return Object.fromEntries(
        Object.entries(counts).sort(([,a], [,b]) => b - a)
      )
    })

    const uniqueLanguages = computed(() => Object.keys(languageCounts.value).length)

    const getLanguagePercentage = (count) => {
      const total = totalAnalyses.value
      if (total === 0) return 0
      return Math.round((count / total) * 100)
    }

    const sentimentChartData = computed(() => ({
      labels: ['Positive', 'Negative', 'Neutral'],
      datasets: [{
        data: [
          sentimentCounts.value.positive,
          sentimentCounts.value.negative,
          sentimentCounts.value.neutral
        ],
        backgroundColor: [
          'rgba(25, 135, 84, 0.8)',    // Green
          'rgba(220, 53, 69, 0.8)',     // Red
          'rgba(108, 117, 125, 0.8)'    // Gray
        ],
        borderColor: [
          'rgba(25, 135, 84, 1)',
          'rgba(220, 53, 69, 1)',
          'rgba(108, 117, 125, 1)'
        ],
        borderWidth: 2
      }]
    }))

    const languageChartData = computed(() => {
      const languages = Object.keys(languageCounts.value)
      const counts = Object.values(languageCounts.value)
      
      // Generate colors for languages
      const colors = [
        'rgba(13, 110, 253, 0.8)',   // Blue
        'rgba(255, 193, 7, 0.8)',    // Yellow
        'rgba(220, 53, 69, 0.8)',    // Red
        'rgba(25, 135, 84, 0.8)',    // Green
        'rgba(111, 66, 193, 0.8)',   // Purple
        'rgba(13, 202, 240, 0.8)',   // Cyan
        'rgba(255, 99, 132, 0.8)',   // Pink
        'rgba(54, 162, 235, 0.8)',   // Light Blue
        'rgba(255, 159, 64, 0.8)',   // Orange
        'rgba(153, 102, 255, 0.8)'   // Lavender
      ]
      
      return {
        labels: languages,
        datasets: [{
          data: counts,
          backgroundColor: colors.slice(0, languages.length),
          borderColor: colors.slice(0, languages.length).map(c => c.replace('0.8', '1')),
          borderWidth: 2
        }]
      }
    })

    const chartOptions = {
      responsive: true,
      maintainAspectRatio: true,
      plugins: {
        legend: {
          position: 'bottom',
          labels: {
            padding: 15,
            font: {
              size: 12
            }
          }
        },
        tooltip: {
          callbacks: {
            label: function(context) {
              const label = context.label || ''
              const value = context.parsed || 0
              const total = context.dataset.data.reduce((a, b) => a + b, 0)
              const percentage = Math.round((value / total) * 100)
              return `${label}: ${value} (${percentage}%)`
            }
          }
        }
      }
    }

    onMounted(() => {
      loadStatistics()
    })

    return {
      analyses,
      loading,
      error,
      loadStatistics,
      totalAnalyses,
      sentimentCounts,
      sentimentPercentages,
      languageCounts,
      uniqueLanguages,
      getLanguagePercentage,
      sentimentChartData,
      languageChartData,
      chartOptions
    }
  }
}
</script>

<style scoped>
.statistics-page {
  padding: 20px 0;
}

.card {
  border-radius: 10px;
}

.card-header {
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
}

.progress {
  height: 25px;
}

.progress-bar {
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
}
</style>
