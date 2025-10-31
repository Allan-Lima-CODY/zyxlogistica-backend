using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Services;

public interface IInventoryService
{
    Task<Inventory> CreateAsync(Inventory inventory);
    Task<Inventory?> GetByIdAsync(Guid id);
    Task<Inventory> UpdateAsync(Inventory inventory);
    Task<Inventory> IncreaseQuantityAsync(Guid id, int amount);
    Task<Inventory> DecreaseQuantityAsync(Guid id, int amount);
    Inventory? GetInventoryByProductCode(string productCode);
    Task<List<Inventory>?> GetInventoriesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Inventory> ToggleActiveAsync(Guid id);
    Task<List<Inventory>?> GetAvailableInventoriesAsync();
}
