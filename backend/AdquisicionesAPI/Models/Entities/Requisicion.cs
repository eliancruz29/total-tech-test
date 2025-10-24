using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdquisicionesAPI.Models.Entities;

[Table("requisicion")]
public class Requisicion
{
    [Key]
    [Column("id_requisicion")]
    public int IdRequisicion { get; set; }

    [Required]
    [Column("folio")]
    [MaxLength(50)]
    public string Folio { get; set; } = string.Empty;

    [Column("numero_requisicion_srm")]
    [MaxLength(50)]
    public string? NumeroRequisicionSrm { get; set; }

    [Required]
    [Column("descripcion")]
    [MaxLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Column("monto_total")]
    [Precision(18, 2)]
    public decimal MontoTotal { get; set; }

    [Column("id_departamento")]
    public int IdDepartamento { get; set; }

    [Column("id_fuente_presupuestal")]
    public int IdFuentePresupuestal { get; set; }

    [Column("id_nivel_urgencia")]
    public int IdNivelUrgencia { get; set; }

    [Column("id_categoria_requisicion")]
    public int IdCategoriaRequisicion { get; set; }

    [Column("id_estado_requisicion")]
    public int IdEstadoRequisicion { get; set; }

    [Column("folio_consecutivo")]
    [MaxLength(50)]
    public string? FolioConsecutivo { get; set; }

    [Column("fecha_recepcion")]
    public DateOnly? FechaRecepcion { get; set; }

    [Column("palabras_clave")]
    [MaxLength(500)]
    public string? PalabrasClave { get; set; }

    [Column("justificacion")]
    [MaxLength(2000)]
    public string? Justificacion { get; set; }

    [Column("observaciones")]
    [MaxLength(2000)]
    public string? Observaciones { get; set; }

    [Column("id_procedimiento_adquisicion")]
    public int? IdProcedimientoAdquisicion { get; set; }

    [Column("fecha_registro")]
    public DateOnly FechaRegistro { get; set; }

    [Column("hora_registro")]
    public TimeOnly HoraRegistro { get; set; }

    [Column("id_usuario_solicitante")]
    public int IdUsuarioSolicitante { get; set; }

    [Column("fecha_modifica")]
    public DateOnly? FechaModifica { get; set; }

    [Column("hora_modifica")]
    public TimeOnly? HoraModifica { get; set; }

    [Column("id_usuario_modifica")]
    public int? IdUsuarioModifica { get; set; }

    // Navigation properties
    [ForeignKey("IdUsuarioSolicitante")]
    public virtual SpartanUser? UsuarioSolicitante { get; set; }

    [ForeignKey("IdProcedimientoAdquisicion")]
    public virtual ProcedimientoAdquisicion? ProcedimientoAdquisicion { get; set; }

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
