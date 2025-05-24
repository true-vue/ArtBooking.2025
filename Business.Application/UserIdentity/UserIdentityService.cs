using Business.Model.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Business.Application.UserIdentity.Dtos;
using AutoMapper;
using Business.Application.Services.Users.Dtos;
using Business.Application.Repositories;

namespace Business.Application.UserIdentity;

/// <summary>
/// Implementation of IUserIdentityService that provides user authentication functionality.
/// </summary>
public class UserIdentityService : IUserIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;

    public UserIdentityService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public LoginResult Login(LoginRequest request)
    {
        var user = _userRepository.GetUserByUserName(request.Username);

        if (user == null)
            return new LoginResult { Success = false, ErrorMessage = "Invalid credentials" };

        if (!user.IsActive)
            return new LoginResult { Success = false, ErrorMessage = "Account is disabled" };

        if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            return new LoginResult { Success = false, ErrorMessage = "Invalid credentials" };

        return new LoginResult { Success = true, User = _mapper.Map<UserDto>(user) };
    }
}