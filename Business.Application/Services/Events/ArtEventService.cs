using Business.Application.DTOs.Events;
using Business.Model.Entities.Events;
using Xtech.Common.Pagination;
using AutoMapper;
using Business.Application.UserIdentity;
using Business.Application.Repositories;

namespace Business.Application.Services.Events
{
    public class ArtEventService : IArtEventService
    {
        private readonly IArtEventRepository _artEventRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public ArtEventService(
            IArtEventRepository artEventRepository,
            IMapper mapper,
            IUserContext userContext)
        {
            _artEventRepository = artEventRepository;
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

            _artEventRepository.Add(artEvent);
            return _mapper.Map<ArtEventDto>(artEvent);
        }

        public ArtEventDto GetEvent(int id)
        {
            var artEvent = _artEventRepository.GetById(id);
            return artEvent != null ? _mapper.Map<ArtEventDto>(artEvent) : null;
        }

        public PagedList<ArtEventDto> ListEvents(PagedListParams<ArtEventFilters> listParams)
        {
            var pagedEvents = _artEventRepository.GetPagedList(
                listParams.Filters?.Name,
                (int?)listParams.Filters?.Category,
                (int?)listParams.Filters?.Status,
                listParams.Filters?.OrganizationId,
                listParams.PageNumber,
                listParams.PageSize,
                listParams.SortBy,
                listParams.IsSortByAsc()
            );

            var dtoItems = _mapper.Map<List<ArtEventDto>>(pagedEvents.Items);
            return new PagedList<ArtEventDto>(
                dtoItems,
                pagedEvents.TotalCount,
                pagedEvents.PageNumber,
                pagedEvents.PageSize
            );
        }

        public ArtEventDto EditEvent(int id, CreateArtEventDto eventDto)
        {
            var existingEvent = _artEventRepository.GetById(id);
            if (existingEvent == null)
            {
                return null;
            }

            _mapper.Map(eventDto, existingEvent);
            existingEvent.UpdatedAt = DateTime.UtcNow;
            existingEvent.UpdatedById = _userContext.UserId;

            var updatedEvent = _artEventRepository.Update(existingEvent);
            return _mapper.Map<ArtEventDto>(updatedEvent);
        }

        public void DeleteEvent(int id)
        {
            var artEvent = _artEventRepository.GetById(id);
            if (artEvent != null)
            {
                _artEventRepository.Delete(artEvent);
            }
        }
    }
}