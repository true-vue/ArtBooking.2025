using Business.Model.Data;
using Business.Model.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Business.Application.UserIdentity.Dtos;
using AutoMapper;
using Business.Application.Services.Users.Dtos;

namespace Business.Application.UserIdentity;

/// <summary>
/// Implementation of IUserIdentityService that provides user authentication functionality.
/// </summary>
public class UserIdentityService : IUserIdentityService
{
    private readonly ArtBookingDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;

    public UserIdentityService(ArtBookingDbContext dbContext, IPasswordHasher<User> passwordHasher, IMapper mapper)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public LoginResult Login(LoginRequest request)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
            return new LoginResult { Success = false, ErrorMessage = "Invalid credentials" };

        if (!user.IsActive)
            return new LoginResult { Success = false, ErrorMessage = "Account is disabled" };

        if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            return new LoginResult { Success = false, ErrorMessage = "Invalid credentials" };

        return new LoginResult { Success = true, User = _mapper.Map<UserDto>(user) };
    }
}