using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Starfall.EventGateway.Api.Controllers;
using Starfall.EventGateway.Application.Abstractions;
using Starfall.EventGateway.Contracts;
using Xunit;

namespace Starfall.EventGateway.Tests.Controllers;

public sealed class EventsControllerTests
{
    [Fact]
    public async Task GetEvents_PageNumberNegative_ReturnsBadRequest()
    {
        var controller = new EventsController(new FakeEventService(), NullLogger<EventsController>.Instance);

        var result = await controller.GetEvents(pageNumber: -1, pageSize: 25, cancellationToken: default);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(201)]
    public async Task GetEvents_PageSizeInvalid_ReturnsBadRequest(int pageSize)
    {
        var controller = new EventsController(new FakeEventService(), NullLogger<EventsController>.Instance);

        var result = await controller.GetEvents(pageNumber: 0, pageSize: pageSize, cancellationToken: default);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetEventDetails_WhenMissing_ReturnsNotFound()
    {
        var controller = new EventsController(new FakeEventService(), NullLogger<EventsController>.Instance);

        var result = await controller.GetEventDetails(Guid.NewGuid(), cancellationToken: default);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetEventHistory_WhenMissingEvent_ReturnsNotFound()
    {
        var controller = new EventsController(new FakeEventService(), NullLogger<EventsController>.Instance);

        var result = await controller.GetEventHistory(Guid.NewGuid(), cancellationToken: default);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    private sealed class FakeEventService : IEventService
    {
        public Task<PagedResult<EventSummaryDto>> GetEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken) =>
            Task.FromResult(new PagedResult<EventSummaryDto>(Array.Empty<EventSummaryDto>(), totalCount: 0, pageNumber, pageSize));

        public Task<EventDetailsDto?> GetEventDetailsAsync(Guid eventId, CancellationToken cancellationToken) =>
            Task.FromResult<EventDetailsDto?>(null);

        public Task<IReadOnlyList<EventHistoryItemDto>> GetEventHistoryAsync(Guid eventId, CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<EventHistoryItemDto>>(Array.Empty<EventHistoryItemDto>());
    }
}

