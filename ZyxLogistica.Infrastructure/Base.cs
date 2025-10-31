using Microsoft.EntityFrameworkCore;
using ZyxLogistica.Domain.Interfaces;

namespace ZyxLogistica.Infrastructure;

public class Base<T> : IBase<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Base(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<List<T>?> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<T?> GetById(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public async Task SaveChangesAsync() { await _context.SaveChangesAsync(); }
}
