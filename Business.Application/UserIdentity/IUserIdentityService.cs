using Business.Application.UserIdentity.Dtos;

namespace Business.Application.UserIdentity;

/// <summary>
/// Provides user identity and authentication services.
/// This interface is used to handle user login and authentication throughout the application.
/// </summary>
public interface IUserIdentityService
{
    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="request">The login request containing username and password.</param>
    /// <returns>A login result indicating success or failure with appropriate messages.</returns>
    LoginResult Login(LoginRequest request);
}
