using Business.Model.Entities.Users;
using Business.Application.Services.Users.Dtos;
using Microsoft.AspNetCore.Identity;
using Business.Application.UserIdentity;
using Business.Application.Repositories;

namespace Business.Application.Services.Users;

/// <summary>
/// Implementation of IUserService that provides user validation functionality.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the UserService class.
    /// </summary>
    /// <param name="userRepository">The repository used for user data access.</param>
    /// <param name="passwordHasher">The password hasher used for hashing user passwords.</param>
    /// <param name="userContext">The user context used for tracking the current user.</param>
    public UserService(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _userContext = userContext;
    }

    /// <summary>
    /// Gets a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>The user if found, null otherwise.</returns>
    public User? GetUser(string userName)
    {
        return _userRepository.GetUserByUserName(userName);
    }

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="createUserDto">The data for creating the new user.</param>
    /// <returns>The created user.</returns>
    /// <exception cref="Exception">Thrown when a user with the same username or email already exists.</exception>
    public User CreateUser(CreateUserDto createUserDto)
    {
        // Check if username already exists
        if (_userRepository.GetUserByUserName(createUserDto.Username) != null)
        {
            throw new Exception($"Username '{createUserDto.Username}' is already taken.");
        }

        // Check if email already exists
        if (_userRepository.GetUserByEmail(createUserDto.Email) != null)
        {
            throw new Exception($"Email '{createUserDto.Email}' is already registered.");
        }

        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Role = createUserDto.Role,
            ArtOrganizationId = createUserDto.ArtOrganizationId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedById = _userContext.UserId,
            UpdatedAt = DateTime.UtcNow,
            UpdatedById = _userContext.UserId
        };

        // Hash the password
        user.PasswordHash = _passwordHasher.HashPassword(user, createUserDto.Password);

        _userRepository.Add(user);
        return user;
    }

    /// <summary>
    /// Checks if there are any users in the system.
    /// </summary>
    /// <returns>True if there are users, false otherwise.</returns>
    public bool HasUsers()
    {
        return _userRepository.HasUsers();
    }
}