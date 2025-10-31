namespace ZyxLogistica.Application.Models;

public record InventoryModel
{
    public record InventoryInput(string ProductCode, string Description, int Quantity, decimal Price);
    public record InventoryUpdate(string Description, decimal Price);
}
