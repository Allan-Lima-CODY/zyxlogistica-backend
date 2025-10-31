using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;

namespace ZyxLogistica.Application.CQRS.Commands;

public class InventoryCommand
{
    public record CreateInventoryCommand(InventoryModel.InventoryInput InventoryInput) : IRequest<InventoryDTO.CommandDTO>;
    public record UpdateInventoryCommand(Guid Id, InventoryModel.InventoryUpdate InventoryUpdate) : IRequest<InventoryDTO.CommandDTO>;
    public record ToggleInventoryActiveCommand(Guid Id) : IRequest<InventoryDTO.CommandDTO>;
    public record IncreaseInventoryCommand(Guid Id, int Amount) : IRequest<InventoryDTO.CommandDTO>;
    public record DecreaseInventoryCommand(Guid Id, int Amount) : IRequest<InventoryDTO.CommandDTO>;

}
