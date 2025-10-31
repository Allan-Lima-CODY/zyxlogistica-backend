namespace ZyxLogistica.Domain.Interfaces;

public interface IBase<T> where T : class
{
    Task Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T?> GetById(Guid id);
    Task<List<T>?> GetAll();
    Task SaveChangesAsync();
}

