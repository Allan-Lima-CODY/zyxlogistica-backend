using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.Services;

public class InventoryService(IInventoryRepository _inventoryRepository, IExpeditionRepository _expeditionRepository) : IInventoryService
{
    public async Task<Inventory> CreateAsync(Inventory inventory)
    {
        var existing = GetInventoryByProductCode(inventory.ProductCode);
        if (existing != null)
            throw new ConflictException("Já existe um item com este código de produto.");

        await _inventoryRepository.Add(inventory);
        await _inventoryRepository.SaveChangesAsync();
        return inventory;
    }

    public async Task<Inventory?> GetByIdAsync(Guid id)
    {
        return await _inventoryRepository.GetById(id);
    }

    public async Task<Inventory> UpdateAsync(Inventory inventory)
    {
        _inventoryRepository.Update(inventory);
        await _inventoryRepository.SaveChangesAsync();
        return inventory;
    }

    public Inventory? GetInventoryByProductCode(string productCode)
    {
        return _inventoryRepository.GetInventoryByProductCode(productCode);
    }

    public async Task<List<Inventory>?> GetInventoriesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var maxRange = startDate.AddMonths(3);
        if (endDate > maxRange) endDate = maxRange;

        return await _inventoryRepository.GetInventoriesByDateRangeAsync(startDate, endDate);
    }

    public async Task<Inventory> IncreaseQuantityAsync(Guid id, int amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount deve ser maior que zero.", nameof(amount));

        var inventory = await _inventoryRepository.GetById(id);
        if (inventory == null) throw new NotFoundException("Item de estoque não encontrado");

        inventory.IncreaseQuantity(amount);
        _inventoryRepository.Update(inventory);
        await _inventoryRepository.SaveChangesAsync();
        return inventory;
    }

    public async Task<Inventory> DecreaseQuantityAsync(Guid id, int amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount deve ser maior que zero.", nameof(amount));

        var inventory = await _inventoryRepository.GetById(id);
        if (inventory == null) throw new NotFoundException("Item de estoque não encontrado");

        if (inventory.Quantity - amount < 0)
            throw new ConflictException("Quantidade insuficiente no estoque.");

        inventory.DecreaseQuantity(amount);
        _inventoryRepository.Update(inventory);
        await _inventoryRepository.SaveChangesAsync();
        return inventory;
    }

    public async Task<Inventory> ToggleActiveAsync(Guid id)
    {
        var inventory = await _inventoryRepository.GetById(id)
            ?? throw new NotFoundException("Item de estoque não encontrado");

        var activeExpedition = await _expeditionRepository.GetExpeditionByInventoryId(inventory.Id);

        if (activeExpedition != null)
        {
            var orderStatus = activeExpedition.Order?.Status;
            if (orderStatus is OrderStatus.Pending
                or OrderStatus.InSeparation
                or OrderStatus.InTransit)
            {
                throw new InvalidOperationException(
                    "Não é possível inativar um item de estoque vinculado a uma expedição com pedido pendente, em separação ou em trânsito."
                );
            }
        }

        if (inventory.Active)
            inventory.Deactivate();
        else
            inventory.Activate();

        _inventoryRepository.Update(inventory);
        await _inventoryRepository.SaveChangesAsync();

        return inventory;
    }

    public async Task<List<Inventory>?> GetAvailableInventoriesAsync()
    {
        return await _inventoryRepository.GetAvailableInventoriesAsync();
    }
}