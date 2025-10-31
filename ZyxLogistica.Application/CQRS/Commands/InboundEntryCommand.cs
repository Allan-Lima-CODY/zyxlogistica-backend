using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;

namespace ZyxLogistica.Application.CQRS.Commands;

public record InboundEntryCommand
{
    public record CreateInboundEntryCommand(InboundEntryModel.InboundEntryInput Input) : IRequest<InboundEntryDTO.CommandDTO>;
    public record UpdateInboundEntryCommand(Guid Id, InboundEntryModel.InboundEntryUpdate Update) : IRequest<InboundEntryDTO.CommandDTO>;
    public record DeleteInboundEntryCommand(Guid Id) : IRequest;
}
