using System.ComponentModel.DataAnnotations;
using Business.Model.Entities.Users;

namespace Business.Application.Services.Users.Dtos;

/// <summary>
/// Data Transfer Object for creating a new user
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// User's username (used for login)
    /// </summary>
    [Required]
    public string Username { get; set; } = null!;

    /// <summary>
    /// User's email address
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    /// <summary>
    /// User's password
    /// </summary>
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    /// <summary>
    /// User's first name
    /// </summary>
    [Required]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// User's last name
    /// </summary>
    [Required]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// User's role in the system
    /// </summary>
    public UserRole? Role { get; set; }

    /// <summary>
    /// ID of the organization this user is associated with (if applicable)
    /// </summary>
    public int? ArtOrganizationId { get; set; }
}