using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Infrastructure.Repositories;

public class OrderRepository : Base<Order>, IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(DatabaseContext context, IConfiguration configuration) : base(context)
    {
        _connectionString = configuration.GetConnectionString("ZyxLogistica")
            ?? throw new InvalidOperationException("Connection string não configurada.");
    }

    public Order? GetByOrderNumber(string orderNumber)
    {
        return _dbSet.AsNoTracking().FirstOrDefault(o => o.OrderNumber == orderNumber);
    }

    public async Task<List<Order>?> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(i => i.Inventory)
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>?> GetPendingOrdersAsync()
    {
        const string sql = @"
        SELECT *
        FROM Orders
        WHERE Status = @Pending
        ORDER BY CreatedAt DESC;
    ";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<Order>(sql, new { Pending = (int)OrderStatus.Pending });
        return result.AsList();
    }
}
