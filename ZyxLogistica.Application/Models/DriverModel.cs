using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Application.Models;

public record DriverModel
{
    public record DriverInput(string Name, string Phone, string Cnh, CnhCategory CnhCategory);
    public record DriverUpdate(string Name, string Phone);
}
