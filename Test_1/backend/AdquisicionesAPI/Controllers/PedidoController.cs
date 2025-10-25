using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdquisicionesAPI.Models.DTOs;
using AdquisicionesAPI.Services.Interfaces;

namespace AdquisicionesAPI.Controllers;

[Route("api/pedido")]
[ApiController]
[Authorize]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(IPedidoService pedidoService, ILogger<PedidoController> logger)
    {
        _pedidoService = pedidoService;
        _logger = logger;
    }

    /// <summary>
    /// List all orders with pagination and filters
    /// </summary>
    /// <param name="startRowIndex">Start row index (1-based)</param>
    /// <param name="maximumRows">Maximum rows to return</param>
    /// <param name="year">Filter by year</param>
    /// <param name="folio">Filter by folio (partial match)</param>
    /// <param name="idProveedor">Filter by supplier ID</param>
    /// <param name="idEstadoPedido">Filter by order status ID</param>
    /// <param name="idEstadoSurtido">Filter by supply status ID</param>
    /// <param name="idTipoPedido">Filter by order type ID</param>
    /// <param name="fechaDesde">Filter by date from (YYYY-MM-DD)</param>
    /// <param name="fechaHasta">Filter by date to (YYYY-MM-DD)</param>
    /// <param name="sortBy">Sort field (id_pedido, folio, fecha_pedido, monto_total)</param>
    /// <param name="sortDirection">Sort direction (ASC or DESC)</param>
    [HttpGet("ListaSelAll")]
    [ProducesResponseType(typeof(PedidoListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PedidoListResponse>> ListaSelAll(
        [FromQuery] int startRowIndex = 1,
        [FromQuery] int maximumRows = 10,
        [FromQuery] int? year = null,
        [FromQuery] string? folio = null,
        [FromQuery] int? idProveedor = null,
        [FromQuery] int? idEstadoPedido = null,
        [FromQuery] int? idEstadoSurtido = null,
        [FromQuery] int? idTipoPedido = null,
        [FromQuery] string? fechaDesde = null,
        [FromQuery] string? fechaHasta = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDirection = null)
    {
        try
        {
            _logger.LogInformation("ListaSelAll called: startRowIndex={StartRow}, maximumRows={MaxRows}, year={Year}, folio={Folio}, idProveedor={IdProveedor}, idEstadoSurtido={IdEstadoSurtido}",
                startRowIndex, maximumRows, year, folio, idProveedor, idEstadoSurtido);

            // Build filter DTO from query parameters
            var filters = new PedidoFilterDto
            {
                Year = year,
                Folio = folio,
                IdProveedor = idProveedor,
                IdEstadoPedido = idEstadoPedido,
                IdEstadoSurtido = idEstadoSurtido,
                IdTipoPedido = idTipoPedido,
                FechaDesde = !string.IsNullOrEmpty(fechaDesde) ? DateOnly.Parse(fechaDesde) : null,
                FechaHasta = !string.IsNullOrEmpty(fechaHasta) ? DateOnly.Parse(fechaHasta) : null,
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            var result = await _pedidoService.GetAllAsync(startRowIndex, maximumRows, filters);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ListaSelAll");
            return StatusCode(500, new { error = "internal_server_error", message = ex.Message });
        }
    }

    /// <summary>
    /// Get a single order by ID
    /// </summary>
    /// <param name="id">Order ID</param>
    [HttpGet("Get")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PedidoDto>> Get([FromQuery] int id)
    {
        try
        {
            _logger.LogInformation("Get called for ID: {Id}", id);

            var pedido = await _pedidoService.GetByIdAsync(id);

            if (pedido == null)
            {
                return NotFound(new { error = "not_found", message = $"Pedido with ID {id} not found" });
            }

            return Ok(pedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Get for ID: {Id}", id);
            return StatusCode(500, new { error = "internal_server_error", message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="pedidoDto">Order data</param>
    [HttpPost("Post")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PedidoDto>> Post([FromBody] PedidoDto pedidoDto)
    {
        try
        {
            _logger.LogInformation("Post called to create new Pedido: {Folio}", pedidoDto.Folio);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPedido = await _pedidoService.CreateAsync(pedidoDto);

            return CreatedAtAction(nameof(Get), new { id = createdPedido.IdPedido }, createdPedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Post");
            return StatusCode(500, new { error = "internal_server_error", message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing order
    /// </summary>
    /// <param name="Id">Order ID</param>
    /// <param name="pedidoDto">Updated order data</param>
    [HttpPut("Put")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PedidoDto>> Put([FromQuery] int Id, [FromBody] PedidoDto pedidoDto)
    {
        try
        {
            _logger.LogInformation("Put called for ID: {Id}", Id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPedido = await _pedidoService.UpdateAsync(Id, pedidoDto);

            if (updatedPedido == null)
            {
                return NotFound(new { error = "not_found", message = $"Pedido with ID {Id} not found" });
            }

            return Ok(updatedPedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Put for ID: {Id}", Id);
            return StatusCode(500, new { error = "internal_server_error", message = ex.Message });
        }
    }

    /// <summary>
    /// Delete an order
    /// </summary>
    /// <param name="id">Order ID</param>
    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        try
        {
            _logger.LogInformation("Delete called for ID: {Id}", id);

            var deleted = await _pedidoService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { error = "not_found", message = $"Pedido with ID {id} not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Delete for ID: {Id}", id);
            return StatusCode(500, new { error = "internal_server_error", message = ex.Message });
        }
    }
}
