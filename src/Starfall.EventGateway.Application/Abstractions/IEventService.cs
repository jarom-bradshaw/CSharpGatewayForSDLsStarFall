using Starfall.EventGateway.Contracts;

namespace Starfall.EventGateway.Application.Abstractions;

public interface IEventService
{
    Task<PagedResult<EventSummaryDto>> GetEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<EventDetailsDto?> GetEventDetailsAsync(Guid eventId, CancellationToken cancellationToken);
    Task<IReadOnlyList<EventHistoryItemDto>> GetEventHistoryAsync(Guid eventId, CancellationToken cancellationToken);
}

