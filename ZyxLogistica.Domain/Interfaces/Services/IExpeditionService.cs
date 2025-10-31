using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Domain.Interfaces.Services;

public interface IExpeditionService
{
    Task<Expedition> CreateAsync(Expedition expedition);
    Task<Expedition?> GetByIdAsync(Guid id);
    Task<Expedition> UpdateAsync(Guid id, Expedition updateExpedition, OrderStatus? newOrderStatus);
    Task<List<Expedition>?> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
