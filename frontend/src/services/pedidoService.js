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
      where = null,
      orderBy = null,
    } = params

    const queryParams = {
      startRowIndex,
      maximumRows,
    }

    if (where) queryParams.Where = where
    if (orderBy) queryParams.OrderBy = orderBy

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
