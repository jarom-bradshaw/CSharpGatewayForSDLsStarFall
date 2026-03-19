namespace Starfall.EventGateway.Contracts;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Data,
    int TotalCount,
    int PageNumber,
    int PageSize
);

