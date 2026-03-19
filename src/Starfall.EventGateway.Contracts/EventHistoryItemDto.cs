namespace Starfall.EventGateway.Contracts;

public sealed record EventHistoryItemDto(
    Guid HistoryId,
    Guid EventId,
    DateTimeOffset Time,
    string Entry,
    string Author
);

