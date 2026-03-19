namespace Starfall.EventGateway.Data.Sql;

internal static class EventSql
{
    internal const string GetEventsPage = """
        SELECT
            event_id AS EventId,
            approx_trigger_time AS ApproxTriggerTimeSeconds,
            created_time AS CreatedTimeSeconds,
            last_update_time AS LastUpdateTimeSeconds,
            processing_state AS ProcessingState,
            user_viewed AS UserViewed,
            approx_energy_j AS ApproxEnergyJ,
            CASE
                WHEN location_ecef_m IS NULL
                  OR location_ecef_m[1] IS NULL
                  OR location_ecef_m[2] IS NULL
                  OR location_ecef_m[3] IS NULL
                THEN NULL
                ELSE location_ecef_m
            END AS LocationEcefM,
            CASE
                WHEN velocity_ecef_m_sec IS NULL
                  OR velocity_ecef_m_sec[1] IS NULL
                  OR velocity_ecef_m_sec[2] IS NULL
                  OR velocity_ecef_m_sec[3] IS NULL
                THEN NULL
                ELSE velocity_ecef_m_sec
            END AS VelocityEcefMSec
        FROM starfall_db_schema.events
        ORDER BY approx_trigger_time DESC
        LIMIT @PageSize OFFSET @Offset;
        """;

    internal const string GetEventsCount = """
        SELECT COUNT(*) FROM starfall_db_schema.events;
        """;

    internal const string GetEventById = """
        SELECT
            event_id AS EventId,
            approx_trigger_time AS ApproxTriggerTimeSeconds,
            created_time AS CreatedTimeSeconds,
            last_update_time AS LastUpdateTimeSeconds,
            processing_state AS ProcessingState,
            user_viewed AS UserViewed,
            approx_energy_j AS ApproxEnergyJ,
            CASE
                WHEN location_ecef_m IS NULL
                  OR location_ecef_m[1] IS NULL
                  OR location_ecef_m[2] IS NULL
                  OR location_ecef_m[3] IS NULL
                THEN NULL
                ELSE location_ecef_m
            END AS LocationEcefM,
            CASE
                WHEN velocity_ecef_m_sec IS NULL
                  OR velocity_ecef_m_sec[1] IS NULL
                  OR velocity_ecef_m_sec[2] IS NULL
                  OR velocity_ecef_m_sec[3] IS NULL
                THEN NULL
                ELSE velocity_ecef_m_sec
            END AS VelocityEcefMSec
        FROM starfall_db_schema.events
        WHERE event_id = @EventId;
        """;

    internal const string GetEventHistoryByEventId = """
        SELECT
            history_id AS HistoryId,
            event_id AS EventId,
            time AS TimeSeconds,
            entry AS Entry,
            author AS Author
        FROM starfall_db_schema.history
        WHERE event_id = @EventId
        ORDER BY time ASC;
        """;
}

