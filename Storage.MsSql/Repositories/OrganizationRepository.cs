using Business.Model.Entities.Organizations;
using Xtech.Common.Pagination;
using Business.Application.Repositories;

namespace Storage.MsSql.Repositories
{
    /// <summary>
    /// Repository implementation for managing art organizations
    /// </summary>
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ArtBookingDbContextMsSql _dbContext;

        public OrganizationRepository(ArtBookingDbContextMsSql dbContext)
        {
            _dbContext = dbContext;
        }

        public ArtOrganization? GetById(int id) => _dbContext.ArtOrganizations.Find(id);

        public ArtOrganization Add(ArtOrganization organization)
        {
            var entity = _dbContext.ArtOrganizations.Add(organization).Entity;
            _dbContext.SaveChanges();
            return entity;
        }

        public ArtOrganization Update(ArtOrganization organization)
        {
            var entity = _dbContext.ArtOrganizations.Update(organization).Entity;
            _dbContext.SaveChanges();
            return entity;
        }

        public bool Delete(int id)
        {
            var org = _dbContext.ArtOrganizations.Find(id);
            if (org == null) return false;
            _dbContext.ArtOrganizations.Remove(org);
            _dbContext.SaveChanges();
            return true;
        }

        public PagedList<ArtOrganization> GetPagedList(
            string? organizationName,
            OrganizationKind? organizationKind,
            int pageNumber = 1,
            int pageSize = 999999,
            string? sortBy = null,
            bool sortAscending = true)
        {
            var query = _dbContext.ArtOrganizations.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(organizationName))
                query = query.Where(o => o.Name.ToLower().Contains(organizationName.ToLower()));
            if (organizationKind.HasValue)
                query = query.Where(o => o.Kind == organizationKind.Value);

            // Apply sorting
            query = ApplySorting(query, sortBy, sortAscending);

            return query.AsPagedList(pageNumber, pageSize);
        }

        public IEnumerable<ArtOrganization> GetAll() => _dbContext.ArtOrganizations.ToList();

        public bool Exists(int id) => _dbContext.ArtOrganizations.Any(o => o.ArtOrganizationId == id);

        private IQueryable<ArtOrganization> ApplySorting(
            IQueryable<ArtOrganization> query,
            string? sortBy,
            bool sortAscending)
        {
            return (sortBy?.ToLower(), sortAscending) switch
            {
                ("name", true) => query.OrderBy(o => o.Name),
                ("name", false) => query.OrderByDescending(o => o.Name),
                ("created-at", true) => query.OrderBy(o => o.CreatedAt),
                ("created-at", false) => query.OrderByDescending(o => o.CreatedAt),
                _ => throw new ArgumentException("Unsupported sort field")
            };
        }
    }
}