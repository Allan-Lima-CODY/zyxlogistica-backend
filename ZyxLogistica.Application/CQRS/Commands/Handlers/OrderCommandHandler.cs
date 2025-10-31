using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.CQRS.Commands.Handlers;

public class OrderCommandHandler(IOrderService _orderService, IInventoryRepository _inventoryRepository, IMapper _mapper) :
      IRequestHandler<OrderCommand.CreateOrderCommand, OrderDTO.CommandDTO>
{
    public async Task<OrderDTO.CommandDTO> Handle(OrderCommand.CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.OrderInput.OrderNumber, request.OrderInput.CustomerName);

        foreach (var itemInput in request.OrderInput.Items)
        {
            var inventory = await _inventoryRepository.GetById(itemInput.InventoryId);
            if (inventory == null)
                throw new NotFoundException($"Item de estoque {itemInput.InventoryId} não encontrado.");

            var orderItem = OrderInventory.Create(order, inventory, itemInput.Quantity);
            order.AddItem(orderItem);
        }

        var saved = await _orderService.CreateAsync(order);

        return _mapper.Map<OrderDTO.CommandDTO>(saved);
    }
}
