using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Services;

public interface ITruckService
{
    Task<Truck> CreateAsync(Truck truck);
    Task<Truck?> GetByIdAsync(Guid id);
    Task<Truck> UpdateAsync(Truck truck);
    Truck? GetTruckByLicensePlate(string licensePlate);
    Task<List<Truck>?> GetTrucksByDateRangeAsync(DateTime startDate, DateTime endDate);
}
