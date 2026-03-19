using Microsoft.AspNetCore.Mvc;
using Starfall.EventGateway.Application.Abstractions;
using Starfall.EventGateway.Contracts;

namespace Starfall.EventGateway.Api.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private const int MaxPageSize = 200;

    private readonly IEventService _service;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IEventService service, ILogger<EventsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<EventSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<EventSummaryDto>>> GetEvents(
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 0)
        {
            return BadRequest("pageNumber must be >= 0.");
        }

        if (pageSize is < 1 or > MaxPageSize)
        {
            return BadRequest($"pageSize must be between 1 and {MaxPageSize}.");
        }

        _logger.LogInformation("Listing events pageNumber={PageNumber} pageSize={PageSize}", pageNumber, pageSize);
        var result = await _service.GetEventsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{eventId:guid}")]
    [ProducesResponseType(typeof(EventDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventDetailsDto>> GetEventDetails(
        [FromRoute] Guid eventId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching event details eventId={EventId}", eventId);
        var result = await _service.GetEventDetailsAsync(eventId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{eventId:guid}/history")]
    [ProducesResponseType(typeof(IReadOnlyList<EventHistoryItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<EventHistoryItemDto>>> GetEventHistory(
        [FromRoute] Guid eventId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching event history eventId={EventId}", eventId);
        var details = await _service.GetEventDetailsAsync(eventId, cancellationToken);
        if (details is null)
        {
            return NotFound();
        }

        var history = await _service.GetEventHistoryAsync(eventId, cancellationToken);
        return Ok(history);
    }
}

