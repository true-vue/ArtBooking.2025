namespace Business.Application.UserIdentity;

/// <summary>
/// Provides context information about the current user in the application.
/// This interface is used to access user identity and authorization information throughout the application.
/// </summary>
public interface IUserContext
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// </summary>
    int UserId { get; }

    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets a read-only list of roles assigned to the current user.
    /// </summary>
    IReadOnlyList<string> Roles { get; }
}