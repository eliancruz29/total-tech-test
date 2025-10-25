using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RequirementsAPI.Data;
using RequirementsAPI.Models.DTOs;
using RequirementsAPI.Models.Entities;
using RequirementsAPI.Services.Interfaces;

namespace RequirementsAPI.Services.Implementation;

public class RequirementService : IRequirementService
{
    private readonly RequirementsDbContext _context;
    private readonly IOpenRouterService _openRouterService;
    private readonly ILogger<RequirementService> _logger;

    public RequirementService(
        RequirementsDbContext context, 
        IOpenRouterService openRouterService,
        ILogger<RequirementService> logger)
    {
        _context = context;
        _openRouterService = openRouterService;
        _logger = logger;
    }

    public async Task<AnalysisResponse> AnalyzeAndSaveRequirementAsync(string requirementText)
    {
        try
        {
            // Call OpenRouter API to analyze requirements
            var aiResponse = await _openRouterService.AnalyzeRequirementsAsync(requirementText);
            
            // Clean the response (remove markdown code blocks if present)
            var cleanedResponse = aiResponse.Trim();
            if (cleanedResponse.StartsWith("```json"))
            {
                cleanedResponse = cleanedResponse.Substring(7);
            }
            else if (cleanedResponse.StartsWith("```"))
            {
                cleanedResponse = cleanedResponse.Substring(3);
            }
            
            if (cleanedResponse.EndsWith("```"))
            {
                cleanedResponse = cleanedResponse.Substring(0, cleanedResponse.Length - 3);
            }
            
            cleanedResponse = cleanedResponse.Trim();
            
            _logger.LogInformation($"Cleaned AI response: {cleanedResponse}");

            // Parse the AI response
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var aiData = JsonSerializer.Deserialize<JsonElement>(cleanedResponse);

            // Create and save Proceso
            var proceso = new Proceso
            {
                Nombre = aiData.GetProperty("nombre").GetString() ?? "Unnamed Process",
                Descripcion = aiData.TryGetProperty("descripcion", out var desc) ? desc.GetString() : null,
                RequirementText = requirementText,
                CreatedAt = DateTime.UtcNow
            };

            _context.Procesos.Add(proceso);
            await _context.SaveChangesAsync();

            var response = new AnalysisResponse
            {
                IdProceso = proceso.IdProceso,
                Nombre = proceso.Nombre,
                Descripcion = proceso.Descripcion,
                RequirementText = proceso.RequirementText,
                CreatedAt = proceso.CreatedAt,
                Subprocesos = new List<SubprocesoDto>()
            };

            // Process subprocesos if they exist
            if (aiData.TryGetProperty("subprocesos", out var subprocesosArray))
            {
                foreach (var subprocesoElement in subprocesosArray.EnumerateArray())
                {
                    var subproceso = new Subproceso
                    {
                        IdProceso = proceso.IdProceso,
                        Nombre = subprocesoElement.GetProperty("nombre").GetString() ?? "Unnamed Subprocess",
                        Descripcion = subprocesoElement.TryGetProperty("descripcion", out var subDesc) ? subDesc.GetString() : null
                    };

                    _context.Subprocesos.Add(subproceso);
                    await _context.SaveChangesAsync();

                    var subprocesoDto = new SubprocesoDto
                    {
                        IdSubproceso = subproceso.IdSubproceso,
                        Nombre = subproceso.Nombre,
                        Descripcion = subproceso.Descripcion,
                        CasosUso = new List<CasoUsoDto>()
                    };

                    // Process casos de uso if they exist
                    if (subprocesoElement.TryGetProperty("casos_uso", out var casosUsoArray))
                    {
                        foreach (var casoUsoElement in casosUsoArray.EnumerateArray())
                        {
                            short? tipoCasoUso = null;
                            if (casoUsoElement.TryGetProperty("tipo_caso_uso", out var tipoElement))
                            {
                                if (tipoElement.ValueKind == JsonValueKind.Number)
                                {
                                    tipoCasoUso = (short)tipoElement.GetInt32();
                                }
                            }

                            var casoUso = new CasoUso
                            {
                                IdSubproceso = subproceso.IdSubproceso,
                                Nombre = casoUsoElement.GetProperty("nombre").GetString() ?? "Unnamed Use Case",
                                Descripcion = casoUsoElement.TryGetProperty("descripcion", out var cuDesc) ? cuDesc.GetString() : null,
                                ActorPrincipal = casoUsoElement.TryGetProperty("actor_principal", out var actor) ? actor.GetString() : null,
                                TipoCasoUso = tipoCasoUso,
                                Precondiciones = casoUsoElement.TryGetProperty("precondiciones", out var pre) ? pre.GetString() : null,
                                Postcondiciones = casoUsoElement.TryGetProperty("postcondiciones", out var post) ? post.GetString() : null,
                                CriteriosDeAceptacion = casoUsoElement.TryGetProperty("criterios_de_aceptacion", out var criterios) ? criterios.GetString() : null
                            };

                            _context.CasosUso.Add(casoUso);
                            await _context.SaveChangesAsync();

                            subprocesoDto.CasosUso.Add(new CasoUsoDto
                            {
                                IdCasoUso = casoUso.IdCasoUso,
                                Nombre = casoUso.Nombre,
                                Descripcion = casoUso.Descripcion,
                                ActorPrincipal = casoUso.ActorPrincipal,
                                TipoCasoUso = casoUso.TipoCasoUso,
                                TipoCasoUsoText = GetTipoCasoUsoText(casoUso.TipoCasoUso),
                                Precondiciones = casoUso.Precondiciones,
                                Postcondiciones = casoUso.Postcondiciones,
                                CriteriosDeAceptacion = casoUso.CriteriosDeAceptacion
                            });
                        }
                    }

                    response.Subprocesos.Add(subprocesoDto);
                }
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing and saving requirements");
            throw;
        }
    }

    public async Task<List<AnalysisResponse>> GetAllRequirementsAsync()
    {
        var procesos = await _context.Procesos
            .Include(p => p.Subprocesos)
                .ThenInclude(s => s.CasosUso)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return procesos.Select(MapToAnalysisResponse).ToList();
    }

    public async Task<AnalysisResponse?> GetRequirementByIdAsync(int id)
    {
        var proceso = await _context.Procesos
            .Include(p => p.Subprocesos)
                .ThenInclude(s => s.CasosUso)
            .FirstOrDefaultAsync(p => p.IdProceso == id);

        return proceso != null ? MapToAnalysisResponse(proceso) : null;
    }

    private AnalysisResponse MapToAnalysisResponse(Proceso proceso)
    {
        return new AnalysisResponse
        {
            IdProceso = proceso.IdProceso,
            Nombre = proceso.Nombre,
            Descripcion = proceso.Descripcion,
            RequirementText = proceso.RequirementText,
            CreatedAt = proceso.CreatedAt,
            Subprocesos = proceso.Subprocesos.Select(s => new SubprocesoDto
            {
                IdSubproceso = s.IdSubproceso,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                CasosUso = s.CasosUso.Select(cu => new CasoUsoDto
                {
                    IdCasoUso = cu.IdCasoUso,
                    Nombre = cu.Nombre,
                    Descripcion = cu.Descripcion,
                    ActorPrincipal = cu.ActorPrincipal,
                    TipoCasoUso = cu.TipoCasoUso,
                    TipoCasoUsoText = GetTipoCasoUsoText(cu.TipoCasoUso),
                    Precondiciones = cu.Precondiciones,
                    Postcondiciones = cu.Postcondiciones,
                    CriteriosDeAceptacion = cu.CriteriosDeAceptacion
                }).ToList()
            }).ToList()
        };
    }

    private string GetTipoCasoUsoText(short? tipo)
    {
        return tipo switch
        {
            1 => "Functional",
            2 => "Non-Functional",
            3 => "System",
            _ => "Unknown"
        };
    }
}

