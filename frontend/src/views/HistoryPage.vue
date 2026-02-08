<template>
  <div class="history-page">
    <div class="card shadow">
      <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
        <h4 class="mb-0">
          <i class="bi bi-clock-history"></i> Analysis History
        </h4>
        <button 
          class="btn btn-light btn-sm" 
          @click="loadHistory"
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
          <p class="mt-2 text-muted">Loading history...</p>
        </div>

        <!-- Error State -->
        <div v-else-if="error" class="alert alert-danger" role="alert">
          <i class="bi bi-exclamation-triangle-fill"></i> {{ error }}
        </div>

        <!-- Empty State -->
        <div v-else-if="!analyses || analyses.length === 0" class="text-center py-5">
          <i class="bi bi-inbox" style="font-size: 4rem; color: #ccc;"></i>
          <p class="text-muted mt-3">No analysis history yet. Start by analyzing some text!</p>
          <router-link to="/" class="btn btn-primary">
            <i class="bi bi-pencil-square"></i> Analyze Text
          </router-link>
        </div>

        <!-- History Table -->
        <div v-else class="table-responsive">
          <table class="table table-hover">
            <thead class="table-light">
              <tr>
                <th scope="col" style="width: 5%;">#</th>
                <th scope="col" style="width: 30%;">Text Content</th>
                <th scope="col" style="width: 15%;">Language</th>
                <th scope="col" style="width: 15%;">Sentiment</th>
                <th scope="col" style="width: 18%;">Date & Time</th>
                <th scope="col" :style="{ width: isAdmin ? '17%' : '12%' }">Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="analysis in analyses" :key="analysis.id">
                <th scope="row">{{ analysis.id }}</th>
                <td>
                  <div class="text-truncate" style="max-width: 300px;" :title="analysis.content">
                    {{ analysis.content }}
                  </div>
                </td>
                <td>
                  <span class="badge bg-info text-dark">
                    <i class="bi bi-translate"></i> {{ analysis.language }}
                  </span>
                </td>
                <td>
                  <span 
                    class="badge" 
                    :class="getSentimentBadgeClass(analysis.sentiment)"
                  >
                    <i :class="getSentimentIcon(analysis.sentiment)"></i> 
                    {{ analysis.sentimentText }}
                  </span>
                </td>
                <td>
                  <small>{{ formatDateTime(analysis.createdAt) }}</small>
                </td>
                <td>
                  <button 
                    class="btn btn-sm btn-outline-primary me-1"
                    @click="viewDetails(analysis)"
                    data-bs-toggle="modal"
                    data-bs-target="#detailsModal"
                    title="View Details"
                  >
                    <i class="bi bi-eye"></i>
                  </button>
                  <button 
                    v-if="isAdmin"
                    class="btn btn-sm btn-outline-danger"
                    @click="confirmDelete(analysis)"
                    data-bs-toggle="modal"
                    data-bs-target="#deleteModal"
                    title="Delete"
                  >
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
          
          <div class="text-muted text-center mt-3">
            <small>Total analyses: {{ analyses.length }}</small>
          </div>
        </div>
      </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header bg-danger text-white">
            <h5 class="modal-title" id="deleteModalLabel">
              <i class="bi bi-exclamation-triangle"></i> Confirm Delete
            </h5>
            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body" v-if="analysisToDelete">
            <p>Are you sure you want to delete this analysis?</p>
            <div class="card bg-light">
              <div class="card-body">
                <strong>ID:</strong> {{ analysisToDelete.id }}<br>
                <strong>Content:</strong> {{ analysisToDelete.content.substring(0, 100) }}{{ analysisToDelete.content.length > 100 ? '...' : '' }}<br>
                <strong>Date:</strong> {{ formatDateTime(analysisToDelete.createdAt) }}
              </div>
            </div>
            <div class="alert alert-warning mt-3 mb-0">
              <i class="bi bi-info-circle"></i> This action cannot be undone.
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" :disabled="deleting">Cancel</button>
            <button type="button" class="btn btn-danger" @click="deleteAnalysis" :disabled="deleting">
              <span v-if="deleting">
                <span class="spinner-border spinner-border-sm me-2" role="status"></span>
                Deleting...
              </span>
              <span v-else>
                <i class="bi bi-trash"></i> Delete
              </span>
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Details Modal -->
    <div class="modal fade" id="detailsModal" tabindex="-1" aria-labelledby="detailsModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header bg-primary text-white">
            <h5 class="modal-title" id="detailsModalLabel">
              <i class="bi bi-card-text"></i> Analysis Details
            </h5>
            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body" v-if="selectedAnalysis">
            <div class="row mb-3">
              <div class="col-md-6">
                <strong>ID:</strong> {{ selectedAnalysis.id }}
              </div>
              <div class="col-md-6">
                <strong>Date:</strong> {{ formatDateTime(selectedAnalysis.createdAt) }}
              </div>
            </div>
            
            <div class="row mb-3">
              <div class="col-md-6">
                <strong>Language:</strong> 
                <span class="badge bg-info text-dark ms-2">
                  <i class="bi bi-translate"></i> {{ selectedAnalysis.language }}
                </span>
              </div>
              <div class="col-md-6">
                <strong>Sentiment:</strong> 
                <span 
                  class="badge ms-2" 
                  :class="getSentimentBadgeClass(selectedAnalysis.sentiment)"
                >
                  <i :class="getSentimentIcon(selectedAnalysis.sentiment)"></i> 
                  {{ selectedAnalysis.sentimentText }}
                </span>
              </div>
            </div>
            
            <div class="mb-3">
              <strong>Text Content:</strong>
              <div class="card bg-light mt-2">
                <div class="card-body">
                  {{ selectedAnalysis.content }}
                </div>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue'
