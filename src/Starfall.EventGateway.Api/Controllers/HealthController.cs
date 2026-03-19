using Microsoft.AspNetCore.Mvc;
using Starfall.EventGateway.Data.Abstractions;

namespace Starfall.EventGateway.Api.Controllers;

[ApiController]
[Route("health")]
public sealed class HealthController : ControllerBase
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<HealthController> _logger;

    public HealthController(IDbConnectionFactory connectionFactory, ILogger<HealthController> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            await using var cmd = new Npgsql.NpgsqlCommand("SELECT 1", conn);
            _ = await cmd.ExecuteScalarAsync(cancellationToken);

            return Ok(new { status = "Healthy" });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Health check failed");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { status = "Unhealthy" });
        }
    }
}

