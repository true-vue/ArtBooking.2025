using Business.Model.Entities.Users;
using Xtech.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Storage.InMemory;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ArtBookingDbContextInMemory _dbContext;

    public UserController(ArtBookingDbContextInMemory dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public ActionResult<User> CreateUser(User user)
    {
        try
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }
        catch (Exception exp)
        {
            return Problem(
                statusCode: 500,
                title: "An unexpected error occurred",
                // just for debugging purposes
                detail: exp.Message
            );
        }

        return CreatedAtAction(nameof(CreateUser), new { user.UserId }, user);
    }

    [HttpGet]
    public ActionResult<User> GetUser(int id)
    {
        try
        {
            var user = _dbContext.Users.Find(id);

            if (user == null) return Problem(
                statusCode: 404,
                title: "User cannot be found",
                detail: $"User with id:{id} cannot be found!"
            );
            return Ok(user);
        }
        catch (Exception exp)
        {
            return Problem(
                statusCode: 500,
                title: "An unexpected error occurred",
                // just for debugging purposes
                detail: exp.Message
            );
        }
    }

    /// <summary>
    /// List users with pagination, filtering, and sorting
    /// </summary>
    /// <param name="listParams">List parameters including pagination, filters, and sorting</param>
    /// <returns>A paged list of users</returns>
    [HttpGet("list")]
    public ActionResult<PagedList<User>> ListUsers([FromQuery] PagedListParams<UserFilters> listParams)
    {
        try
        {
            var query = _dbContext.Users.AsQueryable();

            // Apply filters if provided
            if (listParams.Filters != null)
            {
                // Filter by name (case-insensitive partial match)
                if (!string.IsNullOrEmpty(listParams.Filters.Name))
                {
                    query = query.Where(u =>
                        u.FirstName.ToLower().Contains(listParams.Filters.Name.ToLower()) ||
                        u.LastName.ToLower().Contains(listParams.Filters.Name.ToLower()));
                }

                // Filter by email
                if (!string.IsNullOrEmpty(listParams.Filters.Email))
                {
                    query = query.Where(u => u.Email.ToLower().Contains(listParams.Filters.Email.ToLower()));
                }

                // Filter by role
                if (listParams.Filters.Role.HasValue)
                {
                    query = query.Where(u => u.Role == listParams.Filters.Role.Value);
                }
            }

            // Apply sorting
            if (listParams.HasSort())
            {
                if (listParams.SortByFieldIs("LastName"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(u => u.LastName)
                        : query.OrderByDescending(u => u.LastName);
                }
                else if (listParams.SortByFieldIs("FirstName"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(u => u.FirstName)
                        : query.OrderByDescending(u => u.FirstName);
                }
                else if (listParams.SortByFieldIs("Email"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(u => u.Email)
                        : query.OrderByDescending(u => u.Email);
                }
                else if (listParams.SortByFieldIs("CreatedAt"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(u => u.CreatedAt)
                        : query.OrderByDescending(u => u.CreatedAt);
                }
                // Default sorting by LastName, FirstName ascending if sort field is not recognized
                else
                {
                    query = query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);
                }
            }
            else
            {
                // Default sorting by LastName, FirstName if no sort specified
                query = query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);
            }

            var result = query.AsPagedList(listParams.PageNumber, listParams.PageSize);
            return Ok(result);
        }
        catch (Exception exp)
        {
            return Problem(
                statusCode: 500,
                title: "An unexpected error occurred",
                detail: exp.Message
            );
        }
    }

    /// <summary>
    /// Updates an existing user
    /// </summary>
    /// <param name="id">The ID of the user to update</param>
    /// <param name="user">The updated user data</param>
    /// <returns>The updated user</returns>
    [HttpPut("{id}")]
    public ActionResult<User> EditUser(int id, User user)
    {
        try
        {
            if (id != user.UserId)
            {
                return BadRequest("The ID in the URL does not match the ID in the provided data.");
            }

            var existingUser = _dbContext.Users.Find(id);
            if (existingUser == null)
            {
                return NotFound($"User with id:{id} cannot be found!");
            }

            // Update user properties
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            _dbContext.SaveChanges();

            return Ok(existingUser);
        }
        catch (Exception exp)
        {
            return Problem(
                statusCode: 500,
                title: "An unexpected error occurred",
                detail: exp.Message
            );
        }
    }

    /// <summary>
    /// Deletes a user from the system
    /// </summary>
    /// <param name="id">The ID of the user to delete</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        try
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound($"User with id:{id} cannot be found!");
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return NoContent();
        }
        catch (Exception exp)
        {
            return Problem(
                statusCode: 500,
                title: "An unexpected error occurred",
                detail: exp.Message
            );
        }
    }

    /// <summary>
    /// Filter parameters for user listings
    /// </summary>
    public class UserFilters
    {
        /// <summary>
        /// Filter by user name (partial match on first or last name)
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Filter by user email (partial match)
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Filter by user role
        /// </summary>
        public UserRole? Role { get; set; }
    }
}