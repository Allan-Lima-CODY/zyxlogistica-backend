using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Interfaces.Services;

namespace ZyxLogistica.Application.CQRS.Commands.Handlers;

public class InboundEntryCommandHandler(IInboundEntryService _inboundEntryService, IMapper _mapper) :
    IRequestHandler<InboundEntryCommand.CreateInboundEntryCommand, InboundEntryDTO.CommandDTO>,
    IRequestHandler<InboundEntryCommand.UpdateInboundEntryCommand, InboundEntryDTO.CommandDTO>,
    IRequestHandler<InboundEntryCommand.DeleteInboundEntryCommand>
{
    public async Task<InboundEntryDTO.CommandDTO> Handle(InboundEntryCommand.CreateInboundEntryCommand request, CancellationToken cancellationToken)
    {
        var tempEntry = InboundEntry.Create(
            inventory: null!,
            reference: request.Input.Reference,
            supplierName: request.Input.SupplierName,
            observation: request.Input.Observation
        );

        var tempInventory = _mapper.Map<Inventory>(request.Input.InventoryInput);

        var saved = await _inboundEntryService.CreateAsync(tempEntry, tempInventory);
        return _mapper.Map<InboundEntryDTO.CommandDTO>(saved);
    }

    public async Task<InboundEntryDTO.CommandDTO> Handle(InboundEntryCommand.UpdateInboundEntryCommand request, CancellationToken cancellationToken)
    {
        var inboundEntry = _mapper.Map<InboundEntry>(request.Update);

        var updated = await _inboundEntryService.UpdateAsync(request.Id, inboundEntry);
        return _mapper.Map<InboundEntryDTO.CommandDTO>(updated);
    }

    public async Task Handle(InboundEntryCommand.DeleteInboundEntryCommand request, CancellationToken cancellationToken)
    {
        await _inboundEntryService.DeleteAsync(request.Id);
    }
}
