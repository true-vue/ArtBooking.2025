using Business.Application.DTOs.Organizations;
using Xtech.Common.Pagination;

namespace Business.Application.Services.Organizations
{
    /// <summary>
    /// Interface for Art Organization service operations
    /// </summary>
    public interface IArtOrganizationService
    {
        /// <summary>
        /// Creates a new art organization
        /// </summary>
        /// <param name="organizationDto">The organization DTO with data for creation</param>
        /// <returns>The created organization DTO with ID</returns>
        ArtOrganizationDto CreateOrganization(CreateArtOrganizationDto organizationDto);

        /// <summary>
        /// Gets an art organization by its ID
        /// </summary>
        /// <param name="id">The ID of the organization to retrieve</param>
        /// <returns>The organization DTO if found, null otherwise</returns>
        ArtOrganizationDto? GetOrganization(int id);

        /// <summary>
        /// Updates an existing art organization
        /// </summary>
        /// <param name="id">The ID of the organization to update</param>
        /// <param name="organizationDto">The updated organization data</param>
        /// <returns>The updated organization DTO if successful, null if organization not found</returns>
        ArtOrganizationDto? EditOrganization(int id, CreateArtOrganizationDto organizationDto);

        /// <summary>
        /// List organizations with pagination, filtering, and sorting
        /// </summary>
        /// <param name="listParams">List parameters including pagination, filters, and sorting</param>
        /// <returns>A paged list of art organization DTOs</returns>
        PagedList<ArtOrganizationDto> ListOrganizations(PagedListParams<ArtOrganizationFilters> listParams);

        /// <summary>
        /// Checks if there are any organizations in the database.
        /// </summary>
        /// <returns>True if organizations exist, otherwise false.</returns>
        bool HasOrganizations();
    }
}
