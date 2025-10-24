using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdquisicionesAPI.Models.Entities;

[Table("cat_proveedor")]
public class CatProveedor
{
    [Key]
    [Column("id_proveedor")]
    public int IdProveedor { get; set; }

    [Required]
    [Column("razon_social")]
    [MaxLength(300)]
    public string RazonSocial { get; set; } = string.Empty;

    [Required]
    [Column("rfc")]
    [MaxLength(13)]
    public string Rfc { get; set; } = string.Empty;

    [Required]
    [Column("tipo_persona")]
    [MaxLength(20)]
    public string TipoPersona { get; set; } = string.Empty;

    [Column("nombre_contacto")]
    [MaxLength(200)]
    public string? NombreContacto { get; set; }

    [Column("correo_electronico")]
    [MaxLength(200)]
    public string? CorreoElectronico { get; set; }

    [Column("telefono")]
    [MaxLength(50)]
    public string? Telefono { get; set; }

    [Column("direccion")]
    [MaxLength(500)]
    public string? Direccion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    [Column("fecha_registro")]
    public DateOnly FechaRegistro { get; set; }

    [Column("id_usuario_registro")]
    public int IdUsuarioRegistro { get; set; }

    // Navigation properties
    [ForeignKey("IdUsuarioRegistro")]
    public virtual SpartanUser? UsuarioRegistro { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
