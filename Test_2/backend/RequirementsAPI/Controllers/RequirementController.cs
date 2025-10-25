using Microsoft.AspNetCore.Mvc;
using RequirementsAPI.Models.DTOs;
using RequirementsAPI.Services.Interfaces;

namespace RequirementsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequirementController : ControllerBase
{
    private readonly IRequirementService _requirementService;
    private readonly ILogger<RequirementController> _logger;

    public RequirementController(IRequirementService requirementService, ILogger<RequirementController> logger)
    {
        _requirementService = requirementService;
        _logger = logger;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<AnalysisResponse>> AnalyzeRequirement([FromBody] RequirementRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.RequirementText))
            {
                return BadRequest(new { error = "Requirement text cannot be empty" });
            }

            var result = await _requirementService.AnalyzeAndSaveRequirementAsync(request.RequirementText);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing requirement");
            return StatusCode(500, new { error = "An error occurred while analyzing the requirement", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<AnalysisResponse>>> GetAllRequirements()
    {
        try
        {
            var requirements = await _requirementService.GetAllRequirementsAsync();
            return Ok(requirements);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving requirements");
            return StatusCode(500, new { error = "An error occurred while retrieving requirements", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnalysisResponse>> GetRequirementById(int id)
    {
        try
        {
            var requirement = await _requirementService.GetRequirementByIdAsync(id);
            
            if (requirement == null)
            {
                return NotFound(new { error = "Requirement not found" });
            }

            return Ok(requirement);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving requirement");
            return StatusCode(500, new { error = "An error occurred while retrieving the requirement", details = ex.Message });
        }
    }
}

