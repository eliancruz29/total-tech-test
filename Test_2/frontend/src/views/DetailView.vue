<template>
  <div class="detail-container">
    <el-card v-loading="loading">
      <template #header>
        <div class="card-header">
          <h3>Requirement Details</h3>
          <el-button @click="goBack">Back</el-button>
        </div>
      </template>

      <div v-if="requirement">
        <!-- Process Info -->
        <div class="section">
          <h2>{{ requirement.nombre }}</h2>
          <p class="meta-info">
            <el-icon><Clock /></el-icon>
            Created: {{ formatDate(requirement.createdAt) }}
          </p>
          <p v-if="requirement.descripcion" class="description">{{ requirement.descripcion }}</p>
        </div>

        <el-divider />

        <!-- Original Requirements -->
        <div class="section">
          <h4><el-icon><Document /></el-icon> Original Requirements</h4>
          <el-card class="original-requirements">
            {{ requirement.requirementText }}
          </el-card>
        </div>

        <el-divider />

        <!-- Subprocesses and Use Cases -->
        <div class="section">
          <h4><el-icon><List /></el-icon> Subprocesses and Use Cases</h4>
          
          <div v-for="(subproceso, index) in requirement.subprocesos" :key="subproceso.idSubproceso" class="subprocess">
            <el-card style="margin-bottom: 20px;">
              <template #header>
                <h5>{{ index + 1 }}. {{ subproceso.nombre }}</h5>
              </template>
              
              <p v-if="subproceso.descripcion" class="subprocess-description">
                {{ subproceso.descripcion }}
              </p>

              <!-- Use Cases -->
              <div v-if="subproceso.casosUso && subproceso.casosUso.length > 0" class="use-cases-section">
                <h6>Use Cases:</h6>
                <el-collapse accordion>
                  <el-collapse-item
                    v-for="(casoUso, cuIndex) in subproceso.casosUso"
                    :key="casoUso.idCasoUso"
                    :name="casoUso.idCasoUso"
                  >
                    <template #title>
                      <div class="use-case-title">
                        <span>{{ index + 1 }}.{{ cuIndex + 1 }} {{ casoUso.nombre }}</span>
                        <el-tag
                          v-if="casoUso.tipoCasoUsoText"
                          :type="getTypeTagType(casoUso.tipoCasoUso)"
                          size="small"
                          style="margin-left: 10px;"
                        >
                          {{ casoUso.tipoCasoUsoText }}
                        </el-tag>
                      </div>
                    </template>

                    <div class="use-case-content">
                      <div v-if="casoUso.descripcion" class="field">
                        <strong>Description:</strong>
                        <p>{{ casoUso.descripcion }}</p>
                      </div>

                      <div v-if="casoUso.actorPrincipal" class="field">
                        <strong>Main Actor:</strong>
                        <p>{{ casoUso.actorPrincipal }}</p>
                      </div>

                      <div v-if="casoUso.precondiciones" class="field">
                        <strong>Preconditions:</strong>
                        <p>{{ casoUso.precondiciones }}</p>
                      </div>

                      <div v-if="casoUso.postcondiciones" class="field">
                        <strong>Postconditions:</strong>
                        <p>{{ casoUso.postcondiciones }}</p>
                      </div>

                      <div v-if="casoUso.criteriosDeAceptacion" class="field">
                        <strong>Acceptance Criteria:</strong>
                        <p>{{ casoUso.criteriosDeAceptacion }}</p>
                      </div>
                    </div>
                  </el-collapse-item>
                </el-collapse>
              </div>
            </el-card>
          </div>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { ElMessage } from 'element-plus'
import { Clock, Document, List } from '@element-plus/icons-vue'
import { requirementService } from '../services/api'

const router = useRouter()
const route = useRoute()
const loading = ref(false)
const requirement = ref(null)

const loadRequirement = async () => {
  loading.value = true
  try {
    const id = route.params.id
    requirement.value = await requirementService.getRequirementById(id)
  } catch (error) {
    console.error('Error loading requirement:', error)
    ElMessage.error('Failed to load requirement details')
    router.push('/history')
  } finally {
    loading.value = false
  }
}

const goBack = () => {
  router.push('/history')
}

const formatDate = (dateString) => {
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
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

onMounted(() => {
  loadRequirement()
})
</script>

<style scoped>
.detail-container {
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

.section {
  margin: 20px 0;
}

.section h2 {
  color: #409eff;
  margin: 0 0 10px 0;
}

.section h4 {
  color: #303133;
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 15px;
}

.meta-info {
  color: #909399;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 5px;
  margin: 5px 0;
}

.description {
  color: #606266;
  font-size: 16px;
  margin-top: 10px;
}

.original-requirements {
  background-color: #f5f7fa;
  white-space: pre-wrap;
  word-wrap: break-word;
}

.subprocess h5 {
  color: #67c23a;
  margin: 0;
}

.subprocess-description {
  color: #606266;
  margin-bottom: 15px;
}

.use-cases-section {
  margin-top: 15px;
}

.use-cases-section h6 {
  color: #606266;
  margin-bottom: 10px;
}

.use-case-title {
  display: flex;
  align-items: center;
  width: 100%;
}

.use-case-content {
  padding: 15px;
  background-color: #fafafa;
  border-radius: 4px;
}

.field {
  margin-bottom: 15px;
}

.field:last-child {
  margin-bottom: 0;
}

.field strong {
  color: #303133;
  display: block;
  margin-bottom: 5px;
}

.field p {
  color: #606266;
  margin: 0;
  line-height: 1.6;
}
</style>

