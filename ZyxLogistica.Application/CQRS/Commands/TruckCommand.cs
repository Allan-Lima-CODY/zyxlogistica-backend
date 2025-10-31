using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;

namespace ZyxLogistica.Application.CQRS.Commands;

public record TruckCommand
{
    public record CreateTruckCommand(TruckModel.TruckInput TruckInput) : IRequest<TruckDTO.CommandDTO>;
    public record UpdateTruckCommand(Guid Id, TruckModel.TruckUpdate TruckUpdate) : IRequest<TruckDTO.CommandDTO>;
}
