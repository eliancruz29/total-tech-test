using System.ComponentModel.DataAnnotations;

namespace AdquisicionesAPI.Models.DTOs;

public class LoginRequest
{
    [Required]
    public string GrantType { get; set; } = "password";

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
