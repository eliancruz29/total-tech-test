namespace AdquisicionesAPI.Models.DTOs;

/// <summary>
/// Filter parameters for Pedido queries
/// </summary>
public class PedidoFilterDto
{
    /// <summary>
    /// Filter by year
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Filter by folio (partial match)
    /// </summary>
    public string? Folio { get; set; }

    /// <summary>
    /// Filter by supplier/provider ID
    /// </summary>
    public int? IdProveedor { get; set; }

    /// <summary>
    /// Filter by order status ID
    /// </summary>
    public int? IdEstadoPedido { get; set; }

    /// <summary>
    /// Filter by supply status ID
    /// </summary>
    public int? IdEstadoSurtido { get; set; }

    /// <summary>
    /// Filter by order type ID
    /// </summary>
    public int? IdTipoPedido { get; set; }

    /// <summary>
    /// Filter by date from (inclusive)
    /// </summary>
    public DateOnly? FechaDesde { get; set; }

    /// <summary>
    /// Filter by date to (inclusive)
    /// </summary>
    public DateOnly? FechaHasta { get; set; }

    /// <summary>
    /// Sort field
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction: ASC or DESC
    /// </summary>
    public string? SortDirection { get; set; }
}

