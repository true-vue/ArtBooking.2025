using Business.Model.Entities.Events;
using Business.Model.Entities.Events.Enums;
using Business.Application.Repositories;
using Xtech.Common.Pagination;

namespace Storage.MsSql.Repositories;

public class ArtEventRepository : IArtEventRepository
{
    private readonly ArtBookingDbContextMsSql _dbContext;

    public ArtEventRepository(ArtBookingDbContextMsSql dbContext)
    {
        _dbContext = dbContext;
    }

    public ArtEvent GetById(int id) => _dbContext.ArtEvents.Find(id);

    public void Add(ArtEvent artEvent)
    {
        _dbContext.ArtEvents.Add(artEvent);
        _dbContext.SaveChanges();
    }

    public ArtEvent Update(ArtEvent artEvent)
    {
        var updatedEntity = _dbContext.ArtEvents.Update(artEvent).Entity;
        _dbContext.SaveChanges();
        return updatedEntity;
    }

    public void Delete(ArtEvent artEvent)
    {
        _dbContext.ArtEvents.Remove(artEvent);
        _dbContext.SaveChanges();
    }

    public PagedList<ArtEvent> GetPagedList(
        string? nameFilter = null,
        int? categoryFilter = null,
        int? statusFilter = null,
        int? organizationIdFilter = null,
        int pageNumber = 1,
        int pageSize = 20,
        string? sortBy = null,
        bool sortAscending = true)
    {
        var query = _dbContext.ArtEvents.AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(nameFilter))
            query = query.Where(e => e.Name.ToLower().Contains(nameFilter.ToLower()));
        if (categoryFilter.HasValue)
            query = query.Where(e => e.Category == (EventCategory)categoryFilter.Value);
        if (statusFilter.HasValue)
            query = query.Where(e => e.Status == (EventStatus)statusFilter.Value);
        if (organizationIdFilter.HasValue)
            query = query.Where(e => e.ArtOrganizationId == organizationIdFilter.Value);

        // Apply sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = ApplySorting(query, sortBy, sortAscending);
        }

        return query.AsPagedList(pageNumber, pageSize);
    }

    public IEnumerable<ArtEvent> GetAll() => _dbContext.ArtEvents.ToList();

    public bool Exists(int id) => _dbContext.ArtEvents.Any(e => e.ArtEventId == id);

    private IQueryable<ArtEvent> ApplySorting(
        IQueryable<ArtEvent> query,
        string? sortBy,
        bool sortAscending)
    {
        return (sortBy?.ToLower(), sortAscending) switch
        {
            ("name", true) => query.OrderBy(e => e.Name),
            ("name", false) => query.OrderByDescending(e => e.Name),
            ("status", true) => query.OrderBy(e => e.Status),
            ("status", false) => query.OrderByDescending(e => e.Status),
            ("createdat", true) => query.OrderBy(e => e.CreatedAt),
            ("createdat", false) => query.OrderByDescending(e => e.CreatedAt),
            _ => throw new ArgumentException("Unsupported sort field")
        };
    }
}