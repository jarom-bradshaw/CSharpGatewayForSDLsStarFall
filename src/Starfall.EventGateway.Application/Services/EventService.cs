using Starfall.EventGateway.Application.Abstractions;
using Starfall.EventGateway.Contracts;

namespace Starfall.EventGateway.Application.Services;

public sealed class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public Task<PagedResult<EventSummaryDto>> GetEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken) =>
        _repository.GetEventsAsync(pageNumber, pageSize, cancellationToken);

    public async Task<EventDetailsDto?> GetEventDetailsAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var summary = await _repository.GetEventByIdAsync(eventId, cancellationToken);
        return summary is null ? null : new EventDetailsDto(summary);
    }

    public Task<IReadOnlyList<EventHistoryItemDto>> GetEventHistoryAsync(Guid eventId, CancellationToken cancellationToken) =>
        _repository.GetEventHistoryAsync(eventId, cancellationToken);
}

