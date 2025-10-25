import apiClient from './api'

export const pedidoService = {
  /**
   * Get all pedidos with filters and pagination
   * @param {Object} params - Query parameters
   * @returns {Promise}
   */
  async getAll(params = {}) {
    const {
      startRowIndex = 1,
      maximumRows = 10,
      year = null,
      folio = null,
      idProveedor = null,
      idEstadoPedido = null,
      idEstadoSurtido = null,
      idTipoPedido = null,
      fechaDesde = null,
      fechaHasta = null,
      sortBy = null,
      sortDirection = null,
    } = params

    const queryParams = {
      startRowIndex,
      maximumRows,
    }

    // Add optional filter parameters
    if (year) queryParams.year = year
    if (folio) queryParams.folio = folio
    if (idProveedor) queryParams.idProveedor = idProveedor
    if (idEstadoPedido) queryParams.idEstadoPedido = idEstadoPedido
    if (idEstadoSurtido) queryParams.idEstadoSurtido = idEstadoSurtido
    if (idTipoPedido) queryParams.idTipoPedido = idTipoPedido
    if (fechaDesde) queryParams.fechaDesde = fechaDesde
    if (fechaHasta) queryParams.fechaHasta = fechaHasta
    if (sortBy) queryParams.sortBy = sortBy
    if (sortDirection) queryParams.sortDirection = sortDirection

    const response = await apiClient.get('/api/pedido/ListaSelAll', {
      params: queryParams,
    })

    return response.data
  },

  /**
   * Get single pedido by ID
   * @param {number} id - Pedido ID
   * @returns {Promise}
   */
  async getById(id) {
    const response = await apiClient.get('/api/pedido/Get', {
      params: { id },
    })
    return response.data
  },

  /**
   * Create new pedido
   * @param {Object} pedido - Pedido data
   * @returns {Promise}
   */
  async create(pedido) {
    const response = await apiClient.post('/api/pedido/Post', pedido)
    return response.data
  },

  /**
   * Update existing pedido
   * @param {number} id - Pedido ID
   * @param {Object} pedido - Updated pedido data
   * @returns {Promise}
   */
  async update(id, pedido) {
    const response = await apiClient.put(`/api/pedido/Put?Id=${id}`, pedido)
    return response.data
  },

  /**
   * Delete pedido
   * @param {number} id - Pedido ID
   * @returns {Promise}
   */
  async delete(id) {
    const response = await apiClient.delete('/api/pedido/Delete', {
      params: { id },
    })
    return response.data
  },
}
