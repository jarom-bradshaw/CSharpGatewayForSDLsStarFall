using Starfall.EventGateway.Contracts;

namespace Starfall.EventGateway.Application.Abstractions;

public interface IEventRepository
{
    Task<PagedResult<EventSummaryDto>> GetEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<EventSummaryDto?> GetEventByIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task<IReadOnlyList<EventHistoryItemDto>> GetEventHistoryAsync(Guid eventId, CancellationToken cancellationToken);
}

