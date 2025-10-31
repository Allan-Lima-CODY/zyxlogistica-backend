using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Application.Models;

public record ExpeditionModel
{
    public record ExpeditionInput(Guid OrderId, Guid DriverId, Guid TruckId, DateTime DeliveryForecast, string Observation);
    public record ExpeditionUpdate(DateTime DeliveryForecast, string Observation, Guid DriverId, Guid TruckId, OrderStatus? OrderStatus);
}
