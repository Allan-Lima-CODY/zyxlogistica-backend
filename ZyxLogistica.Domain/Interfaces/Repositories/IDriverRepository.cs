using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Repositories;

public interface IDriverRepository : IBase<Driver>
{
    Driver? GetDriverByPhone(string phone);
    Driver? GetDriverByCnh(string cnh);
    Task<List<Driver>?> GetDriversByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Driver>?> GetAvailableDriversAsync();
}
