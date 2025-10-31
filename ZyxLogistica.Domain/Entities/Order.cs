using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Domain.Entities;

public class Order : EntityBase
{
    public string OrderNumber { get; private set; } = string.Empty;
    public string CustomerName { get; private set; } = string.Empty;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public List<OrderInventory> Items { get; private set; } = new();

    private Order() { }

    private Order(string orderNumber, string customerName)
    {
        OrderNumber = orderNumber;
        CustomerName = customerName;
    }

    public static Order Create(string orderNumber, string customerName) => new(orderNumber, customerName);

    public void AddItem(OrderInventory item)
    {
        Items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid orderInventoryId)
    {
        var item = Items.FirstOrDefault(i => i.Id == orderInventoryId);
        if (item != null)
        {
            Items.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void UpdateCustomer(string customerName)
    {
        CustomerName = customerName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetStatus(OrderStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
