<template>
  <div class="pedido-detail-view">
    <el-page-header @back="goBack" title="Detalle del Pedido">
      <template #content>
        <span class="page-title">Pedido #{{ pedidoId }}</span>
      </template>
    </el-page-header>

    <div class="content">
      <PedidoDetailDialog
        v-model="dialogVisible"
        :pedido-id="pedidoId"
      />
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import PedidoDetailDialog from '../components/PedidoDetailDialog.vue'

const route = useRoute()
const router = useRouter()

const pedidoId = computed(() => parseInt(route.params.id))
const dialogVisible = ref(true)

function goBack() {
  router.push('/pedidos')
}
</script>

<style scoped>
.pedido-detail-view {
  padding: 20px;
}

.page-title {
  font-size: 20px;
  font-weight: 600;
  color: #0d4b76;
}

.content {
  margin-top: 20px;
}
</style>
