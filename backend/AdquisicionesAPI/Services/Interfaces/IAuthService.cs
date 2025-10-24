using AdquisicionesAPI.Models.DTOs;

namespace AdquisicionesAPI.Services.Interfaces;

public interface IAuthService
{
    Task<TokenResponse?> AuthenticateAsync(string username, string password);
    string GenerateJwtToken(int userId, string email, string name);
}
