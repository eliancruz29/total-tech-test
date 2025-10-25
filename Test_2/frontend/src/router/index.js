import { createRouter, createWebHistory } from 'vue-router'
import AnalyzerView from '../views/AnalyzerView.vue'
import HistoryView from '../views/HistoryView.vue'
import DetailView from '../views/DetailView.vue'

const routes = [
  {
    path: '/',
    name: 'Analyzer',
    component: AnalyzerView
  },
  {
    path: '/history',
    name: 'History',
    component: HistoryView
  },
  {
    path: '/detail/:id',
    name: 'Detail',
    component: DetailView
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router

