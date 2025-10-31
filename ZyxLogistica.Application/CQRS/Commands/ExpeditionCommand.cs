using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Application.CQRS.Commands;

public record ExpeditionCommand
{
    public record CreateExpeditionCommand(ExpeditionModel.ExpeditionInput Input) : IRequest<ExpeditionDTO.CommandDTO>;
    public record UpdateExpeditionCommand(Guid Id, ExpeditionModel.ExpeditionUpdate Update, OrderStatus? OrderStatus) : IRequest<ExpeditionDTO.CommandDTO>;
}
