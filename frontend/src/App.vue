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
          </ul>
        </div>
      </div>
    </nav>
    
    <main class="container my-4">
      <router-view />
    </main>
    
    <footer class="bg-light text-center py-3 mt-5">
      <div class="container">
        <small class="text-muted">© 2026 AI Text Analyzer - Powered by ML.NET</small>
      </div>
    </footer>
  </div>
</template>

<script>
export default {
  name: 'App'
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
