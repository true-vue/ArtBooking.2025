using Business.Model.Entities.Organizations;
using Xtech.Common.Pagination;
using Business.Application.DTOs.Organizations;
using AutoMapper;
using Business.Application.UserIdentity;
using Business.Application.Repositories;

namespace Business.Application.Services.Organizations
{
    /// <summary>
    /// Service for handling Art Organization operations
    /// </summary>
    public class ArtOrganizationService : IArtOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public ArtOrganizationService(
            IOrganizationRepository organizationRepository,
            IMapper mapper,
            IUserContext userContext)
        {
            _organizationRepository = organizationRepository;
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

            var createdOrganization = _organizationRepository.Add(organization);
            return _mapper.Map<ArtOrganizationDto>(createdOrganization);
        }

        /// <summary>
        /// Gets an art organization by its ID
        /// </summary>
        /// <param name="id">The ID of the organization to retrieve</param>
        /// <returns>The organization DTO if found, null otherwise</returns>
        public ArtOrganizationDto? GetOrganization(int id)
        {
            var organization = _organizationRepository.GetById(id);
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
            var existingOrganization = _organizationRepository.GetById(id);
            if (existingOrganization == null)
            {
                return null;
            }

            // Update properties using AutoMapper
            _mapper.Map(organizationDto, existingOrganization);

            // Update audit fields
            existingOrganization.UpdatedAt = DateTime.UtcNow;
            existingOrganization.UpdatedById = _userContext.UserId;

            var updatedOrganization = _organizationRepository.Update(existingOrganization);
            return _mapper.Map<ArtOrganizationDto>(updatedOrganization);
        }

        /// <summary>
        /// List organizations with pagination, filtering, and sorting
        /// </summary>
        /// <param name="listParams">List parameters including pagination, filters, and sorting</param>
        /// <returns>A paged list of art organization DTOs</returns>
        public PagedList<ArtOrganizationDto> ListOrganizations(PagedListParams<ArtOrganizationFilters> listParams)
        {
            var pagedOrganizations = _organizationRepository.GetPagedList(
                listParams.Filters?.Name,
                listParams.Filters?.Kind,
                listParams.PageNumber,
                listParams.PageSize,
                listParams.SortBy,
                listParams.IsSortByAsc()
            );

            var dtoItems = _mapper.Map<List<ArtOrganizationDto>>(pagedOrganizations.Items);
            return new PagedList<ArtOrganizationDto>(
                dtoItems,
                pagedOrganizations.TotalCount,
                pagedOrganizations.PageNumber,
                pagedOrganizations.PageSize
            );
        }

        /// <summary>
        /// Checks if there are any organizations present
        /// </summary>
        /// <returns>True if there are organizations, false otherwise</returns>
        public bool HasOrganizations()
        {
            return _organizationRepository.GetAll().Any();
        }
    }
}
