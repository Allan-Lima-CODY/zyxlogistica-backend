namespace ZyxLogistica.Application.DTOs;

public record InboundEntryDTO
{
    public record CommandDTO(Guid Id, Guid InventoryId, string Description, int Quantity, decimal Price, string ProductCode, string Reference, string SupplierName, string Observation, DateTime CreatedAt, DateTime? UpdatedAt);
}

