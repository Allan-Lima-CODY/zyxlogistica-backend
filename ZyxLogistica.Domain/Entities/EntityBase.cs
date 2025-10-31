namespace ZyxLogistica.Domain.Entities;

public class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; private init; }
    public DateTime? UpdatedAt { get; protected set; }
    internal EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}
