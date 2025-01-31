using Bookify.Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers.Apartments;



[ApiController]
[Route("api/[controller]")]
public class ApartmentController : ControllerBase
{
    private readonly ISender _mediator;

    public ApartmentController(ISender mediator)
    {
        _mediator = mediator;
    }

        [HttpGet]
        public async Task<IActionResult> SearchApartments(
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken = default
            )
        {
            // Task.Delay(1000).Wait();
            // return Ok();
        var query =new SerachApartmentQuery(startDate, endDate);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result  );
        }
}