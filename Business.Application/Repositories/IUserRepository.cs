using Business.Model.Entities.Users;

namespace Business.Application.Repositories;

/// <summary>
/// Provides methods for user data access and manipulation.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user to add.</param>
    void Add(User user);

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>The user if found, null otherwise.</returns>
    User? GetUserByUserName(string userName);

    /// <summary>
    /// Retrieves a user by their email.
    /// </summary>
    /// <param name="email">The email of the user to retrieve.</param>
    /// <returns>The user if found, null otherwise.</returns>
    User? GetUserByEmail(string email);

    /// <summary>
    /// Checks if there are any users in the repository.
    /// </summary>
    /// <returns>True if users exist, otherwise false.</returns>
    bool HasUsers();
}
