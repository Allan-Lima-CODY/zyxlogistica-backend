using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Application.DTOs;

public record OrderDTO
{
    public record ListDTO(Guid Id, string OrderNumber, string CustomerName, OrderStatus Status, DateTime CreatedAt);

    public record CommandDTO(
        Guid Id,
        string OrderNumber,
        string CustomerName,
        OrderStatus Status,
        List<OrderInventoryDTO> Items,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    public class OrderInventoryDTO
    {
        public Guid Id { get; set; }
        public Guid InventoryId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
