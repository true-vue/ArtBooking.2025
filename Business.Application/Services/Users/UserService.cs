using Business.Model.Data;
using Business.Model.Entities.Users;
using Business.Application.Services.Users.Dtos;
using Microsoft.AspNetCore.Identity;
using Business.Application.UserIdentity;

namespace Business.Application.Services.Users;

/// <summary>
/// Implementation of IUserService that provides user validation functionality.
/// </summary>
public class UserService : IUserService
{
    private readonly ArtBookingDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the UserService class.
    /// </summary>
    /// <param name="dbContext">The database context used for user validation.</param>
    /// <param name="passwordHasher">The password hasher used for hashing user passwords.</param>
    /// <param name="userContext">The user context used for tracking the current user.</param>
    public UserService(ArtBookingDbContext dbContext, IPasswordHasher<User> passwordHasher, IUserContext userContext)
    {
        _dbContext = dbContext;
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
        return _dbContext.Users.FirstOrDefault(u => u.Username == userName);
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
        if (_dbContext.Users.Any(u => u.Username == createUserDto.Username))
        {
            throw new Exception($"Username '{createUserDto.Username}' is already taken.");
        }

        // Check if email already exists
        if (_dbContext.Users.Any(u => u.Email == createUserDto.Email))
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

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return user;
    }
}