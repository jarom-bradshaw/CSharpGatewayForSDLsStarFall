using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Starfall.EventGateway.Api.Controllers;
using Starfall.EventGateway.Data.Abstractions;
using Xunit;

namespace Starfall.EventGateway.Tests.Controllers;

public sealed class HealthControllerTests
{
    [Fact]
    public async Task Get_WhenDbFactoryThrows_Returns503()
    {
        var controller = new HealthController(new ThrowingDbConnectionFactory(), NullLogger<HealthController>.Instance);

        var result = await controller.Get(cancellationToken: default);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, objectResult.StatusCode);
    }

    private sealed class ThrowingDbConnectionFactory : IDbConnectionFactory
    {
        public Task<Npgsql.NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("boom");
    }
}

