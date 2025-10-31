using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Application.CQRS.Queries;
using static ZyxLogistica.Application.Models.InventoryModel;

namespace ZyxLogistica.API.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        => Ok(await _mediator.Send(new InventoryQuery.GetInventoriesByDateRangeQuery(startDate, endDate)));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InventoryInput inventoryInput)
        => Ok(await _mediator.Send(new InventoryCommand.CreateInventoryCommand(inventoryInput)));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] InventoryUpdate inventoryUpdate)
        => Ok(await _mediator.Send(new InventoryCommand.UpdateInventoryCommand(id, inventoryUpdate)));

    [HttpPatch("increase/{id:guid}")]
    public async Task<IActionResult> Increase(Guid id, [FromBody] int amount)
        => Ok(await _mediator.Send(new InventoryCommand.IncreaseInventoryCommand(id, amount)));

    [HttpPatch("decrease/{id:guid}")]
    public async Task<IActionResult> Decrease(Guid id, [FromBody] int amount)
        => Ok(await _mediator.Send(new InventoryCommand.DecreaseInventoryCommand(id, amount)));

    [HttpPatch("toggle-active/{id:guid}")]
    public async Task<IActionResult> ToggleActive(Guid id)
        => Ok(await _mediator.Send(new InventoryCommand.ToggleInventoryActiveCommand(id)));

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    => Ok(await _mediator.Send(new InventoryQuery.GetAvailableInventoriesQuery()));
}
