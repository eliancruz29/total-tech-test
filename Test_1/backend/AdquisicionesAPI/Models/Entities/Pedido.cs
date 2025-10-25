using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdquisicionesAPI.Models.Entities;

/// <summary>
/// Pedido (Order) - Main entity for the system
/// </summary>
[Table("pedido")]
public class Pedido
{
    [Key]
    [Column("id_pedido")]
    public int IdPedido { get; set; }

    [Required]
    [Column("folio")]
    [MaxLength(50)]
    public string Folio { get; set; } = string.Empty;

    [Column("consecutivo_completo")]
    [MaxLength(100)]
    public string? ConsecutivoCompleto { get; set; }

    [Column("id_tipo_documento_pedido")]
    public int IdTipoDocumentoPedido { get; set; }

    [Column("id_tipo_pedido")]
    public int? IdTipoPedido { get; set; }

    [Column("id_proveedor")]
    public int IdProveedor { get; set; }

    [Column("id_procedimiento_adquisicion")]
    public int? IdProcedimientoAdquisicion { get; set; }

    [Column("fecha_pedido")]
    public DateOnly FechaPedido { get; set; }

    [Column("numero_contrato")]
    [MaxLength(100)]
    public string? NumeroContrato { get; set; }

    [Column("destinatario_factura")]
    [MaxLength(300)]
    public string? DestinatarioFactura { get; set; }

    [Column("direccion_entrega")]
    [MaxLength(500)]
    public string? DireccionEntrega { get; set; }

    [Column("fecha_entrega")]
    public DateOnly? FechaEntrega { get; set; }

    [Column("tiempo_entrega")]
    [MaxLength(100)]
    public string? TiempoEntrega { get; set; }

    [Column("persona_elaboro")]
    [MaxLength(200)]
    public string? PersonaElaboro { get; set; }

    [Column("persona_autorizo")]
    [MaxLength(200)]
    public string? PersonaAutorizo { get; set; }

    [Column("iniciales")]
    [MaxLength(50)]
    public string? Iniciales { get; set; }

    [Column("subtotal")]
    [Precision(18, 2)]
    public decimal Subtotal { get; set; }

    [Column("total_iva")]
    [Precision(18, 2)]
    public decimal TotalIva { get; set; }

    [Column("total_retenciones")]
    [Precision(18, 2)]
    public decimal TotalRetenciones { get; set; }

    [Column("monto_total")]
    [Precision(18, 2)]
    public decimal MontoTotal { get; set; }

    [Column("id_estado_pedido")]
    public int IdEstadoPedido { get; set; }

    [Column("id_estado_surtido")]
    public int? IdEstadoSurtido { get; set; }

    [Column("observaciones")]
    [MaxLength(2000)]
    public string? Observaciones { get; set; }

    [Column("fecha_registro")]
    public DateOnly FechaRegistro { get; set; }

    [Column("hora_registro")]
    public TimeOnly HoraRegistro { get; set; }

    [Column("id_usuario_registro")]
    public int IdUsuarioRegistro { get; set; }

    [Column("fecha_modifica")]
    public DateOnly? FechaModifica { get; set; }

    [Column("hora_modifica")]
    public TimeOnly? HoraModifica { get; set; }

    [Column("id_usuario_modifica")]
    public int? IdUsuarioModifica { get; set; }

    [Column("fecha_aprueba")]
    public DateOnly? FechaAprueba { get; set; }

    [Column("hora_aprueba")]
    public TimeOnly? HoraAprueba { get; set; }

    [Column("id_usuario_aprueba")]
    public int? IdUsuarioAprueba { get; set; }

    [Column("id_archivo_firma")]
    public int? IdArchivoFirma { get; set; }

    // Navigation properties
    [ForeignKey("IdTipoDocumentoPedido")]
    public virtual CatTipoDocumentoPedido? TipoDocumentoPedido { get; set; }

    [ForeignKey("IdTipoPedido")]
    public virtual CatTipoPedido? TipoPedido { get; set; }

    [ForeignKey("IdProveedor")]
    public virtual CatProveedor? Proveedor { get; set; }

    [ForeignKey("IdProcedimientoAdquisicion")]
    public virtual ProcedimientoAdquisicion? ProcedimientoAdquisicion { get; set; }

    [ForeignKey("IdEstadoPedido")]
    public virtual CatEstadoPedido? EstadoPedido { get; set; }

    [ForeignKey("IdEstadoSurtido")]
    public virtual CatEstadoSurtido? EstadoSurtido { get; set; }

    [ForeignKey("IdUsuarioRegistro")]
    public virtual SpartanUser? UsuarioRegistro { get; set; }

    [ForeignKey("IdUsuarioModifica")]
    public virtual SpartanUser? UsuarioModifica { get; set; }

    [ForeignKey("IdUsuarioAprueba")]
    public virtual SpartanUser? UsuarioAprueba { get; set; }

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
