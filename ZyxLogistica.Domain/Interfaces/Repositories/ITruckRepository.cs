using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Domain.Interfaces.Repositories;

public interface ITruckRepository : IBase<Truck>
{
    Truck? GetTruckByLicensePlate(string licensePlate);
    Task<List<Truck>?> GetTrucksByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Truck>?> GetAvailableTrucksAsync();
}
