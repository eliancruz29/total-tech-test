using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdquisicionesAPI.Models.Entities;

[Table("cat_tipo_pedido")]
public class CatTipoPedido
{
    [Key]
    [Column("id_tipo_pedido")]
    public int IdTipoPedido { get; set; }

    [Required]
    [Column("descripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;

    [Column("activo")]
    public bool Activo { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
