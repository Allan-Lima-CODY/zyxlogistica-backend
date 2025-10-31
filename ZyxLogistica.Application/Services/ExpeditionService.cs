using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.Services;

public class ExpeditionService(IExpeditionRepository _expeditionRepository,
                              IOrderRepository _orderRepository,
                              IDriverRepository _driverRepository,
                              ITruckRepository _truckRepository,
                              IInventoryRepository _inventoryRepository) : IExpeditionService
{
    public async Task<Expedition> CreateAsync(Expedition expedition)
    {
        var order = await _orderRepository.GetById(expedition.Order.Id)
            ?? throw new NotFoundException("Pedido não encontrado.");

        if (order.Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Não é possível criar expedição para pedido já entregue.");

        if (order.Status == OrderStatus.Canceled)
            throw new InvalidOperationException("Não é possível criar expedição para pedido cancelado.");

        if (order.Status != OrderStatus.Pending)
            throw new InvalidOperationException("Somente pedidos com status Pending podem ser movimentados para expedição.");

        var existsExp = await _expeditionRepository.GetByOrderId(order.Id);
        if (existsExp != null && existsExp.Order.Status is not (OrderStatus.Delivered or OrderStatus.Canceled))
            throw new ConflictException("Já existe uma expedição ativa para esse pedido.");

        var driver = await _driverRepository.GetById(expedition.Driver.Id)
            ?? throw new NotFoundException("Motorista não encontrado.");

        var truck = await _truckRepository.GetById(expedition.Truck.Id)
            ?? throw new NotFoundException("Caminhão não encontrado.");

        var conflictDriver = await _expeditionRepository.GetActiveByDriver(driver.Id);
        if (conflictDriver != null)
            throw new ConflictException("Motorista está ocupado em outra expedição ativa.");

        var conflictTruck = await _expeditionRepository.GetActiveByTruck(truck.Id);
        if (conflictTruck != null)
            throw new ConflictException("Caminhão está ocupado em outra expedição ativa.");

        var today = DateTime.UtcNow.Date;
        if (expedition.DeliveryForecast.Date < today)
            throw new InvalidOperationException("DeliveryForecast não pode ser no passado.");
        if (expedition.DeliveryForecast.Date > today.AddDays(30))
            throw new InvalidOperationException("DeliveryForecast muito distante (máx 30 dias).");

        order.SetStatus(OrderStatus.InSeparation);
        _orderRepository.Update(order);

        truck.Occupy();
        _truckRepository.Update(truck);

        await _expeditionRepository.Add(expedition);
        await _orderRepository.SaveChangesAsync();
        await _truckRepository.SaveChangesAsync();
        await _expeditionRepository.SaveChangesAsync();

        return expedition;
    }


    public async Task<Expedition?> GetByIdAsync(Guid id)
        => await _expeditionRepository.GetById(id);

    public async Task<Expedition> UpdateAsync(Guid id, Expedition updateExpedition, OrderStatus? newOrderStatus)
    {
        var expedition = await _expeditionRepository.GetById(id)
            ?? throw new NotFoundException("Expedição não encontrada.");

        var order = expedition.Order ?? throw new NotFoundException("Pedido da expedição não encontrado.");

        if (newOrderStatus.HasValue)
        {
            var ns = newOrderStatus.Value;

            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Canceled)
                throw new InvalidOperationException("Status do pedido é imutável após Delivered ou Canceled.");

            if (ns == OrderStatus.Pending)
            {
                var existsExp = await _expeditionRepository.GetByOrderId(order.Id);
                if (existsExp != null)
                    throw new InvalidOperationException("Não é permitido voltar pedido para Pending após ter sido associado a uma expedição.");
            }

            if (order.Status is not (OrderStatus.Pending or OrderStatus.InSeparation or OrderStatus.InTransit))
                throw new InvalidOperationException("Só é possível atualizar expedições enquanto o pedido estiver Pending, InSeparation ou InTransit.");

            order.SetStatus(ns);
            _orderRepository.Update(order);

            if (ns == OrderStatus.Canceled)
            {
                foreach (var item in order.Items)
                {
                    var inventory = await _inventoryRepository.GetById(item.Inventory.Id);
                    if (inventory != null)
                    {
                        inventory.IncreaseQuantity(item.Quantity);
                        _inventoryRepository.Update(inventory);
                    }
                }
            }

            if (ns == OrderStatus.Delivered || ns == OrderStatus.Canceled)
            {
                var truckToFree = await _truckRepository.GetById(expedition.Truck.Id);
                truckToFree?.ToMakeAvailable();
                if (truckToFree != null)
                    _truckRepository.Update(truckToFree);
            }
        }

        var driver = await _driverRepository.GetById(updateExpedition.Driver.Id)
            ?? throw new NotFoundException("Motorista não encontrado.");

        var truck = await _truckRepository.GetById(updateExpedition.Truck.Id)
            ?? throw new NotFoundException("Caminhão não encontrado.");

        var conflictDriver = await _expeditionRepository.GetActiveByDriver(driver.Id);
        if (conflictDriver != null && conflictDriver.Id != expedition.Id)
            throw new ConflictException("Motorista está ocupado em outra expedição ativa.");

        var conflictTruck = await _expeditionRepository.GetActiveByTruck(truck.Id);
        if (conflictTruck != null && conflictTruck.Id != expedition.Id)
            throw new ConflictException("Caminhão está ocupado em outra expedição ativa.");

        var today = DateTime.UtcNow.Date;
        if (updateExpedition.DeliveryForecast.Date < today)
            throw new InvalidOperationException("DeliveryForecast não pode ser no passado.");
        if (updateExpedition.DeliveryForecast.Date > today.AddDays(30))
            throw new InvalidOperationException("DeliveryForecast muito distante (máx 30 dias).");

        if (expedition.Truck.Id != truck.Id)
        {
            expedition.ChangeTruck(truck);
            _truckRepository.Update(expedition.Truck);
        }

        expedition.Update(updateExpedition.DeliveryForecast, updateExpedition.Observation, truck, driver);

        _driverRepository.Update(driver);
        _expeditionRepository.Update(expedition);

        await _orderRepository.SaveChangesAsync();
        await _inventoryRepository.SaveChangesAsync();
        await _truckRepository.SaveChangesAsync();
        await _driverRepository.SaveChangesAsync();
        await _expeditionRepository.SaveChangesAsync();

        return expedition;
    }

    public async Task<List<Expedition>?> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var maxRange = startDate.AddMonths(3);
        if (endDate > maxRange) endDate = maxRange;
        return await _expeditionRepository.GetExpeditionsByDateRangeAsync(startDate, endDate);
    }
}