using Asp.Versioning;
using Bookify.Application.Apartments;
using Bookify.Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers.Apartments;



[ApiController]
[ApiVersion(ApiVersion.v1)]

// [ApiVersion(ApiVersion.v2)]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApartmentController : ControllerBase
{
    private readonly ISender _mediator;

    public ApartmentController(ISender mediator)
    {
        _mediator = mediator;
    }

        [HttpGet]
        // [MapToApiVersion(ApiVersion.v1)]
        public async Task<IActionResult> SearchApartments (
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken = default
            )
        {
            // Task.Delay(1000).Wait();
            // return Ok();
        var query =new SearchApartmentQuery(startDate, endDate);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result  );
        }

        // [HttpGet]
        // [MapToApiVersion(ApiVersion.v2)]
        // public async Task<IActionResult> SearchApartmentsV2(
        //     DateOnly startDate,
        //     DateOnly endDate,
        //     CancellationToken cancellationToken = default
        // )
        // {
        //     // Task.Delay(1000).Wait();
        //     // return Ok();
        //     var query =new SearchApartmentQuery(startDate, endDate);
        //     var result = await _mediator.Send(query, cancellationToken);
        //     return Ok(result  );
        // }

        // [HttpGet("id")]
        // public async Task<IActionResult> GetApartmentById(
        //     int  id,
        //     CancellationToken cancellationToken = default)
        // {
        //     Guid id1 = new Guid("97A8CD18-D15E-1D88-B740-004905D688F3");
        //     var query = new GetApartmentQuery(id1);
        //     var result = await _mediator.Send(query, cancellationToken);
        //     return Ok(result);
        // }
}