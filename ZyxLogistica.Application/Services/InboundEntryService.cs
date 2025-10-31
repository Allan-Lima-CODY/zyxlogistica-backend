using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.Services;

public class InboundEntryService(IInboundEntryRepository _inboundEntryRepository, IInventoryRepository _inventoryRepository) : IInboundEntryService
{
    public async Task<InboundEntry> CreateAsync(InboundEntry inboundEntry, Inventory inventoryInput)
    {
        var existing = _inventoryRepository.GetInventoryByProductCode(inventoryInput.ProductCode);

        if (existing != null)
        {
            existing.IncreaseQuantity(inventoryInput.Quantity);
            _inventoryRepository.Update(existing);
            await _inventoryRepository.SaveChangesAsync();

            var entry = InboundEntry.Create(existing, inboundEntry.Reference, inboundEntry.SupplierName, inboundEntry.Observation);
            await _inboundEntryRepository.Add(entry);
            await _inboundEntryRepository.SaveChangesAsync();

            return entry;
        }
        else
        {
            var newInventory = Inventory.Create(inventoryInput.ProductCode, inventoryInput.Description, inventoryInput.Quantity, inventoryInput.Price);
            await _inventoryRepository.Add(newInventory);
            await _inventoryRepository.SaveChangesAsync();

            var entry = InboundEntry.Create(newInventory, inboundEntry.Reference, inboundEntry.SupplierName, inboundEntry.Observation);
            await _inboundEntryRepository.Add(entry);
            await _inboundEntryRepository.SaveChangesAsync();

            return entry;
        }
    }

    public async Task<InboundEntry?> GetByIdAsync(Guid id)
    {
        return await _inboundEntryRepository.GetById(id);
    }

    public async Task<InboundEntry> UpdateAsync(Guid id, InboundEntry inboundEntry)
    {
        var entry = await _inboundEntryRepository.GetById(id)
                     ?? throw new NotFoundException("InboundEntry não encontrado.");

        entry.Update(inboundEntry.Observation, inboundEntry.SupplierName, inboundEntry.Reference);

        _inboundEntryRepository.Update(entry);
        await _inboundEntryRepository.SaveChangesAsync();

        return entry;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entry = await _inboundEntryRepository.GetById(id) ?? throw new NotFoundException("InboundEntry não encontrado.");

        _inboundEntryRepository.Delete(entry);
        await _inboundEntryRepository.SaveChangesAsync();
    }

    public async Task<List<InboundEntry>?> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var maxRange = startDate.AddMonths(3);
        if (endDate > maxRange) endDate = maxRange;
        return await _inboundEntryRepository.GetEntriesByDateRangeAsync(startDate, endDate);
    }
}
