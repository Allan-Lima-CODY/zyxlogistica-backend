namespace ZyxLogistica.Application.Models;

public record TruckModel
{
    public record TruckInput(string LicensePlate, string Model, int Year, decimal CapacityKg);
    public record TruckUpdate(string Model, int Year, decimal CapacityKg);
}
