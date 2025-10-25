<template>
  <header class="app-header">
    <div class="header-content">
      <div class="header-left">
        <h1 class="app-title">Sistema de Adquisiciones CAASIM</h1>
      </div>
      <div class="header-right">
        <el-dropdown trigger="click">
          <span class="user-menu">
            <el-icon><User /></el-icon>
            <span>{{ authStore.user?.username || 'Usuario' }}</span>
            <el-icon class="el-icon--right"><arrow-down /></el-icon>
          </span>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item @click="handleLogout">
                <el-icon><SwitchButton /></el-icon>
                Cerrar Sesión
              </el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </div>
    </div>
  </header>
</template>

<script setup>
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { User, ArrowDown, SwitchButton } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'

const router = useRouter()
const authStore = useAuthStore()

function handleLogout() {
  authStore.logout()
  ElMessage.success('Sesión cerrada exitosamente')
  router.push('/login')
}
</script>

<style scoped>
.app-header {
  background: linear-gradient(135deg, #0d4b76 0%, #1a6b9f 100%);
  color: white;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 1000;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 24px;
  height: 60px;
  max-width: 1920px;
  margin: 0 auto;
}

.header-left {
  display: flex;
  align-items: center;
}

.app-title {
  font-size: 20px;
  font-weight: 600;
  margin: 0;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.user-menu {
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 12px;
  border-radius: 4px;
  transition: background-color 0.3s;
}

.user-menu:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

@media (max-width: 768px) {
  .app-title {
    font-size: 16px;
  }

  .header-content {
    padding: 0 16px;
  }
}
</style>
