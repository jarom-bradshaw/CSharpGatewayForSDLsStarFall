namespace Starfall.EventGateway.Contracts;

public sealed record EventSummaryDto(
    Guid EventId,
    DateTimeOffset ApproxTriggerTime,
    DateTimeOffset CreatedTime,
    DateTimeOffset LastUpdateTime,
    int ProcessingState,
    bool UserViewed,
    double? ApproxEnergyJ,
    double[]? LocationEcefM,
    double[]? VelocityEcefMSec,
    double? LatitudeDeg,
    double? LongitudeDeg,
    double? AltitudeM
);

