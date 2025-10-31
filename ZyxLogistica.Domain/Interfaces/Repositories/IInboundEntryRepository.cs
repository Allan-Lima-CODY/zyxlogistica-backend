using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Repositories;

public interface IInboundEntryRepository : IBase<InboundEntry>
{
    Task<InboundEntry?> GetByReferenceAsync(string reference);
    Task<List<InboundEntry>?> GetEntriesByDateRangeAsync(DateTime startDate, DateTime endDate);
}
