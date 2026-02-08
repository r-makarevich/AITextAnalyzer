import { createRouter, createWebHistory } from 'vue-router'
import AnalyzePage from '../views/AnalyzePage.vue'
import HistoryPage from '../views/HistoryPage.vue'
import StatisticsPage from '../views/StatisticsPage.vue'

const routes = [
  {
    path: '/',
    name: 'Analyze',
    component: AnalyzePage
  },
  {
    path: '/history',
    name: 'History',
    component: HistoryPage
  },
  {
    path: '/statistics',
    name: 'Statistics',
    component: StatisticsPage
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
