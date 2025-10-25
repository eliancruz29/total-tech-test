namespace RequirementsAPI.Models.DTOs;

public class AnalysisResponse
{
    public int IdProceso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? RequirementText { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SubprocesoDto> Subprocesos { get; set; } = new();
}

public class SubprocesoDto
{
    public int IdSubproceso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public List<CasoUsoDto> CasosUso { get; set; } = new();
}

public class CasoUsoDto
{
    public int IdCasoUso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? ActorPrincipal { get; set; }
    public short? TipoCasoUso { get; set; }
    public string? TipoCasoUsoText { get; set; }
    public string? Precondiciones { get; set; }
    public string? Postcondiciones { get; set; }
    public string? CriteriosDeAceptacion { get; set; }
}

