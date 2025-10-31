using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Application.CQRS.Commands.InventoryCommand;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.CQRS.Commands.Handlers;

public class InventoryCommandHandler(IInventoryService _inventoryService, IMapper _mapper) :
    IRequestHandler<CreateInventoryCommand, InventoryDTO.CommandDTO>,
    IRequestHandler<UpdateInventoryCommand, InventoryDTO.CommandDTO>,
    IRequestHandler<IncreaseInventoryCommand, InventoryDTO.CommandDTO>,
    IRequestHandler<DecreaseInventoryCommand, InventoryDTO.CommandDTO>,
    IRequestHandler<ToggleInventoryActiveCommand, InventoryDTO.CommandDTO>
{
    public async Task<InventoryDTO.CommandDTO> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = _mapper.Map<Domain.Entities.Inventory>(request.InventoryInput);

        var saved = await _inventoryService.CreateAsync(inventory);
        return _mapper.Map<InventoryDTO.CommandDTO>(saved);
    }

    public async Task<InventoryDTO.CommandDTO> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _inventoryService.GetByIdAsync(request.Id) ?? throw new NotFoundException("Item de estoque não encontrado");
        inventory.Update(request.InventoryUpdate.Description, request.InventoryUpdate.Price);

        await _inventoryService.UpdateAsync(inventory);
        return _mapper.Map<InventoryDTO.CommandDTO>(inventory);
    }

    public async Task<InventoryDTO.CommandDTO> Handle(IncreaseInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _inventoryService.IncreaseQuantityAsync(request.Id, request.Amount);
        return _mapper.Map<InventoryDTO.CommandDTO>(inventory);
    }

    public async Task<InventoryDTO.CommandDTO> Handle(DecreaseInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _inventoryService.DecreaseQuantityAsync(request.Id, request.Amount);
        return _mapper.Map<InventoryDTO.CommandDTO>(inventory);
    }

    public async Task<InventoryDTO.CommandDTO> Handle(ToggleInventoryActiveCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _inventoryService.ToggleActiveAsync(request.Id);
        return _mapper.Map<InventoryDTO.CommandDTO>(inventory);
    }
}
