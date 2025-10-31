using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Repositories;

public interface IExpeditionRepository : IBase<Expedition>
{
    Task<Expedition?> GetByOrderId(Guid orderId);
    Task<Expedition?> GetActiveByDriver(Guid driverId);
    Task<Expedition?> GetActiveByTruck(Guid truckId);
    Task<Expedition?> GetBlockingByDriver(Guid driverId);
    Task<List<Expedition>?> GetExpeditionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Expedition?> GetExpeditionByInventoryId(Guid inventoryId);
}
