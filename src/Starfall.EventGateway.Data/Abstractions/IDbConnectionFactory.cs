using Npgsql;

namespace Starfall.EventGateway.Data.Abstractions;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}

