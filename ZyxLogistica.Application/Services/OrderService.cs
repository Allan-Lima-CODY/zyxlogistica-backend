using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.Services;

public class OrderService(IOrderRepository _orderRepository, IInventoryRepository _inventoryRepository) : IOrderService
{
    public async Task<Order> CreateAsync(Order order)
    {
        foreach (var item in order.Items)
        {
            var inventory = await _inventoryRepository.GetById(item.Inventory.Id);
            if (inventory == null)
                throw new NotFoundException($"Item de estoque {item.Inventory.Id} não encontrado.");

            if (item.Quantity > inventory.Quantity)
                throw new InvalidOperationException($"Quantidade solicitada para {inventory.Description} excede o estoque disponível.");

            inventory.DecreaseQuantity(item.Quantity);
            _inventoryRepository.Update(inventory);
        }

        await _orderRepository.Add(order);
        await _orderRepository.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
        => await _orderRepository.GetById(id);

    public async Task<List<Order>?> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var maxRange = startDate.AddMonths(3);
        if (endDate > maxRange) endDate = maxRange;

        return await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);
    }
}