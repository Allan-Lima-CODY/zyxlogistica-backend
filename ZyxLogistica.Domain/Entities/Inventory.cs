namespace ZyxLogistica.Domain.Entities;

public class Inventory : EntityBase
{
    public string ProductCode { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int Quantity { get; private set; } = 0;
    public decimal Price { get; private set; } = 0;
    public bool Active { get; private set; } = true;

    private Inventory() { }

    private Inventory(string productCode, string description, int quantity, decimal price)
    {
        ProductCode = productCode;
        Description = description;
        Quantity = quantity;
        Price = price;
        Active = true;
    }

    public static Inventory Create(string productCode, string description, int quantity, decimal price)
        => new(productCode, description, quantity, price);

    public void Update(string description, decimal price)
    {
        Description = description;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncreaseQuantity(int amount)
    {
        Quantity += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DecreaseQuantity(int amount)
    {
        Quantity -= amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Active = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Active = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
