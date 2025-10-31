using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Application.CQRS.Queries;
using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.API.Controllers;

[ApiController]
[Route("api/expedition")]
public class ExpeditionController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        => Ok(await _mediator.Send(new ExpeditionQuery.GetExpeditionsByDateRangeQuery(startDate, endDate)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _mediator.Send(new ExpeditionQuery.GetExpeditionByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ExpeditionModel.ExpeditionInput input)
        => Ok(await _mediator.Send(new ExpeditionCommand.CreateExpeditionCommand(input)));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] ExpeditionModel.ExpeditionUpdate update, OrderStatus? orderStatus)
        => Ok(await _mediator.Send(new ExpeditionCommand.UpdateExpeditionCommand(id, update, orderStatus)));
}
