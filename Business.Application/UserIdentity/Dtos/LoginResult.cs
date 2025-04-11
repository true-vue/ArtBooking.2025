using Business.Application.Services.Users.Dtos;

namespace Business.Application.UserIdentity.Dtos;

public class LoginResult
{
    public bool Success { get; set; }
    public UserDto User { get; set; }
    public string ErrorMessage { get; set; }
}