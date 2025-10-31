using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Infrastructure.Repositories;

public class ExpeditionRepository : Base<Expedition>, IExpeditionRepository
{
    private readonly string _connectionString;

    public ExpeditionRepository(DatabaseContext context, IConfiguration configuration) : base(context)
    {
        _connectionString = configuration.GetConnectionString("ZyxLogistica")
            ?? throw new InvalidOperationException("Connection string não configurada.");
    }

    public async Task<Expedition?> GetByOrderId(Guid orderId)
    {
        return await _dbSet
            .Include(e => e.Order)
            .Include(e => e.Driver)
            .Include(e => e.Truck)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Order.Id == orderId);
    }

    public async Task<Expedition?> GetBlockingByDriver(Guid driverId)
    {
        return await _dbSet
            .Include(e => e.Order)
            .Include(e => e.Driver)
            .Include(e => e.Truck)
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.Driver.Id == driverId &&
                (e.Order.Status == OrderStatus.InSeparation || e.Order.Status == OrderStatus.InTransit || e.Order.Status == OrderStatus.Pending));
    }


    public async Task<Expedition?> GetActiveByDriver(Guid driverId)
    {
        return await _dbSet
            .Include(e => e.Order)
            .Include(e => e.Driver)
            .Include(e => e.Truck)
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.Driver.Id == driverId &&
                e.Order.Status != Domain.Enums.OrderStatus.Delivered &&
                e.Order.Status != Domain.Enums.OrderStatus.Canceled);
    }

    public async Task<Expedition?> GetActiveByTruck(Guid truckId)
    {
        return await _dbSet
            .Include(e => e.Order)
            .Include(e => e.Driver)
            .Include(e => e.Truck)
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.Truck.Id == truckId &&
                e.Order.Status != Domain.Enums.OrderStatus.Delivered &&
                e.Order.Status != Domain.Enums.OrderStatus.Canceled);
    }


    public async Task<List<Expedition>?> GetExpeditionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(e => e.Order)
                .ThenInclude(o => o.Items)
                    .ThenInclude(i => i.Inventory)
            .Include(e => e.Driver)
            .Include(e => e.Truck)
            .AsNoTracking()
            .Where(e => e.DeliveryForecast >= startDate && e.DeliveryForecast <= endDate)
            .OrderByDescending(e => e.DeliveryForecast)
            .ToListAsync();
    }

    public async Task<Expedition?> GetExpeditionByInventoryId(Guid inventoryId)
    {
        return await _dbSet
            .Include(e => e.Order)
                .ThenInclude(o => o.Items)
                    .ThenInclude(oi => oi.Inventory)
            .Include(e => e.Driver)
            .Include(e => e.Truck)
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.Order.Items.Any(i => i.Inventory.Id == inventoryId) &&
                e.Order.Status != OrderStatus.Delivered &&
                e.Order.Status != OrderStatus.Canceled
            );
    }

    public override async Task<Expedition?> GetById(Guid id)
    {
        return await _dbSet
            .Include(e => e.Order)
            .ThenInclude(o => o.Items)
            .ThenInclude(i => i.Inventory)
            .Include(e => e.Driver)
            .Include(e => e.Truck)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
