using Business.Model.Entities.Organizations;
using Xtech.Common.Pagination;
using Business.Application.DTOs.Organizations;
using AutoMapper;
using Business.Application.UserIdentity;
using Storage.InMemory;

namespace Business.Application.Services.Organizations
{
    /// <summary>
    /// Service for handling Art Organization operations
    /// </summary>
    public class ArtOrganizationService : IArtOrganizationService
    {
        private readonly ArtBookingDbContextInMemory _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public ArtOrganizationService(ArtBookingDbContextInMemory dbContext, IMapper mapper, IUserContext userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContext = userContext;
        }

        /// <summary>
        /// Creates a new art organization
        /// </summary>
        /// <param name="organizationDto">The organization DTO with data for creation</param>
        /// <returns>The created organization DTO with ID</returns>
        /// <exception cref="Exception">Thrown when an error occurs during organization creation</exception>
        public ArtOrganizationDto CreateOrganization(CreateArtOrganizationDto organizationDto)
        {
            var organization = _mapper.Map<ArtOrganization>(organizationDto);

            // Set creation and update properties
            organization.CreatedAt = DateTime.Now;
            organization.CreatedById = _userContext.UserId;
            organization.UpdatedAt = DateTime.Now;
            organization.UpdatedById = _userContext.UserId;

            _dbContext.Add(organization);
            _dbContext.SaveChanges();
            return _mapper.Map<ArtOrganizationDto>(organization);
        }

        /// <summary>
        /// Gets an art organization by its ID
        /// </summary>
        /// <param name="id">The ID of the organization to retrieve</param>
        /// <returns>The organization DTO if found, null otherwise</returns>
        public ArtOrganizationDto? GetOrganization(int id)
        {
            var organization = _dbContext.ArtOrganizations.Find(id);
            return organization != null ? _mapper.Map<ArtOrganizationDto>(organization) : null;
        }

        /// <summary>
        /// Updates an existing art organization
        /// </summary>
        /// <param name="id">The ID of the organization to update</param>
        /// <param name="organizationDto">The updated organization data</param>
        /// <returns>The updated organization DTO if successful, null if organization not found</returns>
        public ArtOrganizationDto? EditOrganization(int id, CreateArtOrganizationDto organizationDto)
        {
            var existingOrganization = _dbContext.ArtOrganizations.Find(id);
            if (existingOrganization == null)
            {
                return null;
            }

            // Update properties using AutoMapper
            _mapper.Map(organizationDto, existingOrganization);

            // Update audit fields
            existingOrganization.UpdatedAt = DateTime.UtcNow;
            existingOrganization.UpdatedById = _userContext.UserId;

            _dbContext.SaveChanges();
            return _mapper.Map<ArtOrganizationDto>(existingOrganization);
        }

        /// <summary>
        /// List organizations with pagination, filtering, and sorting
        /// </summary>
        /// <param name="listParams">List parameters including pagination, filters, and sorting</param>
        /// <returns>A paged list of art organization DTOs</returns>
        public PagedList<ArtOrganizationDto> ListOrganizations(PagedListParams<ArtOrganizationFilters> listParams)
        {
            var query = _dbContext.ArtOrganizations.AsQueryable();

            // Apply filters if provided
            if (listParams.Filters != null)
            {
                // Filter by name (case-insensitive partial match)
                if (!string.IsNullOrEmpty(listParams.Filters.Name))
                {
                    query = query.Where(o => o.Name.ToLower().Contains(listParams.Filters.Name.ToLower()));
                }

                // Filter by kind
                if (listParams.Filters.Kind.HasValue)
                {
                    query = query.Where(o => o.Kind == listParams.Filters.Kind.Value);
                }
            }

            // Apply sorting
            if (listParams.HasSort())
            {
                if (listParams.SortByFieldIs("OrganizationName"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(o => o.Name)
                        : query.OrderByDescending(o => o.Name);
                }
                else if (listParams.SortByFieldIs("CreatedAt"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(o => o.CreatedAt)
                        : query.OrderByDescending(o => o.CreatedAt);
                }
                // Default sorting by Name ascending if sort field is not recognized
                else
                {
                    query = query.OrderBy(o => o.Name);
                }
            }
            else
            {
                // Default sorting by Name if no sort specified
                query = query.OrderBy(o => o.Name);
            }

            // First get the paged entities
            var pagedEntities = query.AsPagedList(listParams.PageNumber, listParams.PageSize);

            // Then convert to DTOs using AutoMapper
            var dtoItems = _mapper.Map<List<ArtOrganizationDto>>(pagedEntities.Items);

            // Create a new paged list with the DTOs
            return new PagedList<ArtOrganizationDto>(
                dtoItems,
                pagedEntities.TotalCount,
                pagedEntities.PageNumber,
                pagedEntities.PageSize
            );
        }

        /// <summary>
        /// Checks if there are any organizations present
        /// </summary>
        /// <returns>True if there are organizations, false otherwise</returns>
        public bool HasOrganizations()
        {
            return _dbContext.ArtOrganizations.Any();
        }
    }
}
