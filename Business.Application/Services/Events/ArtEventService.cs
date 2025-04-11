using Business.Application.DTOs.Events;
using Business.Model.Data;
using Business.Model.Entities.Events;
using Xtech.Common.Pagination;
using AutoMapper;
using Business.Application.UserIdentity;

namespace Business.Application.Services.Events
{
    public class ArtEventService : IArtEventService
    {
        private readonly ArtBookingDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public ArtEventService(ArtBookingDbContext dbContext, IMapper mapper, IUserContext userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContext = userContext;
        }

        public ArtEventDto CreateEvent(CreateArtEventDto eventDto, int? artOrganizationId = null)
        {
            var artEvent = _mapper.Map<ArtEvent>(eventDto);
            artEvent.CreatedAt = DateTime.UtcNow;
            artEvent.CreatedById = _userContext.UserId;
            artEvent.UpdatedAt = DateTime.UtcNow;
            artEvent.UpdatedById = _userContext.UserId;
            artEvent.ArtOrganizationId = artOrganizationId ?? 0; // Set to 0 if null

            _dbContext.ArtEvents.Add(artEvent);
            _dbContext.SaveChanges();

            return _mapper.Map<ArtEventDto>(artEvent);
        }

        public ArtEventDto GetEvent(int id)
        {
            var artEvent = _dbContext.ArtEvents.Find(id);
            return artEvent != null ? _mapper.Map<ArtEventDto>(artEvent) : null;
        }

        public PagedList<ArtEventDto> ListEvents(PagedListParams<ArtEventFilters> listParams)
        {
            var query = _dbContext.ArtEvents.AsQueryable();

            if (listParams.Filters != null)
            {
                if (!string.IsNullOrEmpty(listParams.Filters.Name))
                {
                    query = query.Where(e => e.Name.ToLower().Contains(listParams.Filters.Name.ToLower()));
                }

                if (listParams.Filters.Category.HasValue)
                {
                    query = query.Where(e => e.Category == listParams.Filters.Category.Value);
                }

                if (listParams.Filters.Status.HasValue)
                {
                    query = query.Where(e => e.Status == listParams.Filters.Status.Value);
                }

                if (listParams.Filters.OrganizationId.HasValue)
                {
                    query = query.Where(e => e.ArtOrganizationId == listParams.Filters.OrganizationId.Value);
                }
            }

            if (listParams.HasSort())
            {
                if (listParams.SortByFieldIs("Name"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(e => e.Name)
                        : query.OrderByDescending(e => e.Name);
                }
                else if (listParams.SortByFieldIs("Status"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(e => e.Status)
                        : query.OrderByDescending(e => e.Status);
                }
                else if (listParams.SortByFieldIs("CreatedAt"))
                {
                    query = listParams.IsSortByAsc()
                        ? query.OrderBy(e => e.CreatedAt)
                        : query.OrderByDescending(e => e.CreatedAt);
                }
                else
                {
                    query = query.OrderBy(e => e.Name);
                }
            }
            else
            {
                query = query.OrderBy(e => e.Name);
            }

            var pagedList = query.AsPagedList(listParams.PageNumber, listParams.PageSize);
            return new PagedList<ArtEventDto>(_mapper.Map<List<ArtEventDto>>(pagedList.Items), pagedList.TotalCount, pagedList.PageNumber, pagedList.PageSize);
        }

        public ArtEventDto EditEvent(int id, CreateArtEventDto eventDto)
        {
            var existingEvent = _dbContext.ArtEvents.Find(id);
            if (existingEvent == null)
            {
                return null;
            }

            _mapper.Map(eventDto, existingEvent);
            existingEvent.UpdatedAt = DateTime.UtcNow;
            existingEvent.UpdatedById = _userContext.UserId;
            _dbContext.SaveChanges();

            return _mapper.Map<ArtEventDto>(existingEvent);
        }

        public void DeleteEvent(int id)
        {
            var artEvent = _dbContext.ArtEvents.Find(id);
            if (artEvent != null)
            {
                _dbContext.ArtEvents.Remove(artEvent);
                _dbContext.SaveChanges();
            }
        }
    }
}