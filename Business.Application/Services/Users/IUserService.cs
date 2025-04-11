using Business.Model.Entities.Users;
using Business.Application.Services.Users.Dtos;

namespace Business.Application.Services.Users;

/// <summary>
/// Provides user validation and management services.
/// This interface is used to perform user-related operations and validations throughout the application.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>The user if found, null otherwise.</returns>
    User? GetUser(string userName);

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="createUserDto">The data for creating the new user.</param>
    /// <returns>The created user.</returns>
    /// <exception cref="Exception">Thrown when a user with the same username or email already exists.</exception>
    User CreateUser(CreateUserDto createUserDto);
}