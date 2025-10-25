<template>
  <el-dialog
    v-model="dialogVisible"
    title="Detalle del Pedido"
    width="90%"
    :before-close="handleClose"
  >
    <div v-if="loading" class="loading-container">
      <el-icon class="is-loading" :size="48"><Loading /></el-icon>
      <p>Cargando detalle...</p>
    </div>

    <div v-else-if="pedido" class="pedido-detail">
      <el-row :gutter="24">
        <!-- General Information -->
        <el-col :xs="24" :md="12">
          <h3 class="section-title">Información General</h3>
          <el-descriptions :column="1" border>
            <el-descriptions-item label="Folio">{{ pedido.folio }}</el-descriptions-item>
            <el-descriptions-item label="Fecha">{{ pedido.fechaPedido }}</el-descriptions-item>
            <el-descriptions-item label="Tipo">{{ pedido.tipoPedidoDescripcion }}</el-descriptions-item>
            <el-descriptions-item label="Iniciales">{{ pedido.iniciales }}</el-descriptions-item>
            <el-descriptions-item label="Estado">
              <el-tag :type="getStatusType(pedido.estadoPedidoDescripcion)">
                {{ pedido.estadoPedidoDescripcion }}
              </el-tag>
            </el-descriptions-item>
          </el-descriptions>
        </el-col>

        <!-- Supplier Information -->
        <el-col :xs="24" :md="12">
          <h3 class="section-title">Información del Proveedor</h3>
          <el-descriptions :column="1" border>
            <el-descriptions-item label="Proveedor">{{ pedido.proveedorRazonSocial }}</el-descriptions-item>
            <el-descriptions-item label="R.F.C">{{ pedido.proveedorRfc }}</el-descriptions-item>
            <el-descriptions-item label="Dirección Entrega">
              {{ pedido.direccionEntrega || 'N/A' }}
            </el-descriptions-item>
            <el-descriptions-item label="Tiempo Entrega">
              {{ pedido.tiempoEntrega || 'N/A' }}
            </el-descriptions-item>
          </el-descriptions>
        </el-col>
      </el-row>

      <el-divider />

      <!-- Financial Information -->
      <el-row :gutter="24">
        <el-col :xs="24">
          <h3 class="section-title">Información Financiera</h3>
          <el-row :gutter="16">
            <el-col :xs="24" :sm="12" :md="6">
              <div class="amount-card">
                <div class="amount-label">Subtotal</div>
                <div class="amount-value">{{ formatCurrency(pedido.subtotal) }}</div>
              </div>
            </el-col>
            <el-col :xs="24" :sm="12" :md="6">
              <div class="amount-card">
                <div class="amount-label">IVA</div>
                <div class="amount-value">{{ formatCurrency(pedido.totalIva) }}</div>
              </div>
            </el-col>
            <el-col :xs="24" :sm="12" :md="6">
              <div class="amount-card">
                <div class="amount-label">Retenciones</div>
                <div class="amount-value">{{ formatCurrency(pedido.totalRetenciones) }}</div>
              </div>
            </el-col>
            <el-col :xs="24" :sm="12" :md="6">
              <div class="amount-card total">
                <div class="amount-label">Total</div>
                <div class="amount-value">{{ formatCurrency(pedido.montoTotal) }}</div>
              </div>
            </el-col>
          </el-row>
        </el-col>
      </el-row>

      <el-divider />

      <!-- Order Details/Items -->
      <el-row v-if="pedido.detalles && pedido.detalles.length > 0">
        <el-col :span="24">
          <h3 class="section-title">Detalles del Pedido</h3>
          <el-table :data="pedido.detalles" border stripe>
            <el-table-column prop="numeroPartida" label="Partida" width="100" />
            <el-table-column prop="descripcion" label="Descripción" min-width="200" />
            <el-table-column prop="cantidad" label="Cantidad" width="100" align="right" />
            <el-table-column prop="cantidadSurtida" label="Surtida" width="100" align="right" />
            <el-table-column prop="unidad" label="Unidad" width="80" />
            <el-table-column prop="precioUnitario" label="Precio Unit." width="120" align="right">
              <template #default="scope">
                {{ formatCurrency(scope.row.precioUnitario) }}
              </template>
            </el-table-column>
            <el-table-column prop="total" label="Total" width="120" align="right">
              <template #default="scope">
                {{ formatCurrency(scope.row.total) }}
              </template>
            </el-table-column>
          </el-table>
        </el-col>
      </el-row>

      <el-divider v-if="pedido.observaciones" />

      <!-- Additional Information -->
      <el-row v-if="pedido.observaciones">
        <el-col :span="24">
          <h3 class="section-title">Observaciones</h3>
          <el-alert :title="pedido.observaciones" type="info" :closable="false" />
        </el-col>
      </el-row>
    </div>

    <template #footer>
      <span class="dialog-footer">
        <el-button @click="handleClose">Cerrar</el-button>
        <el-button type="primary" @click="printDetail">
          <el-icon><Printer /></el-icon>
          Imprimir
        </el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script setup>
import { ref, watch, computed } from 'vue'
import { usePedidosStore } from '../stores/pedidos'
import { Loading, Printer } from '@element-plus/icons-vue'

const props = defineProps({
  modelValue: Boolean,
  pedidoId: Number,
})

const emit = defineEmits(['update:modelValue'])

const pedidosStore = usePedidosStore()
const loading = ref(false)
const pedido = ref(null)

const dialogVisible = computed({
  get: () => props.modelValue,
  set: (value) => emit('update:modelValue', value),
})

watch(() => props.pedidoId, async (newId) => {
  if (newId && dialogVisible.value) {
    await loadPedidoDetail(newId)
  }
})

watch(dialogVisible, async (newValue) => {
  if (newValue && props.pedidoId) {
    await loadPedidoDetail(props.pedidoId)
  }
})

async function loadPedidoDetail(id) {
  loading.value = true
  try {
    pedido.value = await pedidosStore.fetchPedidoById(id)
  } catch (error) {
    console.error('Error loading pedido detail:', error)
  } finally {
    loading.value = false
  }
}

function handleClose() {
  dialogVisible.value = false
  pedido.value = null
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

function printDetail() {
  window.print()
}
</script>

<style scoped>
.loading-container {
  text-align: center;
  padding: 60px 20px;
  color: #1a6b9f;
}

.loading-container p {
  margin-top: 16px;
}

.pedido-detail {
  padding: 20px 0;
}

.section-title {
  color: #0d4b76;
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 16px;
}

.amount-card {
  background: #f5f7fa;
  border: 1px solid #e4e7ed;
  border-radius: 8px;
  padding: 16px;
  text-align: center;
  margin-bottom: 16px;
}

.amount-card.total {
  background: linear-gradient(135deg, #0d4b76 0%, #1a6b9f 100%);
  color: white;
  border: none;
}

.amount-label {
  font-size: 14px;
  opacity: 0.8;
  margin-bottom: 8px;
}

.amount-value {
  font-size: 24px;
  font-weight: 700;
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}
</style>
