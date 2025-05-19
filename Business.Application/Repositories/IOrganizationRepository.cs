using Business.Model.Entities.Organizations;
using Xtech.Common.Pagination;

namespace Business.Application.Repositories
{
    /// <summary>
    /// Repository interface for managing art organizations
    /// </summary>
    public interface IOrganizationRepository
    {
        /// <summary>
        /// Gets an art organization by its ID
        /// </summary>
        /// <param name="id">The ID of the organization to retrieve</param>
        /// <returns>The organization if found, null otherwise</returns>
        ArtOrganization? GetById(int id);

        /// <summary>
        /// Adds a new art organization to the repository
        /// </summary>
        /// <param name="organization">The organization entity to be added</param>
        /// <returns>The added organization with ID</returns>
        ArtOrganization Add(ArtOrganization organization);

        /// <summary>
        /// Updates an existing art organization
        /// </summary>
        /// <param name="organization">The organization entity with updated data</param>
        /// <returns>The updated organization</returns>
        ArtOrganization Update(ArtOrganization organization);

        /// <summary>
        /// Deletes an art organization
        /// </summary>
        /// <param name="id">The ID of the organization to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        bool Delete(int id);

        /// <summary>
        /// Gets a paged list of organizations with filtering and sorting
        /// </summary>
        /// <param name="OrganizationName">Optional name filter for organizations</param>
        /// <param name="OrganizationKind">Optional kind filter for organizations</param>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="sortBy">Field to sort by</param>
        /// <param name="sortAscending">Whether to sort in ascending order</param>
        /// <returns>A paged list of organizations</returns>
        PagedList<ArtOrganization> GetPagedList(
            string? organizationName,
            OrganizationKind? organizationKind,
            int pageNumber = 1,
            int pageSize = 999999,
            string? sortBy = null,
            bool sortAscending = true);

        /// <summary>
        /// Gets all organizations
        /// </summary>
        /// <returns>A list of all organizations</returns>
        IEnumerable<ArtOrganization> GetAll();

        /// <summary>
        /// Checks if an organization exists by its ID
        /// </summary>
        /// <param name="id">The ID to check</param>
        /// <returns>True if the organization exists, false otherwise</returns>
        bool Exists(int id);
    }
}