using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdquisicionesAPI.Models.Entities;

[Table("cat_insumo")]
public class CatInsumo
{
    [Key]
    [Column("id_insumo")]
    public int IdInsumo { get; set; }

    [Required]
    [Column("codigo")]
    [MaxLength(50)]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    [Column("nombre")]
    [MaxLength(300)]
    public string Nombre { get; set; } = string.Empty;

    [Column("descripcion")]
    [MaxLength(1000)]
    public string? Descripcion { get; set; }

    [Column("id_unidad_medida")]
    public int? IdUnidadMedida { get; set; }

    [Column("id_categoria_insumo")]
    public int? IdCategoriaInsumo { get; set; }

    [Column("precio_referencia")]
    [Precision(18, 2)]
    public decimal? PrecioReferencia { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    [Column("fecha_registro")]
    public DateOnly FechaRegistro { get; set; }

    [Column("id_usuario_registro")]
    public int IdUsuarioRegistro { get; set; }

    // Navigation properties
    [ForeignKey("IdUsuarioRegistro")]
    public virtual SpartanUser? UsuarioRegistro { get; set; }

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
