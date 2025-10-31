using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Application.DTOs;

public record DriverDTO
{
    public record ListDTO(Guid Id, string Name, string Phone, string Cnh, bool Active);
    public record CommandDTO(Guid Id, string Name, string Phone, string Cnh, CnhCategory CnhCategory, bool Active, DateTime CreatedAt, DateTime? UpdatedAt);
}
