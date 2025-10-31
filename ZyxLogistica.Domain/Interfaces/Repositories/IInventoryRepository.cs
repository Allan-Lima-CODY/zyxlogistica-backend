using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Repositories;

public interface IInventoryRepository : IBase<Inventory>
{
    Inventory? GetInventoryByProductCode(string productCode);
    Task<List<Inventory>?> GetInventoriesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Inventory>?> GetAvailableInventoriesAsync();
}
