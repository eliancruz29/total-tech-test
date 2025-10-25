namespace RequirementsAPI.Models.Entities;

public class Subproceso
{
    public int IdSubproceso { get; set; }
    public int IdProceso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    
    // Navigation properties
    public Proceso? Proceso { get; set; }
    public ICollection<CasoUso> CasosUso { get; set; } = new List<CasoUso>();
}

