using Business.Model.Entities.Venues;
using Xtech.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Storage.InMemory;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AreaController : ControllerBase
{
    private readonly ArtBookingDbContextInMemory _dbContext;

    public AreaController(ArtBookingDbContextInMemory dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Creates a new area
    /// </summary>
    /// <param name="area">The area to create</param>
    /// <returns>The created area</returns>
    [HttpPost]
    public ActionResult<Area> CreateArea(Area area)
    {
        try
        {
            _dbContext.Add(area);
            _dbContext.SaveChanges();
        }
        catch (Exception exp)
        {
            return Problem(
                statusCode: 500,
                title: "An unexpected error occurred",
                detail: exp.Message
            );
        }

        return CreatedAtAction(nameof(CreateArea), new { area.AreaId }, area);
    }

    /// <summary>
    /// Retrieves an area by ID
    /// </summary>
    /// <param name="id">The ID of the area to retrieve</param>
    /// <returns>The area if found</returns>
    [HttpGet("{id}")]
    public ActionResult<Area> GetArea(int id)
    {
        try
        {
            var area = _dbContext.Areas.Find(id);

            if (area == null) return Problem(
                statusCode: 404,
                title: "Area cannot be found",
                detail: $"Area with id:{id} cannot be found!"
            );
            return Ok(area);
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
    /// List areas with pagination, filtering, and sorting
    /// </summary>
    /// <param name="listParams">List parameters including pagination, filters, and sorting</param>
    /// <returns>A paged list of areas</returns>
    [HttpGet("list")]
    public ActionResult<PagedList<Area>> ListAreas([FromQuery] PagedListParams<AreaFilters> listParams)
    {
        try
        {
            var query = _dbContext.Areas.AsQueryable();

            // Apply filters if provided
            if (listParams.Filters != null)
            {
                // Filter by name (case-insensitive partial match)
                if (!string.IsNullOrEmpty(listParams.Filters.Name))
                {
                    query = query.Where(a => a.Name.ToLower().Contains(listParams.Filters.Name.ToLower()));
                }

                // Filter by venue ID
                if (listParams.Filters.VenueId.HasValue)
                {
                    query = query.Where(a => a.VenueId == listParams.Filters.VenueId.Value);
                }

                // Filter by capacity
                if (listParams.Filters.MinCapacity.HasValue)
                {
                    query = query.Where(a => a.Capacity >= listParams.Filters.MinCapacity.Value);
                }

                // Filter by assigned seats
                if (listParams.Filters.HasAssignedSeats.HasValue)
                {
                    query = query.Where(a => a.HasAssignedSeats == listParams.Filters.HasAssignedSeats.Value);
                }
            }

            // Apply sorting
            if (listParams.HasSort())
            {
                if (listParams.SortByFieldIs("Name"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(a => a.Name)
                        : query.OrderByDescending(a => a.Name);
                }
                else if (listParams.SortByFieldIs("Capacity"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(a => a.Capacity)
                        : query.OrderByDescending(a => a.Capacity);
                }
                else if (listParams.SortByFieldIs("CreatedAt"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(a => a.CreatedAt)
                        : query.OrderByDescending(a => a.CreatedAt);
                }
                // Default sorting by Name ascending if sort field is not recognized
                else
                {
                    query = query.OrderBy(a => a.Name);
                }
            }
            else
            {
                // Default sorting by Name if no sort specified
                query = query.OrderBy(a => a.Name);
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
    /// Updates an existing area
    /// </summary>
    /// <param name="id">The ID of the area to update</param>
    /// <param name="area">The updated area data</param>
    /// <returns>The updated area</returns>
    [HttpPut("{id}")]
    public ActionResult<Area> EditArea(int id, Area area)
    {
        try
        {
            if (id != area.AreaId)
            {
                return BadRequest("The ID in the URL does not match the ID in the provided data.");
            }

            var existingArea = _dbContext.Areas.Find(id);
            if (existingArea == null)
            {
                return NotFound($"Area with id:{id} cannot be found!");
            }

            // Update only scalar properties, preserving relationships
            existingArea.Name = area.Name;
            existingArea.Description = area.Description;
            existingArea.Capacity = area.Capacity;
            existingArea.HasAssignedSeats = area.HasAssignedSeats;
            existingArea.VenueId = area.VenueId;
            // Do NOT update navigation properties (Venue, Seats, PriceEntries, Tickets)

            _dbContext.SaveChanges();

            return Ok(existingArea);
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
    /// Deletes an area by ID
    /// </summary>
    /// <param name="id">The ID of the area to delete</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public ActionResult DeleteArea(int id)
    {
        try
        {
            var area = _dbContext.Areas.Find(id);
            if (area == null)
            {
                return NotFound($"Area with id:{id} cannot be found!");
            }

            _dbContext.Areas.Remove(area);
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
    /// Filter parameters for area listings
    /// </summary>
    public class AreaFilters
    {
        /// <summary>
        /// Filter by area name (partial match)
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Filter by venue ID
        /// </summary>
        public int? VenueId { get; set; }

        /// <summary>
        /// Filter by minimum capacity
        /// </summary>
        public int? MinCapacity { get; set; }

        /// <summary>
        /// Filter by whether the area has assigned seats
        /// </summary>
        public bool? HasAssignedSeats { get; set; }
    }
}