using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Infrastructure.Repositories;

public class InboundEntryRepository : Base<InboundEntry>, IInboundEntryRepository
{
    private readonly string _connectionString;

    public InboundEntryRepository(DatabaseContext context, IConfiguration configuration) : base(context)
    {
        _connectionString = configuration.GetConnectionString("ZyxLogistica")
            ?? throw new InvalidOperationException("Connection string não configurada.");
    }

    public InboundEntry? GetByReference(string reference)
    {
        return _dbSet
            .Include(ie => ie.Inventory)
            .AsNoTracking()
            .FirstOrDefault(ie => ie.Reference == reference);
    }

    // async version as interface demands
    public async Task<InboundEntry?> GetByReferenceAsync(string reference)
    {
        return await Task.FromResult(GetByReference(reference));
    }

    public async Task<List<InboundEntry>?> GetEntriesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        const string sql = @"
        SELECT ie.*, inv.*
        FROM InboundEntries ie
        INNER JOIN Inventories inv ON ie.InventoryId = inv.Id
        WHERE ie.CreatedAt BETWEEN @StartDate AND @EndDate
        ORDER BY ie.CreatedAt DESC;
    ";

        using var connection = new SqlConnection(_connectionString);

        var result = await connection.QueryAsync<InboundEntry, Inventory, InboundEntry>(
            sql,
            (entry, inventory) =>
            {
                entry.SetInventory(inventory);
                return entry;
            },
            new { StartDate = startDate, EndDate = endDate }
        );

        return result.ToList();
    }

    public async Task<InboundEntry?> GetByIdWithInventoryAsync(Guid id)
    {
        return await _dbSet
            .Include(ie => ie.Inventory)
            .AsNoTracking()
            .FirstOrDefaultAsync(ie => ie.Id == id);
    }

    public override async Task<InboundEntry?> GetById(Guid id)
    {
        return await _context.Set<InboundEntry>()
                             .Include(e => e.Inventory)
                             .FirstOrDefaultAsync(e => e.Id == id);
    }
}
