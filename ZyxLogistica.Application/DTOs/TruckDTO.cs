namespace ZyxLogistica.Application.DTOs;

public record TruckDTO
{
    public record ListDTO(Guid Id, string LicensePlate, string Model, int Year, decimal CapacityKg, bool Available, DateTime CreatedAt);
    public record CommandDTO(Guid Id, string LicensePlate, string Model, int Year, decimal CapacityKg, bool Available, DateTime CreatedAt, DateTime? UpdatedAt);
}
