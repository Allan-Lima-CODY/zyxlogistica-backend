using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Domain.Interfaces.Services;

public interface IOrderService
{
    Task<Order> CreateAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<List<Order>?> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
}
