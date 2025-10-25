namespace RequirementsAPI.Models.Entities;

public class Proceso
{
    public int IdProceso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? RequirementText { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ICollection<Subproceso> Subprocesos { get; set; } = new List<Subproceso>();
}

