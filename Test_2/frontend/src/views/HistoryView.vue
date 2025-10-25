<template>
  <div class="history-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <h3>Requirements History</h3>
          <el-button type="primary" @click="goToAnalyzer">New Analysis</el-button>
        </div>
      </template>

      <div v-loading="loading">
        <el-empty v-if="!loading && requirements.length === 0" description="No requirements found" />

        <div v-else class="requirements-list">
          <el-card
            v-for="req in requirements"
            :key="req.idProceso"
            shadow="hover"
            class="requirement-card"
            @click="viewDetail(req.idProceso)"
          >
            <div class="requirement-header">
              <h4>{{ req.nombre }}</h4>
              <el-tag type="info" size="small">{{ formatDate(req.createdAt) }}</el-tag>
            </div>
            <p class="requirement-description">{{ req.descripcion || 'No description' }}</p>
            <div class="requirement-stats">
              <el-tag size="small">{{ req.subprocesos?.length || 0 }} Subprocesses</el-tag>
              <el-tag size="small" type="success">
                {{ getTotalUseCases(req) }} Use Cases
              </el-tag>
            </div>
            <el-divider style="margin: 10px 0;" />
            <div class="requirement-text">
              <strong>Original Requirements:</strong>
              <p>{{ truncateText(req.requirementText, 200) }}</p>
            </div>
          </el-card>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { requirementService } from '../services/api'

const router = useRouter()
const loading = ref(false)
const requirements = ref([])

const loadRequirements = async () => {
  loading.value = true
  try {
    requirements.value = await requirementService.getAllRequirements()
  } catch (error) {
    console.error('Error loading requirements:', error)
    ElMessage.error('Failed to load requirements')
  } finally {
    loading.value = false
  }
}

const goToAnalyzer = () => {
  router.push('/')
}

const viewDetail = (id) => {
  router.push(`/detail/${id}`)
}

const formatDate = (dateString) => {
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const getTotalUseCases = (req) => {
  if (!req.subprocesos) return 0
  return req.subprocesos.reduce((total, sub) => total + (sub.casosUso?.length || 0), 0)
}

const truncateText = (text, maxLength) => {
  if (!text) return ''
  return text.length > maxLength ? text.substring(0, maxLength) + '...' : text
}

onMounted(() => {
  loadRequirements()
})
</script>

<style scoped>
.history-container {
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

.requirements-list {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.requirement-card {
  cursor: pointer;
  transition: transform 0.2s;
}

.requirement-card:hover {
  transform: translateY(-2px);
}

.requirement-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.requirement-header h4 {
  margin: 0;
  color: #409eff;
}

.requirement-description {
  color: #606266;
  margin: 10px 0;
}

.requirement-stats {
  display: flex;
  gap: 10px;
  margin: 10px 0;
}

.requirement-text {
  margin-top: 10px;
}

.requirement-text strong {
  color: #303133;
  display: block;
  margin-bottom: 5px;
}

.requirement-text p {
  color: #606266;
  font-size: 14px;
  margin: 0;
}
</style>

