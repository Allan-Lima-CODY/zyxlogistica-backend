using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Services;

public interface IDriverService
{
    Task<Driver> CreateAsync(Driver driver);
    Task<Driver?> GetByIdAsync(Guid id);
    Task<Driver> UpdateAsync(Driver driver);
    Driver? GetDriverByCnh(string cnh);
    Driver? GetDriverByPhone(string phone);
    Task<Driver> ToggleStatusAsync(Guid id);
    Task<List<Driver>?> GetDriversByDateRangeAsync(DateTime startDate, DateTime endDate);
}
