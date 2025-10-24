using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdquisicionesAPI.Models.Entities;

[Table("procedimiento_adquisicion")]
public class ProcedimientoAdquisicion
{
    [Key]
    [Column("id_procedimiento_adquisicion")]
    public int IdProcedimientoAdquisicion { get; set; }

    [Required]
    [Column("identificador")]
    [MaxLength(50)]
    public string Identificador { get; set; } = string.Empty;

    [Required]
    [Column("prefijo")]
    [MaxLength(5)]
    public string Prefijo { get; set; } = string.Empty;

    [Column("consecutivo")]
    public int Consecutivo { get; set; }

    [Column("anio")]
    public int Anio { get; set; }

    [Column("descripcion")]
    [MaxLength(1000)]
    public string? Descripcion { get; set; }

    [Column("id_modalidad_compra")]
    public int IdModalidadCompra { get; set; }

    [Column("id_estado_procedimiento")]
    public int IdEstadoProcedimiento { get; set; }

    [Column("monto_total")]
    [Precision(18, 2)]
    public decimal MontoTotal { get; set; }

    [Column("cantidad_requisiciones")]
    public int CantidadRequisiciones { get; set; }

    [Column("cantidad_partidas")]
    public int CantidadPartidas { get; set; }

    [Column("aplica_capitulo_3000")]
    public bool AplicaCapitulo3000 { get; set; }

    [Column("indicaciones_especiales")]
    [MaxLength(2000)]
    public string? IndicacionesEspeciales { get; set; }

    [Column("observaciones")]
    [MaxLength(2000)]
    public string? Observaciones { get; set; }

    [Column("fecha_creacion")]
    public DateOnly FechaCreacion { get; set; }

    [Column("fecha_validacion")]
    public DateOnly? FechaValidacion { get; set; }

    [Column("fecha_asignacion")]
    public DateOnly FechaAsignacion { get; set; }

    [Column("hora_asignacion")]
    public TimeOnly HoraAsignacion { get; set; }

    [Column("id_usuario_responsable")]
    public int IdUsuarioResponsable { get; set; }

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

    // Navigation properties
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
