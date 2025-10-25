using Microsoft.AspNetCore.Mvc;
using AdquisicionesAPI.Models.DTOs;
using AdquisicionesAPI.Services.Interfaces;

namespace AdquisicionesAPI.Controllers;

[Route("oauth")]
[ApiController]
public class OAuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<OAuthController> _logger;

    public OAuthController(IAuthService authService, ILogger<OAuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Obtain JWT authentication token
    /// </summary>
    /// <param name="grantType">Grant type (must be "password")</param>
    /// <param name="username">User email</param>
    /// <param name="password">User password</param>
    /// <returns>JWT token response</returns>
    [HttpGet("token")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> GetToken(
        [FromQuery(Name = "grant_type")] string? grantType,
        [FromQuery(Name = "username")] string? username,
        [FromQuery(Name = "password")] string? password)
    {
        // Also support form data (from Postman collection)
        if (string.IsNullOrEmpty(grantType) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            var form = await Request.ReadFormAsync();
            grantType ??= form["grant_type"].ToString();
            username ??= form["username"].ToString();
            password ??= form["password"].ToString();
        }

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return BadRequest(new { error = "invalid_request", error_description = "Username and password are required" });
        }

        if (grantType != "password")
        {
            return BadRequest(new { error = "unsupported_grant_type", error_description = "Only 'password' grant type is supported" });
        }

        var tokenResponse = await _authService.AuthenticateAsync(username, password);

        if (tokenResponse == null)
        {
            return Unauthorized(new { error = "invalid_grant", error_description = "Invalid username or password" });
        }

        _logger.LogInformation("User authenticated successfully: {Username}", username);

        return Ok(tokenResponse);
    }
}
