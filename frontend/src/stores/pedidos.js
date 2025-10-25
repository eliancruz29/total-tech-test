import { defineStore } from 'pinia'
import { ref } from 'vue'
import { pedidoService } from '../services/pedidoService'

export const usePedidosStore = defineStore('pedidos', () => {
  const pedidos = ref([])
  const currentPedido = ref(null)
  const loading = ref(false)
  const error = ref(null)
  const totalRecords = ref(0)
  const currentPage = ref(1)
  const pageSize = ref(10)

  // Filters
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

  async function fetchPedidos(params = {}) {
    loading.value = true
    error.value = null

    try {
      const queryParams = {
        startRowIndex: params.startRowIndex || (currentPage.value - 1) * pageSize.value + 1,
        maximumRows: params.maximumRows || pageSize.value,
        where: params.where || buildWhereClause(),
        orderBy: params.orderBy || 'pedido.fecha_pedido DESC',
      }

      const response = await pedidoService.getAll(queryParams)
      pedidos.value = response.data
      totalRecords.value = response.totalRecords
      currentPage.value = response.pageNumber
      pageSize.value = response.pageSize

      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al cargar pedidos'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchPedidoById(id) {
    loading.value = true
    error.value = null

    try {
      const response = await pedidoService.getById(id)
      currentPedido.value = response
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al cargar pedido'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function createPedido(pedido) {
    loading.value = true
    error.value = null

    try {
      const response = await pedidoService.create(pedido)
      await fetchPedidos() // Refresh list
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al crear pedido'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updatePedido(id, pedido) {
    loading.value = true
    error.value = null

    try {
      const response = await pedidoService.update(id, pedido)
      await fetchPedidos() // Refresh list
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al actualizar pedido'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deletePedido(id) {
    loading.value = true
    error.value = null

    try {
      await pedidoService.delete(id)
      await fetchPedidos() // Refresh list
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al eliminar pedido'
      throw err
    } finally {
      loading.value = false
    }
  }

  function buildWhereClause() {
    const conditions = []

    if (filters.value.status) {
      // This would need to map status names to IDs
      // For now, assuming status is an ID
      conditions.push(`pedido.id_estado_pedido=${filters.value.status}`)
    }

    // Add more filter conditions as needed
    return conditions.length > 0 ? conditions.join(' AND ') : null
  }

  function setFilters(newFilters) {
    filters.value = { ...filters.value, ...newFilters }
  }

  function clearFilters() {
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
  }

  function setPage(page) {
    currentPage.value = page
  }

  function setPageSize(size) {
    pageSize.value = size
    currentPage.value = 1 // Reset to first page
  }

  return {
    pedidos,
    currentPedido,
    loading,
    error,
    totalRecords,
    currentPage,
    pageSize,
    filters,
    fetchPedidos,
    fetchPedidoById,
    createPedido,
    updatePedido,
    deletePedido,
    setFilters,
    clearFilters,
    setPage,
    setPageSize,
  }
})
