using Business.Model.Entities.Common;
using Business.Model.Entities.Organizations;
using System.Text.Json.Serialization;

namespace Business.Model.Entities.Users
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User's username (used for login)
        /// <summary>
        public string Username { get; set; } = null!;

        /// User's email address (used for login)
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User's password hash
        /// </summary>
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// User's role in the system
        /// </summary>
        public UserRole? Role { get; set; }

        /// <summary>
        /// ID of the organization this user is associated with (if applicable)
        /// </summary>
        public int? ArtOrganizationId { get; set; }

        /// <summary>
        /// Navigation property for the organization this user is associated with (if applicable)
        /// </summary>
        [JsonIgnore]
        public virtual ArtOrganization? Organization { get; set; }
    }
}