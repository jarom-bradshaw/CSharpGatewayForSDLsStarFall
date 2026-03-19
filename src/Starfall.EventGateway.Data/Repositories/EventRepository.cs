using Dapper;
using Starfall.EventGateway.Contracts;
using Starfall.EventGateway.Application.Abstractions;
using Starfall.EventGateway.Data.Abstractions;
using Starfall.EventGateway.Data.Sql;

namespace Starfall.EventGateway.Data.Repositories;

public sealed class EventRepository : IEventRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EventRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PagedResult<EventSummaryDto>> GetEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var offset = checked(pageNumber * pageSize);

        var totalCount = await conn.ExecuteScalarAsync<int>(new CommandDefinition(
            EventSql.GetEventsCount,
            cancellationToken: cancellationToken
        ));

        var rows = await conn.QueryAsync<EventRow>(new CommandDefinition(
            EventSql.GetEventsPage,
            parameters: new { PageSize = pageSize, Offset = offset },
            cancellationToken: cancellationToken
        ));

        var data = rows.Select(r => r.ToDto()).ToList();

        return new PagedResult<EventSummaryDto>(
            Data: data,
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize
        );
    }

    public async Task<EventSummaryDto?> GetEventByIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var row = await conn.QuerySingleOrDefaultAsync<EventRow>(new CommandDefinition(
            EventSql.GetEventById,
            parameters: new { EventId = eventId },
            cancellationToken: cancellationToken
        ));

        return row?.ToDto();
    }

    public async Task<IReadOnlyList<EventHistoryItemDto>> GetEventHistoryAsync(Guid eventId, CancellationToken cancellationToken)
    {
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var rows = await conn.QueryAsync<EventHistoryRow>(new CommandDefinition(
            EventSql.GetEventHistoryByEventId,
            parameters: new { EventId = eventId },
            cancellationToken: cancellationToken
        ));

        return rows
            .Select(r => new EventHistoryItemDto(
                HistoryId: r.HistoryId,
                EventId: r.EventId,
                Time: EventRow.FromEpochSeconds(r.TimeSeconds),
                Entry: r.Entry,
                Author: r.Author
            ))
            .ToList();
    }

    private sealed class EventRow
    {
        public Guid EventId { get; init; }
        public double ApproxTriggerTimeSeconds { get; init; }
        public double CreatedTimeSeconds { get; init; }
        public double LastUpdateTimeSeconds { get; init; }
        public int ProcessingState { get; init; }
        public bool UserViewed { get; init; }
        public double? ApproxEnergyJ { get; init; }

        // Npgsql may return arrays as System.Array here; normalize in ToDto().
        public object? LocationEcefM { get; init; }
        public object? VelocityEcefMSec { get; init; }

        public EventSummaryDto ToDto() => new(
            EventId: EventId,
            ApproxTriggerTime: FromEpochSeconds(ApproxTriggerTimeSeconds),
            CreatedTime: FromEpochSeconds(CreatedTimeSeconds),
            LastUpdateTime: FromEpochSeconds(LastUpdateTimeSeconds),
            ProcessingState: ProcessingState,
            UserViewed: UserViewed,
            ApproxEnergyJ: ApproxEnergyJ,
            LocationEcefM: ToDoubleArrayOrNull(LocationEcefM),
            VelocityEcefMSec: ToDoubleArrayOrNull(VelocityEcefMSec),
            LatitudeDeg: null,
            LongitudeDeg: null,
            AltitudeM: null
        );

        internal static DateTimeOffset FromEpochSeconds(double seconds) =>
            DateTimeOffset.FromUnixTimeMilliseconds((long)Math.Round(seconds * 1000d));

        private static double[]? ToDoubleArrayOrNull(object? value) =>
            value switch
            {
                null => null,
                double[] d => d,
                float[] f => f.Select(x => (double)x).ToArray(),
                Array a => a.Cast<object?>()
                    .Select(o => o is null ? (double?)null : Convert.ToDouble(o))
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .ToArray(),
                _ => null
            };
    }

    private sealed class EventHistoryRow
    {
        public Guid HistoryId { get; init; }
        public Guid EventId { get; init; }
        public double TimeSeconds { get; init; }
        public string Entry { get; init; } = "";
        public string Author { get; init; } = "";
    }
}

