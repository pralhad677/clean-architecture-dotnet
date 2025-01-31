using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers.Bookings;


[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly ISender _mediator;

    public BookingController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(" {id}")]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        // return result.Success ? Ok(result) : NotFound( );
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate
        );
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }
}