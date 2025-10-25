import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/LoginView.vue'),
    meta: { requiresAuth: false },
  },
  {
    path: '/',
    name: 'Home',
    redirect: '/pedidos',
  },
  {
    path: '/pedidos',
    name: 'Pedidos',
    component: () => import('../views/PedidosView.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/pedidos/:id',
    name: 'PedidoDetail',
    component: () => import('../views/PedidoDetailView.vue'),
    meta: { requiresAuth: true },
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

// Navigation guard
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)

  if (requiresAuth && !authStore.isAuthenticated) {
    next('/login')
  } else if (to.path === '/login' && authStore.isAuthenticated) {
    next('/pedidos')
  } else {
    next()
  }
})

export default router
