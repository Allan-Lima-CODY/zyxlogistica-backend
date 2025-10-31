using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Repositories;

public interface IOrderRepository : IBase<Order>
{
    Order? GetByOrderNumber(string orderNumber);
    Task<List<Order>?> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Order>?> GetPendingOrdersAsync();
}
