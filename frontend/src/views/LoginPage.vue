<template>
  <div class="login-page">
    <div class="row justify-content-center">
      <div class="col-md-6 col-lg-4">
        <div class="card shadow">
          <div class="card-header bg-primary text-white text-center">
            <h4 class="mb-0">
              <i class="bi bi-shield-lock"></i> Admin Login
            </h4>
          </div>
          <div class="card-body">
            <form @submit.prevent="handleLogin">
              <div class="mb-3">
                <label for="password" class="form-label">Password</label>
                <input 
                  type="password" 
                  class="form-control" 
                  id="password" 
                  v-model="password"
                  placeholder="Enter admin password"
                  :disabled="loading"
                  required
                />
              </div>

              <!-- Error Message -->
              <div v-if="error" class="alert alert-danger" role="alert">
                <i class="bi bi-exclamation-triangle-fill"></i> {{ error }}
              </div>

              <!-- Success Message -->
              <div v-if="success" class="alert alert-success" role="alert">
                <i class="bi bi-check-circle-fill"></i> Login successful! Redirecting...
              </div>

              <div class="d-grid gap-2">
                <button 
                  type="submit" 
                  class="btn btn-primary"
                  :disabled="loading"
                >
                  <span v-if="loading">
                    <span class="spinner-border spinner-border-sm me-2" role="status"></span>
                    Logging in...
                  </span>
                  <span v-else>
                    <i class="bi bi-box-arrow-in-right"></i> Login
                  </span>
                </button>
                <button 
                  type="button" 
                  class="btn btn-outline-secondary"
                  @click="goBack"
                  :disabled="loading"
                >
                  <i class="bi bi-arrow-left"></i> Back
                </button>
              </div>
            </form>

            <div class="mt-4 text-center">
              <small class="text-muted">
                <i class="bi bi-info-circle"></i> Admin access required to delete analysis records
              </small>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

export default {
  name: 'LoginPage',
  setup() {
    const router = useRouter()
    const password = ref('')
    const loading = ref(false)
    const error = ref(null)
    const success = ref(false)

    // Hardcoded admin password
    const ADMIN_PASSWORD = 'admin123'

    const handleLogin = async () => {
      loading.value = true
      error.value = null
      success.value = false

      // Simulate API call delay
      await new Promise(resolve => setTimeout(resolve, 500))

      if (password.value === ADMIN_PASSWORD) {
        // Store admin authentication in localStorage
        localStorage.setItem('isAdmin', 'true')
        localStorage.setItem('adminLoginTime', new Date().toISOString())
        
        success.value = true
        
        // Redirect to history page after successful login
        setTimeout(() => {
          router.push('/history')
        }, 1000)
      } else {
        error.value = 'Invalid password. Please try again.'
        loading.value = false
      }
    }

    const goBack = () => {
      router.back()
    }

    return {
      password,
      loading,
      error,
      success,
      handleLogin,
      goBack
    }
  }
}
</script>

<style scoped>
.login-page {
  padding: 3rem 0;
}

.card {
  border: none;
  border-radius: 10px;
}

.card-header {
  border-radius: 10px 10px 0 0;
  padding: 1.5rem;
}

.card-body {
  padding: 2rem;
}
</style>
