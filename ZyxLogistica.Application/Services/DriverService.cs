using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.Services;

public class DriverService(IDriverRepository _driverRepository, IExpeditionRepository _expeditionRepository) : IDriverService
{
    public async Task<Driver> CreateAsync(Driver driver)
    {
        var existingByCnh = GetDriverByCnh(driver.Cnh);
        if (existingByCnh != null)
            throw new ConflictException("Já existe um motorista com esta CNH.");

        var existingByPhone = GetDriverByPhone(driver.Phone);
        if (existingByPhone != null)
            throw new ConflictException("Já existe um motorista com este telefone.");

        await _driverRepository.Add(driver);
        await _driverRepository.SaveChangesAsync();

        return driver;
    }

    public async Task<Driver?> GetByIdAsync(Guid id)
    {
        return await _driverRepository.GetById(id);
    }

    public async Task<Driver> UpdateAsync(Driver driver)
    {
        _driverRepository.Update(driver);
        await _driverRepository.SaveChangesAsync();
        return driver;
    }

    public Driver? GetDriverByCnh(string cnh)
    {
        var cleanedCnh = Extensions.CleanNumber(cnh);
        return _driverRepository.GetDriverByCnh(cleanedCnh);
    }

    public Driver? GetDriverByPhone(string phone)
    {
        var cleanedPhone = Extensions.CleanNumber(phone);
        return _driverRepository.GetDriverByPhone(cleanedPhone);
    }

    public async Task<Driver> ToggleStatusAsync(Guid id)
    {
        var driver = await _driverRepository.GetById(id)
            ?? throw new NotFoundException("Motorista não encontrado.");

        var activeExpedition = await _expeditionRepository.GetBlockingByDriver(id);

        if (activeExpedition != null)
        {
            var orderStatus = activeExpedition.Order?.Status;

            if (orderStatus is OrderStatus.InSeparation or OrderStatus.InTransit or OrderStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Não é possível inativar um motorista vinculado a uma expedição com pedido em separação ou em trânsito."
                );
            }
        }

        if (driver.Active)
            driver.Deactivate();
        else
            driver.Activate();

        _driverRepository.Update(driver);
        await _driverRepository.SaveChangesAsync();

        return driver;
    }


    public async Task<List<Driver>?> GetDriversByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var maxRange = startDate.AddMonths(3);
        if (endDate > maxRange) endDate = maxRange;

        var drivers = await _driverRepository.GetDriversByDateRangeAsync(startDate, endDate);
        return drivers;
    }
}
