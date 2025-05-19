using Business.Model.Entities.Venues;
using Xtech.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Storage.InMemory;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VenueController : ControllerBase
{
    private readonly ArtBookingDbContextInMemory _dbContext;

    public VenueController(ArtBookingDbContextInMemory dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public ActionResult<Venue> CreateVenue(Venue venue)
    {
        try
        {
            _dbContext.Add(venue);
            _dbContext.SaveChanges();
        }
        catch (Exception exp)
        {
            return Problem(
                statusCode: 500,
                title: "An unexpected error occured",
                // just for debugging purposes
                detail: exp.Message
            );
        }

        return CreatedAtAction(nameof(CreateVenue), new { venue.VenueId }, venue);
    }

    [HttpGet("{id}")]
    public ActionResult<Venue> GetVenue(int id)
    {
        try
        {
            var venue = _dbContext.Venues.Find(id);

            if (venue == null) return Problem(
                statusCode: 404,
                title: "Venue cannot be found",
                detail: $"Venue with id:{id} cannot be found!"
            );
            return Ok(venue);
        }
        catch (Exception exp)
        {
            return Problem(
                statusCode: 500,
                title: "An unexpected error occured",
                // just for debugging purposes
                detail: exp.Message
            );
        }
    }

    /// <summary>
    /// List venues with pagination, filtering, and sorting
    /// </summary>
    /// <param name="listParams">List parameters including pagination, filters, and sorting</param>
    /// <returns>A paged list of venues</returns>
    [HttpGet("list")]
    public ActionResult<PagedList<Venue>> ListVenues([FromQuery] PagedListParams<VenueFilters> listParams)
    {
        try
        {
            var query = _dbContext.Venues.AsQueryable();

            // Apply filters if provided
            if (listParams.Filters != null)
            {
                // Filter by name (case-insensitive partial match)
                if (!string.IsNullOrEmpty(listParams.Filters.Name))
                {
                    query = query.Where(v => v.Name.ToLower().Contains(listParams.Filters.Name.ToLower()));
                }

                // Filter by city
                if (!string.IsNullOrEmpty(listParams.Filters.City))
                {
                    query = query.Where(v => v.City.ToLower().Contains(listParams.Filters.City.ToLower()));
                }

                // Filter by capacity
                if (listParams.Filters.MinCapacity.HasValue)
                {
                    query = query.Where(v => v.Capacity >= listParams.Filters.MinCapacity.Value);
                }

                // Filter by organization
                if (listParams.Filters.ArtOrganizationId.HasValue)
                {
                    query = query.Where(v => v.ArtOrganizationId == listParams.Filters.ArtOrganizationId.Value);
                }
            }

            // Apply sorting
            if (listParams.HasSort())
            {
                if (listParams.SortByFieldIs("Name"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(v => v.Name)
                        : query.OrderByDescending(v => v.Name);
                }
                else if (listParams.SortByFieldIs("Capacity"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(v => v.Capacity)
                        : query.OrderByDescending(v => v.Capacity);
                }
                else if (listParams.SortByFieldIs("City"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(v => v.City)
                        : query.OrderByDescending(v => v.City);
                }
                else if (listParams.SortByFieldIs("CreatedAt"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(v => v.CreatedAt)
                        : query.OrderByDescending(v => v.CreatedAt);
                }
                // Default sorting by Name ascending if sort field is not recognized
                else
                {
                    query = query.OrderBy(v => v.Name);
                }
            }
            else
            {
                // Default sorting by Name if no sort specified
                query = query.OrderBy(v => v.Name);
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
    /// Updates an existing venue
    /// </summary>
    /// <param name="id">The ID of the venue to update</param>
    /// <param name="venue">The updated venue data</param>
    /// <returns>The updated venue</returns>
    [HttpPut("{id}")]
    public ActionResult<Venue> EditVenue(int id, Venue venue)
    {
        try
        {
            if (id != venue.VenueId)
            {
                return BadRequest("The ID in the URL does not match the ID in the provided data.");
            }

            var existingVenue = _dbContext.Venues.Find(id);
            if (existingVenue == null)
            {
                return NotFound($"Venue with id:{id} cannot be found!");
            }

            // Update only scalar properties, preserving relationships
            existingVenue.Name = venue.Name;
            existingVenue.Description = venue.Description;
            existingVenue.Address = venue.Address;
            existingVenue.City = venue.City;
            existingVenue.State = venue.State;
            existingVenue.Country = venue.Country;
            existingVenue.PostalCode = venue.PostalCode;
            existingVenue.Email = venue.Email;
            existingVenue.PhoneNumber = venue.PhoneNumber;
            existingVenue.Website = venue.Website;
            existingVenue.Capacity = venue.Capacity;
            existingVenue.ImageUrl = venue.ImageUrl;
            existingVenue.ArtOrganizationId = venue.ArtOrganizationId;
            // Do NOT update navigation properties (Organization, Areas, PriceLists, ScheduleItems)

            _dbContext.SaveChanges();

            return Ok(existingVenue);
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
    /// Deletes a venue by ID
    /// </summary>
    /// <param name="id">The ID of the venue to delete</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public ActionResult DeleteVenue(int id)
    {
        try
        {
            var venue = _dbContext.Venues.Find(id);
            if (venue == null)
            {
                return NotFound($"Venue with id:{id} cannot be found!");
            }

            _dbContext.Venues.Remove(venue);
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
    /// Filter parameters for venue listings
    /// </summary>
    public class VenueFilters
    {
        /// <summary>
        /// Filter by venue name (partial match)
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Filter by city (partial match)
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Filter by minimum capacity
        /// </summary>
        public int? MinCapacity { get; set; }

        /// <summary>
        /// Filter by art organization ID
        /// </summary>
        public int? ArtOrganizationId { get; set; }
    }
}