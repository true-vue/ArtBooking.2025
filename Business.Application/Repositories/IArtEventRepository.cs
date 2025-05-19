using Business.Model.Entities.Events;
using Xtech.Common.Pagination;

namespace Business.Application.Repositories
{
    /// <summary>
    /// Interface for managing ArtEvent entities in the repository.
    /// </summary>
    public interface IArtEventRepository
    {
        /// <summary>
        /// Adds a new ArtEvent to the repository.
        /// </summary>
        /// <param name="artEvent">The ArtEvent to add.</param>
        void Add(ArtEvent artEvent);

        /// <summary>
        /// Updates an existing ArtEvent in the repository.
        /// </summary>
        /// <param name="artEvent">The ArtEvent to update.</param>
        /// <returns>The updated ArtEvent.</returns>
        ArtEvent Update(ArtEvent artEvent);

        /// <summary>
        /// Retrieves an ArtEvent by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ArtEvent.</param>
        /// <returns>The ArtEvent with the specified ID.</returns>
        ArtEvent GetById(int id);

        /// <summary>
        /// Deletes an ArtEvent from the repository.
        /// </summary>
        /// <param name="artEvent">The ArtEvent to delete.</param>
        void Delete(ArtEvent artEvent);

        /// <summary>
        /// Retrieves a paginated list of ArtEvents based on the provided filters.
        /// </summary>
        /// <param name="nameFilter">Optional filter for the name of the ArtEvent.</param>
        /// <param name="categoryFilter">Optional filter for the category of the ArtEvent.</param>
        /// <param name="statusFilter">Optional filter for the status of the ArtEvent.</param>
        /// <param name="organizationIdFilter">Optional filter for the organization ID associated with the ArtEvent.</param>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 999999).</param>
        /// <param name="sortBy">Optional field to sort by.</param>
        /// <param name="sortAscending">Specifies whether sorting is ascending (default is true).</param>
        /// <returns>A paginated list of ArtEvents.</returns>
        PagedList<ArtEvent> GetPagedList(
            string? nameFilter = null,
            int? categoryFilter = null,
            int? statusFilter = null,
            int? organizationIdFilter = null,
            int pageNumber = 1,
            int pageSize = 999999,
            string? sortBy = null,
            bool sortAscending = true
        );

        /// <summary>
        /// Retrieves all ArtEvents from the repository.
        /// </summary>
        /// <returns>An enumerable collection of all ArtEvents.</returns>
        IEnumerable<ArtEvent> GetAll();

        /// <summary>
        /// Checks if an ArtEvent exists in the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ArtEvent.</param>
        /// <returns>True if the ArtEvent exists, otherwise false.</returns>
        bool Exists(int id);
    }
}
