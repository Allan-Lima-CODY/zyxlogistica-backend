using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;

namespace ZyxLogistica.Application.CQRS.Commands;

public record DriverCommand
{
    public record CreateDriverCommand(DriverModel.DriverInput DriverInput) : IRequest<DriverDTO.CommandDTO>;
    public record UpdateDriverCommand(Guid Id, DriverModel.DriverUpdate DriverUpdate) : IRequest<DriverDTO.CommandDTO>;
    public record ToggleDriverStatusCommand(Guid Id) : IRequest<DriverDTO.CommandDTO>;

}

