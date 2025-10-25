<template>
  <div class="login-container">
    <el-card class="login-card">
      <template #header>
        <div class="card-header">
          <h1>Sistema de Adquisiciones</h1>
          <p>CAASIM</p>
        </div>
      </template>

      <el-form
        ref="loginFormRef"
        :model="loginForm"
        :rules="rules"
        label-position="top"
        @submit.prevent="handleLogin"
      >
        <el-form-item label="Usuario (Email)" prop="username">
          <el-input
            v-model="loginForm.username"
            placeholder="admin@caasim.gob.mx"
            size="large"
            :prefix-icon="User"
          />
        </el-form-item>

        <el-form-item label="Contraseña" prop="password">
          <el-input
            v-model="loginForm.password"
            type="password"
            placeholder="Ingrese su contraseña"
            size="large"
            :prefix-icon="Lock"
            show-password
            @keyup.enter="handleLogin"
          />
        </el-form-item>

        <el-form-item>
          <el-button
            type="primary"
            size="large"
            :loading="authStore.loading"
            @click="handleLogin"
            style="width: 100%"
          >
            Iniciar Sesión
          </el-button>
        </el-form-item>

        <el-alert
          v-if="authStore.error"
          :title="authStore.error"
          type="error"
          :closable="false"
          style="margin-top: 16px"
        />
      </el-form>

      <div class="login-info">
        <el-divider />
        <p><strong>Usuarios de prueba:</strong></p>
        <p>admin@caasim.gob.mx / admin123</p>
        <p>jgarcia@caasim.gob.mx / admin123</p>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { User, Lock } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'

const router = useRouter()
const authStore = useAuthStore()
const loginFormRef = ref()

const loginForm = reactive({
  username: '',
  password: '',
})

const rules = {
  username: [
    { required: true, message: 'Por favor ingrese su usuario', trigger: 'blur' },
    { type: 'email', message: 'Por favor ingrese un email válido', trigger: 'blur' },
  ],
  password: [
    { required: true, message: 'Por favor ingrese su contraseña', trigger: 'blur' },
    { min: 3, message: 'La contraseña debe tener al menos 3 caracteres', trigger: 'blur' },
  ],
}

const handleLogin = async () => {
  if (!loginFormRef.value) return

  await loginFormRef.value.validate(async (valid) => {
    if (valid) {
      try {
        await authStore.login(loginForm.username, loginForm.password)
        ElMessage.success('Inicio de sesión exitoso')
        router.push('/pedidos')
      } catch (error) {
        ElMessage.error(authStore.error || 'Error al iniciar sesión')
      }
    }
  })
}
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: linear-gradient(135deg, #0d4b76 0%, #1a6b9f 100%);
  padding: 20px;
}

.login-card {
  width: 100%;
  max-width: 450px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.2);
}

.card-header {
  text-align: center;
}

.card-header h1 {
  margin: 0 0 8px 0;
  color: #0d4b76;
  font-size: 28px;
  font-weight: 600;
}

.card-header p {
  margin: 0;
  color: #1a6b9f;
  font-size: 16px;
  font-weight: 500;
}

.login-info {
  margin-top: 20px;
  text-align: center;
  color: #666;
  font-size: 14px;
}

.login-info p {
  margin: 4px 0;
}
</style>
