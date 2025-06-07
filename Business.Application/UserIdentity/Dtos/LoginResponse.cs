using Business.Application.Services.Users.Dtos;

namespace Business.Application.UserIdentity.Dtos;

/// <summary>
/// Response object for successful login containing JWT token and user information
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// JWT token for authentication
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// Token expiration time
    /// </summary>
    public DateTime ValidUntil { get; set; }

    /// <summary>
    /// User information
    /// </summary>
    public UserDto User { get; set; } = null!;
}