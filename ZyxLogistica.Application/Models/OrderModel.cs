using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Application.Models;

public record OrderModel
{
    public record OrderInput(string OrderNumber, string CustomerName, List<OrderInventoryInput> Items);
    public record OrderInventoryInput(Guid InventoryId, int Quantity);
}
