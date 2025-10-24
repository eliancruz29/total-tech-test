using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdquisicionesAPI.Models.Entities;

[Table("spartan_user")]
public class SpartanUser
{
    [Key]
    [Column("id_user")]
    public int IdUser { get; set; }

    [Required]
    [Column("name")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("email")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("password_hash")]
    [MaxLength(500)]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<CatProveedor> ProveedoresRegistrados { get; set; } = new List<CatProveedor>();
    public virtual ICollection<Requisicion> RequisicionesSolicitadas { get; set; } = new List<Requisicion>();
    public virtual ICollection<Pedido> PedidosRegistrados { get; set; } = new List<Pedido>();
}
