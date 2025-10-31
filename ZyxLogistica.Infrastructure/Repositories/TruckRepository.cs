using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Infrastructure.Repositories;

public class TruckRepository : Base<Truck>, ITruckRepository
{
    private readonly string _connectionString;

    public TruckRepository(DatabaseContext context, IConfiguration configuration)
        : base(context)
    {
        _connectionString = configuration.GetConnectionString("ZyxLogistica")
            ?? throw new InvalidOperationException("Connection string não configurada.");
    }

    public Truck? GetTruckByLicensePlate(string licensePlate)
    {
        return (
            from d in _dbSet
            where d.LicensePlate == licensePlate
            select d).AsNoTracking().FirstOrDefault();
    }

    public async Task<List<Truck>?> GetTrucksByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        const string sql = @"
            SELECT *
            FROM Trucks
            WHERE CreatedAt BETWEEN @StartDate AND @EndDate
            ORDER BY CreatedAt DESC;
        ";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<Truck>(sql, new { StartDate = startDate, EndDate = endDate });
        return result.AsList();
    }

    public async Task<List<Truck>?> GetAvailableTrucksAsync()
    {
        const string sql = @"
        SELECT T.*
        FROM Trucks T
        WHERE T.Available = 1
          AND T.Id NOT IN (
              SELECT E.TruckId
              FROM Expeditions E
              INNER JOIN Orders O ON O.Id = E.OrderId
              WHERE O.Status NOT IN (@Delivered, @Canceled)
          )
        ORDER BY T.CreatedAt DESC;
    ";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<Truck>(sql, new
        {
            Delivered = (int)OrderStatus.Delivered,
            Canceled = (int)OrderStatus.Canceled
        });

        return result.AsList();
    }

}
