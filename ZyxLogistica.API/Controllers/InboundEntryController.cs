using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Application.CQRS.Queries;
using ZyxLogistica.Application.Models;

namespace ZyxLogistica.API.Controllers;

[ApiController]
[Route("api/inbound-entry")]
public class InboundEntryController(IMediator _mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _mediator.Send(new InboundEntryQuery.GetInboundEntryByIdQuery(id)));

    [HttpGet]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        => Ok(await _mediator.Send(new InboundEntryQuery.GetInboundEntriesByDateRangeQuery(startDate, endDate)));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InboundEntryModel.InboundEntryInput input)
        => Ok(await _mediator.Send(new InboundEntryCommand.CreateInboundEntryCommand(input)));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] InboundEntryModel.InboundEntryUpdate update)
        => Ok(await _mediator.Send(new InboundEntryCommand.UpdateInboundEntryCommand(id, update)));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new InboundEntryCommand.DeleteInboundEntryCommand(id));
        return Ok();
    }
}
