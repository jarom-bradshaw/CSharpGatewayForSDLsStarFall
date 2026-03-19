using Microsoft.Extensions.Configuration;
using Npgsql;
using Starfall.EventGateway.Data.Abstractions;

namespace Starfall.EventGateway.Data.Npgsql;

public sealed class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public NpgsqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString("StarfallDatabase");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'StarfallDatabase' is not configured.");
        }

        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}

