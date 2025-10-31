using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Application.CQRS.Queries;
using static ZyxLogistica.Application.Models.DriverModel;

namespace ZyxLogistica.API.Controllers;

[ApiController]
[Route("api/driver")]
public class DriverController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        => Ok(await _mediator.Send(new DriverQuery.GetDriversByDateRangeQuery(startDate, endDate)));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DriverInput driverInput) 
        => Ok(await _mediator.Send(new DriverCommand.CreateDriverCommand(driverInput)));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] DriverUpdate driverUpdate)
        => Ok(await _mediator.Send(new DriverCommand.UpdateDriverCommand(id, driverUpdate)));

    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
        => Ok(await _mediator.Send(new DriverCommand.ToggleDriverStatusCommand(id)));

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableDrivers()
    => Ok(await _mediator.Send(new DriverQuery.GetAvailableDriversQuery()));
}