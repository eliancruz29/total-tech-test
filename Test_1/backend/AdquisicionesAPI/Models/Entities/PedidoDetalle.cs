using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdquisicionesAPI.Models.Entities;

/// <summary>
/// Pedido Detalle - Order line items/details
/// </summary>
[Table("pedido_detalle")]
public class PedidoDetalle
{
    [Key]
    [Column("id_pedido_detalle")]
    public int IdPedidoDetalle { get; set; }

    [Column("id_pedido")]
    public int IdPedido { get; set; }

    [Column("id_requisicion_detalle")]
    [MaxLength(50)]
    public string? IdRequisicionDetalle { get; set; }

    [Column("id_requisicion")]
    public int? IdRequisicion { get; set; }

    [Column("clave_presupuestal")]
    [MaxLength(100)]
    public string? ClavePresupuestal { get; set; }

    [Column("nombre_partida")]
    [MaxLength(300)]
    public string? NombrePartida { get; set; }

    [Column("id_insumo")]
    public int IdInsumo { get; set; }

    [Required]
    [Column("descripcion")]
    [MaxLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Column("numero_partida")]
    [MaxLength(50)]
    public string? NumeroPartida { get; set; }

    [Column("anio")]
    public int? Anio { get; set; }

    [Column("cantidad")]
    [Precision(18, 6)]
    public decimal Cantidad { get; set; }

    [Column("cantidad_surtida")]
    [Precision(18, 6)]
    public decimal? CantidadSurtida { get; set; }

    [Required]
    [Column("unidad")]
    [MaxLength(50)]
    public string Unidad { get; set; } = string.Empty;

    [Column("precio_unitario")]
    [Precision(18, 2)]
    public decimal PrecioUnitario { get; set; }

    [Column("subtotal")]
    [Precision(18, 2)]
    public decimal Subtotal { get; set; }

    [Column("iva")]
    [Precision(18, 6)]
    public decimal Iva { get; set; }

    [Column("retenciones")]
    [Precision(18, 2)]
    public decimal Retenciones { get; set; }

    [Column("total")]
    [Precision(18, 2)]
    public decimal Total { get; set; }

    // Navigation properties
    [ForeignKey("IdPedido")]
    public virtual Pedido? Pedido { get; set; }

    [ForeignKey("IdRequisicion")]
    public virtual Requisicion? Requisicion { get; set; }

    [ForeignKey("IdInsumo")]
    public virtual CatInsumo? Insumo { get; set; }
}
