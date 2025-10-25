<template>
  <div class="pedidos-container">
    <!-- Page Header -->
    <div class="page-header">
      <h1 class="page-title">
        <el-icon><Search /></el-icon>
        Consulta de Pedidos
      </h1>
      <p class="page-subtitle">
        Consulte y analice todas las órdenes de pedido de compras con información detallada y filtros avanzados
      </p>
    </div>

    <!-- Filters Section -->
    <el-card class="filters-section" shadow="hover">
      <template #header>
        <div class="section-header">
          <h3>
            <el-icon><Filter /></el-icon>
            Filtros de Búsqueda
          </h3>
        </div>
      </template>

      <el-form :model="filters" label-position="top">
        <el-row :gutter="16">
          <el-col :xs="24" :sm="12" :md="6">
            <el-form-item label="Año *">
              <el-select v-model="filters.year" placeholder="Seleccione un año" size="large" style="width: 100%">
                <el-option label="2024" value="2024" />
                <el-option label="2023" value="2023" />
                <el-option label="2022" value="2022" />
                <el-option label="2021" value="2021" />
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :xs="24" :sm="12" :md="6">
            <el-form-item label="Folio">
              <el-input
                v-model="filters.folio"
                placeholder="Ej: PED-2024-001"
                size="large"
                clearable
              />
            </el-form-item>
          </el-col>

          <el-col :xs="24" :sm="12" :md="6">
            <el-form-item label="Proveedor">
              <el-select
                v-model="filters.supplier"
                placeholder="Todos los proveedores"
                size="large"
                clearable
                style="width: 100%"
              >
                <el-option label="Todos los proveedores" value="" />
                <el-option label="Distribuidora Industrial del Centro" value="1" />
                <el-option label="Suministros Técnicos Hidalgo" value="2" />
                <el-option label="Comercializadora de Equipos Gubernamentales" value="3" />
                <el-option label="Papelería y Suministros Oficina Total" value="4" />
                <el-option label="Constructora y Servicios Múltiples del Estado" value="5" />
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :xs="24" :sm="12" :md="6">
            <el-form-item label="Estado">
              <el-select
                v-model="filters.status"
                placeholder="Todos los estados"
                size="large"
                clearable
                style="width: 100%"
              >
                <el-option label="Todos los estados" value="" />
                <el-option label="Completo" value="COMPLETO" />
                <el-option label="Pendiente" value="PENDIENTE" />
                <el-option label="Parcial" value="PARCIAL" />
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :xs="24" :sm="12" :md="6">
            <el-form-item label="Fecha Desde">
              <el-date-picker
                v-model="filters.dateFrom"
                type="date"
                placeholder="Fecha desde"
                size="large"
                style="width: 100%"
                format="YYYY-MM-DD"
                value-format="YYYY-MM-DD"
              />
            </el-form-item>
          </el-col>

          <el-col :xs="24" :sm="12" :md="6">
            <el-form-item label="Fecha Hasta">
              <el-date-picker
                v-model="filters.dateTo"
                type="date"
                placeholder="Fecha hasta"
                size="large"
                style="width: 100%"
                format="YYYY-MM-DD"
                value-format="YYYY-MM-DD"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <div class="filter-actions">
              <el-button type="primary" size="large" @click="handleSearch" :loading="pedidosStore.loading">
                <el-icon><Search /></el-icon>
                Buscar
              </el-button>
              <el-button size="large" @click="handleClearFilters">
                <el-icon><RefreshLeft /></el-icon>
                Limpiar Filtros
              </el-button>
            </div>
          </el-col>
        </el-row>
      </el-form>
    </el-card>

    <!-- Statistics Cards -->
    <el-row v-if="showStats" :gutter="20" class="stats-section">
      <el-col :xs="24" :sm="12" :md="6">
        <el-card class="stat-card" shadow="hover">
          <div class="stat-number">{{ stats.totalOrders }}</div>
          <div class="stat-label">Total de Pedidos</div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6">
        <el-card class="stat-card" shadow="hover">
          <div class="stat-number">{{ formatCurrency(stats.totalAmount) }}</div>
          <div class="stat-label">Monto Total</div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6">
        <el-card class="stat-card" shadow="hover">
          <div class="stat-number">{{ stats.completedOrders }}</div>
          <div class="stat-label">Pedidos Completos</div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6">
        <el-card class="stat-card" shadow="hover">
          <div class="stat-number">{{ stats.pendingOrders }}</div>
          <div class="stat-label">Pedidos Pendientes</div>
        </el-card>
      </el-col>
    </el-row>

    <!-- Results Section -->
    <el-card class="results-section" shadow="hover">
      <template #header>
        <div class="results-header">
          <h3>
            <el-icon><List /></el-icon>
            Resultados de la Consulta
          </h3>
          <div class="export-buttons">
            <el-button type="success" @click="exportToExcel" :disabled="!hasPedidos">
              <el-icon><Download /></el-icon>
              Excel
            </el-button>
            <el-button type="warning" @click="exportToPDF" :disabled="!hasPedidos">
              <el-icon><Document /></el-icon>
              PDF
            </el-button>
            <el-button type="info" @click="printTable" :disabled="!hasPedidos">
              <el-icon><Printer /></el-icon>
              Imprimir
            </el-button>
          </div>
        </div>
      </template>

      <!-- Loading State -->
      <div v-if="pedidosStore.loading" class="loading-state">
        <el-icon class="is-loading" :size="48"><Loading /></el-icon>
        <p>Buscando pedidos...</p>
      </div>

      <!-- No Results -->
      <el-empty
        v-else-if="!hasPedidos && searched"
        description="No se encontraron resultados"
        :image-size="200"
      >
        <template #extra>
          <el-button type="primary" @click="handleClearFilters">
            Limpiar Filtros
          </el-button>
        </template>
      </el-empty>

      <!-- Initial Message -->
      <el-alert
        v-else-if="!searched"
        title="Seleccione un año y haga clic en 'Buscar' para consultar los pedidos"
        type="info"
        :closable="false"
        show-icon
      />

      <!-- Results Table -->
      <div v-else class="table-container">
        <el-table
          :data="pedidosStore.pedidos"
          style="width: 100%"
          stripe
          border
          :default-sort="{ prop: 'fechaPedido', order: 'descending' }"
        >
          <el-table-column prop="folio" label="Folio" min-width="150" fixed />
          <el-table-column prop="fechaPedido" label="Fecha Pedido" min-width="120" sortable />
          <el-table-column prop="tipoPedido" label="Tipo" min-width="100" />
          <el-table-column prop="iniciales" label="Iniciales" min-width="100" />
          <el-table-column prop="proveedorRazonSocial" label="Proveedor" min-width="250" show-overflow-tooltip />
          <el-table-column prop="proveedorRfc" label="R.F.C" min-width="130" />
          <el-table-column prop="montoTotal" label="Monto Total" min-width="130" sortable>
            <template #default="scope">
              {{ formatCurrency(scope.row.montoTotal) }}
            </template>
          </el-table-column>
          <el-table-column prop="estadoPedido" label="Estado Pedido" min-width="120">
            <template #default="scope">
              <el-tag :type="getStatusType(scope.row.estadoPedido)">
                {{ scope.row.estadoPedido }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column prop="estadoSurtido" label="Estado Surtido" min-width="130">
            <template #default="scope">
              <el-tag v-if="scope.row.estadoSurtido" :type="getSurtidoType(scope.row.estadoSurtido)">
                {{ scope.row.estadoSurtido }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column label="Acciones" min-width="120" fixed="right">
            <template #default="scope">
              <el-button
                type="primary"
                size="small"
                @click="viewDetail(scope.row.idPedido)"
                circle
              >
                <el-icon><View /></el-icon>
              </el-button>
            </template>
          </el-table-column>
        </el-table>

        <!-- Pagination -->
        <div class="pagination-container">
          <el-pagination
            v-model:current-page="currentPage"
            v-model:page-size="pageSize"
            :page-sizes="[10, 25, 50, 100]"
            :total="pedidosStore.totalRecords"
            layout="total, sizes, prev, pager, next, jumper"
            @size-change="handleSizeChange"
            @current-change="handlePageChange"
          />
        </div>
      </div>
    </el-card>

    <!-- Detail Dialog -->
    <PedidoDetailDialog
      v-model="detailDialogVisible"
      :pedido-id="selectedPedidoId"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { usePedidosStore } from '../stores/pedidos'
import {
  Search,
  Filter,
  List,
  Download,
  Document,
  Printer,
  RefreshLeft,
  Loading,
  View,
} from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'
import PedidoDetailDialog from '../components/PedidoDetailDialog.vue'
import * as XLSX from 'xlsx'

const pedidosStore = usePedidosStore()

const filters = ref({
  year: new Date().getFullYear().toString(),
  folio: '',
  supplier: '',
  budgetKey: '',
  orderType: '',
  dateFrom: `${new Date().getFullYear()}-01-01`,
  dateTo: `${new Date().getFullYear()}-12-31`,
  status: '',
})

const searched = ref(false)
const showStats = ref(false)
const detailDialogVisible = ref(false)
const selectedPedidoId = ref(null)

const currentPage = ref(1)
const pageSize = ref(10)

const hasPedidos = computed(() => pedidosStore.pedidos.length > 0)

const stats = computed(() => {
  const pedidos = pedidosStore.pedidos
  return {
    totalOrders: pedidos.length,
    totalAmount: pedidos.reduce((sum, p) => sum + p.montoTotal, 0),
    completedOrders: pedidos.filter(p => p.estadoSurtido === 'Completo').length,
    pendingOrders: pedidos.filter(p => p.estadoSurtido === 'Pendiente').length,
  }
})

async function handleSearch() {
  try {
    pedidosStore.setFilters(filters.value)
    await pedidosStore.fetchPedidos({
      startRowIndex: 1,
      maximumRows: pageSize.value,
    })
    searched.value = true
    showStats.value = true
    ElMessage.success(`Se encontraron ${pedidosStore.totalRecords} pedidos`)
  } catch (error) {
    ElMessage.error('Error al buscar pedidos')
  }
}

function handleClearFilters() {
  filters.value = {
    year: new Date().getFullYear().toString(),
    folio: '',
    supplier: '',
    budgetKey: '',
    orderType: '',
    dateFrom: `${new Date().getFullYear()}-01-01`,
    dateTo: `${new Date().getFullYear()}-12-31`,
    status: '',
  }
  pedidosStore.clearFilters()
}

async function handlePageChange(page) {
  currentPage.value = page
  await pedidosStore.fetchPedidos({
    startRowIndex: (page - 1) * pageSize.value + 1,
    maximumRows: pageSize.value,
  })
}

async function handleSizeChange(size) {
  pageSize.value = size
  currentPage.value = 1
  await pedidosStore.fetchPedidos({
    startRowIndex: 1,
    maximumRows: size,
  })
}

function viewDetail(id) {
  selectedPedidoId.value = id
  detailDialogVisible.value = true
}

function formatCurrency(value) {
  return new Intl.NumberFormat('es-MX', {
    style: 'currency',
    currency: 'MXN',
  }).format(value)
}

function getStatusType(status) {
  const types = {
    'Completo': 'success',
    'Completado': 'success',
    'Aprobado': 'success',
    'Pendiente': 'warning',
    'En Proceso': 'info',
    'Cancelado': 'danger',
  }
  return types[status] || 'info'
}

function getSurtidoType(status) {
  const types = {
    'Completo': 'success',
    'Pendiente': 'warning',
    'Parcial': 'info',
  }
  return types[status] || 'info'
}

function exportToExcel() {
  try {
    const data = pedidosStore.pedidos.map(p => ({
      'Folio': p.folio,
      'Fecha': p.fechaPedido,
      'Tipo': p.tipoPedido,
      'Iniciales': p.iniciales,
      'Proveedor': p.proveedorRazonSocial,
      'RFC': p.proveedorRfc,
      'Monto Total': p.montoTotal,
      'Estado Pedido': p.estadoPedido,
      'Estado Surtido': p.estadoSurtido,
      'Observaciones': p.observaciones,
    }))

    const ws = XLSX.utils.json_to_sheet(data)
    const wb = XLSX.utils.book_new()
    XLSX.utils.book_append_sheet(wb, ws, 'Pedidos')
    XLSX.writeFile(wb, `Pedidos_${new Date().toISOString().split('T')[0]}.xlsx`)
    ElMessage.success('Archivo Excel generado correctamente')
  } catch (error) {
    ElMessage.error('Error al exportar a Excel')
  }
}

function exportToPDF() {
  ElMessage.info('Funcionalidad de exportación a PDF en desarrollo')
  // Would implement jsPDF here
}

function printTable() {
  window.print()
}

onMounted(() => {
  // Check if user is authenticated
  pedidosStore.checkAuth?.()
})
</script>

<style scoped>
.pedidos-container {
  padding: 20px;
  background-color: #f5f7fa;
  min-height: 100vh;
  width: 100%;
}

.page-header {
  background: linear-gradient(135deg, #0d4b76 0%, #1a6b9f 100%);
  color: white;
  padding: 30px;
  border-radius: 12px;
  margin-bottom: 24px;
  box-shadow: 0 4px 15px rgba(13, 75, 118, 0.3);
}

.page-title {
  font-size: 2rem;
  font-weight: 600;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 15px;
}

.page-subtitle {
  font-size: 1.1rem;
  opacity: 0.9;
  margin: 10px 0 0 0;
}

.filters-section {
  margin-bottom: 24px;
}

.section-header h3 {
  margin: 0;
  display: flex;
  align-items: center;
  gap: 8px;
  color: #0d4b76;
}

.filter-actions {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
}

.stats-section {
  margin-bottom: 24px;
}

.stat-card {
  background: linear-gradient(135deg, #0d4b76 0%, #1a6b9f 100%);
  color: white;
  text-align: center;
  cursor: pointer;
  transition: transform 0.3s;
}

.stat-card:hover {
  transform: translateY(-4px);
}

.stat-number {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: 8px;
}

.stat-label {
  font-size: 0.95rem;
  opacity: 0.9;
}

.results-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 16px;
}

.results-header h3 {
  margin: 0;
  display: flex;
  align-items: center;
  gap: 8px;
  color: #0d4b76;
}

.export-buttons {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.loading-state {
  text-align: center;
  padding: 60px 20px;
  color: #1a6b9f;
}

.loading-state p {
  margin-top: 16px;
  font-size: 16px;
}

.table-container {
  margin-top: 20px;
}

.pagination-container {
  margin-top: 20px;
  display: flex;
  justify-content: center;
}

@media print {
  .page-header,
  .filters-section,
  .stats-section,
  .export-buttons,
  .pagination-container {
    display: none !important;
  }
}

@media (max-width: 768px) {
  .page-title {
    font-size: 1.5rem;
  }

  .results-header {
    flex-direction: column;
    align-items: stretch;
  }

  .export-buttons {
    justify-content: center;
  }
}
</style>
