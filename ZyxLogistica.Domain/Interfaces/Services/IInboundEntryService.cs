using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Services;

public interface IInboundEntryService
{
    Task<InboundEntry> CreateAsync(InboundEntry inboundEntry, Inventory inventoryInput);
    Task<InboundEntry?> GetByIdAsync(Guid id);
    Task<InboundEntry> UpdateAsync(Guid id, InboundEntry updateInboundEntry);
    Task DeleteAsync(Guid id);
    Task<List<InboundEntry>?> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