import api from '../services/api'
import { Modal } from 'bootstrap'

export default {
  name: 'HistoryPage',
  setup() {
    const analyses = ref([])
    const loading = ref(false)
    const error = ref(null)
    const selectedAnalysis = ref(null)
    const analysisToDelete = ref(null)
    const deleting = ref(false)
    
    const isAdmin = computed(() => {
      return localStorage.getItem('isAdmin') === 'true'
    })

    const loadHistory = async () => {
      loading.value = true
      error.value = null
      
      try {
        const response = await api.getAllTexts()
        // Sort by date descending (newest first)
        analyses.value = response.sort((a, b) => 
          new Date(b.createdAt) - new Date(a.createdAt)
        )
      } catch (err) {
        error.value = err.response?.data?.message || 'Failed to load history. Please try again.'
        console.error('Error loading history:', err)
      } finally {
        loading.value = false
      }
    }

    const viewDetails = (analysis) => {
      selectedAnalysis.value = analysis
    }

    const confirmDelete = (analysis) => {
      analysisToDelete.value = analysis
    }

    const deleteAnalysis = async () => {
      if (!analysisToDelete.value) return

      deleting.value = true
      
      try {
        await api.deleteText(analysisToDelete.value.id)
        
        // Remove from local list
        analyses.value = analyses.value.filter(a => a.id !== analysisToDelete.value.id)
        
        // Close and cleanup modal
        const deleteModalElement = document.getElementById('deleteModal')
        let deleteModal = Modal.getInstance(deleteModalElement)
        
        // If no instance exists, create one to properly close it
        if (!deleteModal) {
          deleteModal = new Modal(deleteModalElement)
        }
        
        // Listen for modal hidden event to clean up
        deleteModalElement.addEventListener('hidden.bs.modal', () => {
          // Force remove all backdrops
          document.querySelectorAll('.modal-backdrop').forEach(backdrop => backdrop.remove())
          
          // Clean up body classes and styles
          document.body.classList.remove('modal-open')
          document.body.style.overflow = ''
          document.body.style.paddingRight = ''
          
          // Dispose of the modal instance
          if (deleteModal) {
            deleteModal.dispose()
          }
        }, { once: true })
        
        deleteModal.hide()
        analysisToDelete.value = null
      } catch (err) {
        error.value = err.response?.data?.message || 'Failed to delete analysis. Please try again.'
        console.error('Error deleting analysis:', err)
      } finally {
        deleting.value = false
      }
    }

    const getSentimentBadgeClass = (sentiment) => {
      switch (sentiment) {
        case 0: // Positive
          return 'bg-success'
        case 1: // Negative
          return 'bg-danger'
        default:
          return 'bg-secondary'
      }
    }

    const getSentimentIcon = (sentiment) => {
      switch (sentiment) {
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

    onMounted(() => {
      loadHistory()
    })

    return {
      analyses,
      loading,
      error,
      selectedAnalysis,
      analysisToDelete,
      deleting,
      isAdmin,
      loadHistory,
      viewDetails,
      confirmDelete,
      deleteAnalysis,
      getSentimentBadgeClass,
      getSentimentIcon,
      formatDateTime
    }
  }
}
</script>

<style scoped>
.history-page {
  padding: 20px 0;
}

.card {
  border-radius: 10px;
}

.card-header {
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
}

.table-responsive {
  max-height: 70vh;
  overflow-y: auto;
}

.text-truncate {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.badge {
  font-size: 0.85rem;
}
</style>
