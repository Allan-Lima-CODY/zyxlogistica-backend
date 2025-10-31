using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Infrastructure.Repositories;

public class InventoryRepository : Base<Inventory>, IInventoryRepository
{
    private readonly string _connectionString;

    public InventoryRepository(DatabaseContext context, IConfiguration configuration)
        : base(context)
    {
        _connectionString = configuration.GetConnectionString("ZyxLogistica")
            ?? throw new InvalidOperationException("Connection string não configurada.");
    }

    public Inventory? GetInventoryByProductCode(string productCode)
    {
        return (
            from i in _dbSet
            where i.ProductCode == productCode
            select i).AsNoTracking().FirstOrDefault();
    }

    public async Task<List<Inventory>?> GetInventoriesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        const string sql = @"
            SELECT *
            FROM Inventories
            WHERE CreatedAt BETWEEN @StartDate AND @EndDate
            ORDER BY CreatedAt DESC;
        ";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<Inventory>(sql, new { StartDate = startDate, EndDate = endDate });
        return result.AsList();
    }

    public async Task<List<Inventory>?> GetAvailableInventoriesAsync()
    {
        const string sql = @"
        SELECT *
        FROM Inventories
        WHERE Active = 1 AND Quantity > 0
        ORDER BY Description;
    ";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<Inventory>(sql);
        return result.AsList();
    }

}
