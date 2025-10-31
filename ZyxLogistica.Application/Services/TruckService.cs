using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.Services;

public class TruckService(ITruckRepository _truckRepository) : ITruckService
{
    public async Task<Truck> CreateAsync(Truck truck)
    {
        var existingTruck = GetTruckByLicensePlate(truck.LicensePlate);
        if (existingTruck != null)
            throw new ConflictException("Já existe um caminhão com esta placa.");

        await _truckRepository.Add(truck);
        await _truckRepository.SaveChangesAsync();

        return truck;
    }

    public async Task<Truck?> GetByIdAsync(Guid id)
    {
        return await _truckRepository.GetById(id);
    }

    public Truck? GetTruckByLicensePlate(string licensePlate)
    {
        var cleanedPlate = Extensions.CleanNumber(licensePlate);
        return _truckRepository.GetTruckByLicensePlate(cleanedPlate);
    }

    public async Task<Truck> UpdateAsync(Truck truck)
    {
        _truckRepository.Update(truck);
        await _truckRepository.SaveChangesAsync();
        return truck;
    }

    public async Task<List<Truck>?> GetTrucksByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var maxRange = startDate.AddMonths(3);
        if (endDate > maxRange) endDate = maxRange;

        return await _truckRepository.GetTrucksByDateRangeAsync(startDate, endDate);
    }
}
