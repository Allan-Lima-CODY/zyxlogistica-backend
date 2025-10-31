namespace ZyxLogistica.Application.DTOs;

public record InventoryDTO
{
    public record CommandDTO(Guid Id, string ProductCode, string Description, int Quantity, decimal Price, bool Active, DateTime CreatedAt, DateTime? UpdatedAt);
}
