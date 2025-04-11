using Business.Model.Entities.Users;

namespace Business.Application.Services.Users.Dtos;

public class UserDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserRole? Role { get; set; }
    public int? ArtOrganizationId { get; set; }
    public bool IsActive { get; set; }
}