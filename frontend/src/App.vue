<template>
  <div id="app">
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
      <div class="container">
        <router-link class="navbar-brand" to="/">
          <i class="bi bi-cpu-fill"></i> AI Text Analyzer
        </router-link>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav ms-auto">
            <li class="nav-item">
              <router-link class="nav-link" to="/" exact-active-class="active">
                Analyze
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/history" active-class="active">
                History
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/statistics" active-class="active">
                Statistics
              </router-link>
            </li>
            <li class="nav-item" v-if="!isAdmin">
              <router-link class="nav-link" to="/login">
                <i class="bi bi-shield-lock"></i> Admin Login
              </router-link>
            </li>
            <li class="nav-item" v-if="isAdmin">
              <span class="nav-link text-warning">
                <i class="bi bi-shield-check"></i> Admin
              </span>
            </li>
            <li class="nav-item" v-if="isAdmin">
              <button class="btn btn-outline-light btn-sm ms-2" @click="handleLogout">
                <i class="bi bi-box-arrow-right"></i> Logout
              </button>
            </li>
          </ul>
        </div>
      </div>
    </nav>
    
    <main class="container my-4">
      <router-view :key="$route.fullPath" />
    </main>
    
    <footer class="bg-light text-center py-3 mt-5">
      <div class="container">
        <small class="text-muted">© 2026 AI Text Analyzer - Powered by ML.NET</small>
      </div>
    </footer>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

export default {
  name: 'App',
  setup() {
    const router = useRouter()
    const isAdmin = ref(false)

    const checkAdminStatus = () => {
      isAdmin.value = localStorage.getItem('isAdmin') === 'true'
    }

    const handleLogout = () => {
      localStorage.removeItem('isAdmin')
      localStorage.removeItem('adminLoginTime')
      isAdmin.value = false
      router.push('/')
    }

    onMounted(() => {
      checkAdminStatus()
      // Check admin status on storage change (e.g., from login page)
      window.addEventListener('storage', checkAdminStatus)
      // Also check on focus in case localStorage was updated in same tab
      setInterval(checkAdminStatus, 1000)
    })

    return {
      isAdmin,
      handleLogout
    }
  }
}
</script>

<style>
#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

main {
  flex: 1;
}

.router-link-exact-active,
.router-link-active {
  font-weight: bold;
}
</style>
