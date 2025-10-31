using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Application.CQRS.Queries;
using static ZyxLogistica.Application.Models.TruckModel;

namespace ZyxLogistica.API.Controllers;

[ApiController]
[Route("api/truck")]
public class TruckController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        => Ok(await _mediator.Send(new TruckQuery.GetTrucksByDateRangeQuery(startDate, endDate)));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TruckInput truckInput)
        => Ok(await _mediator.Send(new TruckCommand.CreateTruckCommand(truckInput)));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] TruckUpdate truckUpdate)
        => Ok(await _mediator.Send(new TruckCommand.UpdateTruckCommand(id, truckUpdate)));

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableTrucks()
    => Ok(await _mediator.Send(new TruckQuery.GetAvailableTrucksQuery()));
}
