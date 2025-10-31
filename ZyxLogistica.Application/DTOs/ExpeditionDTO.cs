namespace ZyxLogistica.Application.DTOs;

public record ExpeditionDTO
{
    public record CommandDTO(Guid Id, Guid OrderId, string OrderNumber, string CustomerName, string OrderStatus, Guid DriverId, string DriverName, Guid TruckId, string TruckModel, string TruckPlate, DateTime DeliveryForecast, string Observation, DateTime CreatedAt, DateTime? UpdatedAt);
}
