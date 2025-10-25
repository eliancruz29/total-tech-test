using RequirementsAPI.Models.DTOs;

namespace RequirementsAPI.Services.Interfaces;

public interface IOpenRouterService
{
    Task<string> AnalyzeRequirementsAsync(string requirementText);
}

