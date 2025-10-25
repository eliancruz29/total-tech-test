using RequirementsAPI.Models.DTOs;

namespace RequirementsAPI.Services.Interfaces;

public interface IRequirementService
{
    Task<AnalysisResponse> AnalyzeAndSaveRequirementAsync(string requirementText);
    Task<List<AnalysisResponse>> GetAllRequirementsAsync();
    Task<AnalysisResponse?> GetRequirementByIdAsync(int id);
}

