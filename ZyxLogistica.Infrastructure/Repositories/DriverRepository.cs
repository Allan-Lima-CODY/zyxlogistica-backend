using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Infrastructure.Repositories;

public class DriverRepository : Base<Driver>, IDriverRepository
{
    private readonly string _connectionString;

    public DriverRepository(DatabaseContext context, IConfiguration configuration)
        : base(context)
    {
        _connectionString = configuration.GetConnectionString("ZyxLogistica")
            ?? throw new InvalidOperationException("Connection string não configurada.");
    }
    public Driver? GetDriverByCnh(string cnh)
    {
        return (
            from d in _dbSet
            where d.Cnh == cnh
            select d).AsNoTracking().FirstOrDefault();
    }

    public Driver? GetDriverByPhone(string phone)
    {
        return (
            from d in _dbSet
            where d.Phone == phone
            select d).AsNoTracking().FirstOrDefault();
    }

    public async Task<List<Driver>?> GetDriversByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        const string sql = @"
            SELECT *
            FROM Drivers
            WHERE CreatedAt BETWEEN @StartDate AND @EndDate
            ORDER BY CreatedAt DESC;
        ";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<Driver>(sql, new { StartDate = startDate, EndDate = endDate });
        return [.. result];
    }

    public async Task<List<Driver>?> GetAvailableDriversAsync()
    {
        const string sql = @"
        SELECT D.*
        FROM Drivers D
        WHERE D.Active = 1
          AND D.Id NOT IN (
              SELECT E.DriverId
              FROM Expeditions E
              INNER JOIN Orders O ON O.Id = E.OrderId
              WHERE O.Status NOT IN (@Delivered, @Canceled)
          )
        ORDER BY D.CreatedAt DESC;
    ";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<Driver>(sql, new
        {
            Delivered = (int)OrderStatus.Delivered,
            Canceled = (int)OrderStatus.Canceled
        });

        return result.AsList();
    }

}
