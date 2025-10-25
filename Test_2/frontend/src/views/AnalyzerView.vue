<template>
  <div class="analyzer-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <h3>Analyze Software Requirements</h3>
          <el-button type="primary" @click="goToHistory">View History</el-button>
        </div>
      </template>

      <el-form :model="form" label-position="top">
        <el-form-item label="Enter your software requirements:">
          <el-input
            v-model="form.requirementText"
            type="textarea"
            :rows="8"
            placeholder="Describe your software requirements here. For example: 'Create a user management system with login, registration, and profile management features...'"
          />
        </el-form-item>

        <el-form-item>
          <el-button
            type="primary"
            :loading="loading"
            @click="analyzeRequirements"
            :disabled="!form.requirementText.trim()"
            style="width: 100%;"
          >
            {{ loading ? 'Analyzing...' : 'Analyze Requirements' }}
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <!-- Results Section -->
    <el-card v-if="result" style="margin-top: 20px;">
      <template #header>
        <div class="card-header">
          <h3>Analysis Results</h3>
          <el-tag type="success">Saved to Database</el-tag>
        </div>
      </template>

      <div class="result-content">
        <!-- Proceso -->
        <div class="section">
          <h4>Process: {{ result.nombre }}</h4>
          <p v-if="result.descripcion">{{ result.descripcion }}</p>
        </div>

        <!-- Subprocesos -->
        <div v-for="subproceso in result.subprocesos" :key="subproceso.idSubproceso" class="subprocess-section">
          <el-divider />
          <h5><el-icon><List /></el-icon> {{ subproceso.nombre }}</h5>
          <p v-if="subproceso.descripcion">{{ subproceso.descripcion }}</p>

          <!-- Casos de Uso -->
          <div v-if="subproceso.casosUso && subproceso.casosUso.length > 0" class="use-cases">
            <h6>Use Cases:</h6>
            <el-collapse>
              <el-collapse-item
                v-for="casoUso in subproceso.casosUso"
                :key="casoUso.idCasoUso"
                :title="casoUso.nombre"
              >
                <div class="use-case-details">
                  <p v-if="casoUso.descripcion"><strong>Description:</strong> {{ casoUso.descripcion }}</p>
                  <p v-if="casoUso.actorPrincipal"><strong>Main Actor:</strong> {{ casoUso.actorPrincipal }}</p>
                  <p v-if="casoUso.tipoCasoUsoText">
                    <strong>Type:</strong>
                    <el-tag :type="getTypeTagType(casoUso.tipoCasoUso)" size="small">
                      {{ casoUso.tipoCasoUsoText }}
                    </el-tag>
                  </p>
                  <p v-if="casoUso.precondiciones"><strong>Preconditions:</strong> {{ casoUso.precondiciones }}</p>
                  <p v-if="casoUso.postcondiciones"><strong>Postconditions:</strong> {{ casoUso.postcondiciones }}</p>
                  <p v-if="casoUso.criteriosDeAceptacion"><strong>Acceptance Criteria:</strong> {{ casoUso.criteriosDeAceptacion }}</p>
                </div>
              </el-collapse-item>
            </el-collapse>
          </div>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { List } from '@element-plus/icons-vue'
import { requirementService } from '../services/api'

const router = useRouter()
const form = ref({
  requirementText: ''
})
const loading = ref(false)
const result = ref(null)

const analyzeRequirements = async () => {
  if (!form.value.requirementText.trim()) {
    ElMessage.warning('Please enter requirements text')
    return
  }

  loading.value = true
  result.value = null

  try {
    const response = await requirementService.analyzeRequirement(form.value.requirementText)
    result.value = response
    ElMessage.success('Requirements analyzed and saved successfully!')
  } catch (error) {
    console.error('Error analyzing requirements:', error)
    ElMessage.error(error.response?.data?.error || 'Failed to analyze requirements')
  } finally {
    loading.value = false
  }
}

const goToHistory = () => {
  router.push('/history')
}

const getTypeTagType = (tipo) => {
  switch (tipo) {
    case 1:
      return 'success'
    case 2:
      return 'warning'
    case 3:
      return 'info'
    default:
      return ''
  }
}
</script>

<style scoped>
.analyzer-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header h3 {
  margin: 0;
}

.result-content {
  padding: 10px 0;
}

.section {
  margin-bottom: 20px;
}

.section h4 {
  color: #409eff;
  margin-bottom: 10px;
}

.subprocess-section {
  margin: 20px 0;
}

.subprocess-section h5 {
  color: #67c23a;
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 10px;
}

.use-cases {
  margin-top: 15px;
  padding-left: 20px;
}

.use-cases h6 {
  margin-bottom: 10px;
  color: #606266;
}

.use-case-details {
  padding: 10px;
  background-color: #f5f7fa;
  border-radius: 4px;
}

.use-case-details p {
  margin: 8px 0;
}

.use-case-details strong {
  color: #303133;
}
</style>

