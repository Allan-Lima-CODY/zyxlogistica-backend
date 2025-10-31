using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Application.CQRS.Queries;
using ZyxLogistica.Application.Models;

namespace ZyxLogistica.API.Controllers;

[ApiController]
[Route("api/order")]
public class OrderController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        => Ok(await _mediator.Send(new OrderQuery.GetOrdersByDateRangeQuery(startDate, endDate)));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrderModel.OrderInput input)
        => Ok(await _mediator.Send(new OrderCommand.CreateOrderCommand(input)));

    [HttpGet("available-for-expedition")]
    public async Task<IActionResult> GetAvailableOrdersForExpedition()
    => Ok(await _mediator.Send(new OrderQuery.GetPendingOrdersQuery()));
}
