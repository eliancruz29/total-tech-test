using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AdquisicionesAPI.Data;
using AdquisicionesAPI.Models.DTOs;
using AdquisicionesAPI.Services.Interfaces;
using BCrypt.Net;

namespace AdquisicionesAPI.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly AdquisicionesDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        AdquisicionesDbContext context,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<TokenResponse?> AuthenticateAsync(string username, string password)
    {
        try
        {
            // Find user by email (username)
            var user = await _context.SpartanUsers
                .FirstOrDefaultAsync(u => u.Email == username && u.IsActive);

            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
                return null;
            }

            // Verify password using BCrypt
            // Note: For the seed data, password "admin123" has hash starting with $2a$11$
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid password for user: {Username}", username);
                return null;
            }

            // Generate JWT token
            var token = GenerateJwtToken(user.IdUser, user.Email, user.Name);

            var expirationMinutes = int.Parse(_configuration["JWT:ExpirationMinutes"] ?? "60");

            return new TokenResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expirationMinutes * 60, // Convert to seconds
                Issuer = _configuration["JWT:Issuer"] ?? "",
                Audience = _configuration["JWT:Audience"] ?? ""
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for user: {Username}", username);
            return null;
        }
    }

    public string GenerateJwtToken(int userId, string email, string name)
    {
        var secret = _configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var issuer = _configuration["JWT:Issuer"] ?? "AdquisicionesAPI";
        var audience = _configuration["JWT:Audience"] ?? "AdquisicionesClient";
        var expirationMinutes = int.Parse(_configuration["JWT:ExpirationMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
