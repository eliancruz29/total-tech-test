namespace AdquisicionesAPI.Models.DTOs;

/// <summary>
/// DTO for Pedido entity - matches Postman collection structure
/// </summary>
public class PedidoDto
{
    public int? IdPedido { get; set; }
    public string Folio { get; set; } = string.Empty;
    public string? ConsecutivoCompleto { get; set; }
    public int IdTipoDocumentoPedido { get; set; }
    public int? IdTipoPedido { get; set; }
    public int IdProveedor { get; set; }
    public int? IdProcedimientoAdquisicion { get; set; }
    public string FechaPedido { get; set; } = string.Empty;
    public string? NumeroContrato { get; set; }
    public string? DestinatarioFactura { get; set; }
    public string? DireccionEntrega { get; set; }
    public string? FechaEntrega { get; set; }
    public string? TiempoEntrega { get; set; }
    public string? PersonaElaboro { get; set; }
    public string? PersonaAutorizo { get; set; }
    public string? Iniciales { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TotalIva { get; set; }
    public decimal TotalRetenciones { get; set; }
    public decimal MontoTotal { get; set; }
    public int IdEstadoPedido { get; set; }
    public int? IdEstadoSurtido { get; set; }
    public string? Observaciones { get; set; }
    public string FechaRegistro { get; set; } = string.Empty;
    public string HoraRegistro { get; set; } = string.Empty;
    public int IdUsuarioRegistro { get; set; }
    public string? FechaModifica { get; set; }
    public string? HoraModifica { get; set; }
    public int? IdUsuarioModifica { get; set; }
    public string? FechaAprueba { get; set; }
    public string? HoraAprueba { get; set; }
    public int? IdUsuarioAprueba { get; set; }
    public int? IdArchivoFirma { get; set; }

    // Related data for display
    public string? TipoDocumentoPedidoDescripcion { get; set; }
    public string? TipoPedidoDescripcion { get; set; }
    public string? ProveedorRazonSocial { get; set; }
    public string? ProveedorRfc { get; set; }
    public string? EstadoPedidoDescripcion { get; set; }
    public string? EstadoSurtidoDescripcion { get; set; }

    public List<PedidoDetalleDto>? Detalles { get; set; }
}

public class PedidoDetalleDto
{
    public int? IdPedidoDetalle { get; set; }
    public int IdPedido { get; set; }
    public string? IdRequisicionDetalle { get; set; }
    public int? IdRequisicion { get; set; }
    public string? ClavePresupuestal { get; set; }
    public string? NombrePartida { get; set; }
    public int IdInsumo { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? NumeroPartida { get; set; }
    public int? Anio { get; set; }
    public decimal Cantidad { get; set; }
    public decimal? CantidadSurtida { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Iva { get; set; }
    public decimal Retenciones { get; set; }
    public decimal Total { get; set; }
}

public class PedidoListDto
{
    public int IdPedido { get; set; }
    public string Folio { get; set; } = string.Empty;
    public string FechaPedido { get; set; } = string.Empty;
    public string? TipoPedido { get; set; }
    public string? Iniciales { get; set; }
    public string ProveedorRazonSocial { get; set; } = string.Empty;
    public string ProveedorRfc { get; set; } = string.Empty;
    public decimal MontoTotal { get; set; }
    public string EstadoPedido { get; set; } = string.Empty;
    public string? EstadoSurtido { get; set; }
    public string? Observaciones { get; set; }
}

public class PedidoListResponse
{
    public List<PedidoListDto> Data { get; set; } = new();
    public int TotalRecords { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
