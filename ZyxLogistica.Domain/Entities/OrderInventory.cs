namespace ZyxLogistica.Domain.Entities;

public class OrderInventory : EntityBase
{
    public Order Order { get; private set; } = null!;
    public Inventory Inventory { get; private set; } = null!;
    public int Quantity { get; private set; }

    private OrderInventory() { }

    private OrderInventory(Order order, Inventory inventory, int quantity)
    {
        Order = order;
        Inventory = inventory;
        Quantity = quantity;
    }

    public static OrderInventory Create(Order order, Inventory inventory, int quantity) => new(order, inventory, quantity);

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}