using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdquisicionesAPI.Models.Entities;

[Table("cat_estado_surtido")]
public class CatEstadoSurtido
{
    [Key]
    [Column("id_estado_surtido")]
    public int IdEstadoSurtido { get; set; }

    [Required]
    [Column("descripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;

    [Column("color_badge")]
    [MaxLength(20)]
    public string? ColorBadge { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
