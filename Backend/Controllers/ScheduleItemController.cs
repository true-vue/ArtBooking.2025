using Business.Model.Entities.Events;
using Xtech.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Storage.InMemory;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ScheduleItemController : ControllerBase
{
    private readonly ArtBookingDbContextInMemory _dbContext;

    public ScheduleItemController(ArtBookingDbContextInMemory dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public ActionResult<ScheduleItem> CreateScheduleItem(ScheduleItem scheduleItem)
    {
        try
        {
            _dbContext.Add(scheduleItem);
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

        return CreatedAtAction(nameof(CreateScheduleItem), new { scheduleItem.ScheduleItemId }, scheduleItem);
    }

    [HttpGet("{id}")]
    public ActionResult<ScheduleItem> GetScheduleItem(int id)
    {
        try
        {
            var scheduleItem = _dbContext.ScheduleItems.Find(id);

            if (scheduleItem == null) return Problem(
                statusCode: 404,
                title: "Schedule item cannot be found",
                detail: $"Schedule item with id:{id} cannot be found!"
            );
            return Ok(scheduleItem);
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
    /// List schedule items with pagination, filtering, and sorting
    /// </summary>
    /// <param name="listParams">List parameters including pagination, filters, and sorting</param>
    /// <returns>A paged list of schedule items</returns>
    [HttpGet("list")]
    public ActionResult<PagedList<ScheduleItem>> ListScheduleItems([FromQuery] PagedListParams<ScheduleItemFilters> listParams)
    {
        try
        {
            var query = _dbContext.ScheduleItems.AsQueryable();

            // Apply filters if provided
            if (listParams.Filters != null)
            {
                // Filter by title (case-insensitive partial match)
                if (!string.IsNullOrEmpty(listParams.Filters.Title))
                {
                    query = query.Where(s => s.Title.ToLower().Contains(listParams.Filters.Title.ToLower()));
                }

                // Filter by event ID
                if (listParams.Filters.ArtEventId.HasValue)
                {
                    query = query.Where(s => s.ArtEventId == listParams.Filters.ArtEventId.Value);
                }

                // Filter by venue ID
                if (listParams.Filters.VenueId.HasValue)
                {
                    query = query.Where(s => s.VenueId == listParams.Filters.VenueId.Value);
                }

                // Filter by date range
                if (listParams.Filters.StartDateFrom.HasValue)
                {
                    query = query.Where(s => s.StartTime >= listParams.Filters.StartDateFrom.Value);
                }

                if (listParams.Filters.StartDateTo.HasValue)
                {
                    query = query.Where(s => s.StartTime <= listParams.Filters.StartDateTo.Value);
                }
            }

            // Apply sorting
            if (listParams.HasSort())
            {
                if (listParams.SortByFieldIs("Title"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(s => s.Title)
                        : query.OrderByDescending(s => s.Title);
                }
                else if (listParams.SortByFieldIs("StartTime"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(s => s.StartTime)
                        : query.OrderByDescending(s => s.StartTime);
                }
                else if (listParams.SortByFieldIs("EndTime"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(s => s.EndTime)
                        : query.OrderByDescending(s => s.EndTime);
                }
                else if (listParams.SortByFieldIs("Order"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(s => s.Order)
                        : query.OrderByDescending(s => s.Order);
                }
                else if (listParams.SortByFieldIs("CreatedAt"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(s => s.CreatedAt)
                        : query.OrderByDescending(s => s.CreatedAt);
                }
                // Default sorting by StartTime ascending if sort field is not recognized
                else
                {
                    query = query.OrderBy(s => s.StartTime);
                }
            }
            else
            {
                // Default sorting by StartTime if no sort specified
                query = query.OrderBy(s => s.StartTime);
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
    /// Updates an existing schedule item
    /// </summary>
    /// <param name="id">The ID of the schedule item to update</param>
    /// <param name="scheduleItem">The updated schedule item data</param>
    /// <returns>The updated schedule item</returns>
    [HttpPut("{id}")]
    public ActionResult<ScheduleItem> EditScheduleItem(int id, ScheduleItem scheduleItem)
    {
        try
        {
            if (id != scheduleItem.ScheduleItemId)
            {
                return BadRequest("The ID in the URL does not match the ID in the provided data.");
            }

            var existingScheduleItem = _dbContext.ScheduleItems.Find(id);
            if (existingScheduleItem == null)
            {
                return NotFound($"Schedule item with id:{id} cannot be found!");
            }

            // Update properties
            existingScheduleItem.Title = scheduleItem.Title;
            existingScheduleItem.Description = scheduleItem.Description;
            existingScheduleItem.StartTime = scheduleItem.StartTime;
            existingScheduleItem.EndTime = scheduleItem.EndTime;
            existingScheduleItem.Location = scheduleItem.Location;
            existingScheduleItem.Order = scheduleItem.Order;
            existingScheduleItem.ArtEventId = scheduleItem.ArtEventId;
            existingScheduleItem.VenueId = scheduleItem.VenueId;
            existingScheduleItem.PriceListId = scheduleItem.PriceListId;
            // Do NOT update navigation properties

            _dbContext.SaveChanges();

            return Ok(existingScheduleItem);
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
    /// Deletes a schedule item by ID
    /// </summary>
    /// <param name="id">The ID of the schedule item to delete</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public ActionResult DeleteScheduleItem(int id)
    {
        try
        {
            var scheduleItem = _dbContext.ScheduleItems.Find(id);
            if (scheduleItem == null)
            {
                return NotFound($"Schedule item with id:{id} cannot be found!");
            }

            _dbContext.ScheduleItems.Remove(scheduleItem);
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
    /// Filter parameters for schedule item listings
    /// </summary>
    public class ScheduleItemFilters
    {
        /// <summary>
        /// Filter by schedule item title (partial match)
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Filter by event ID
        /// </summary>
        public int? ArtEventId { get; set; }

        /// <summary>
        /// Filter by venue ID
        /// </summary>
        public int? VenueId { get; set; }

        /// <summary>
        /// Filter by start date (from)
        /// </summary>
        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        /// Filter by start date (to)
        /// </summary>
        public DateTime? StartDateTo { get; set; }
    }
}