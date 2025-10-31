using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Interfaces.Services;

namespace ZyxLogistica.Application.CQRS.Queries.Handlers;

public class InboundEntryQueryHandler(IInboundEntryService _inboundEntryService, IMapper _mapper) :
    IRequestHandler<InboundEntryQuery.GetInboundEntryByIdQuery, InboundEntryDTO.CommandDTO?>,
    IRequestHandler<InboundEntryQuery.GetInboundEntriesByDateRangeQuery, List<InboundEntryDTO.CommandDTO>?>
{
    public async Task<InboundEntryDTO.CommandDTO?> Handle(InboundEntryQuery.GetInboundEntryByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _inboundEntryService.GetByIdAsync(request.Id);
        return _mapper.Map<InboundEntryDTO.CommandDTO?>(item);
    }

    public async Task<List<InboundEntryDTO.CommandDTO>?> Handle(InboundEntryQuery.GetInboundEntriesByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var list = await _inboundEntryService.GetByDateRangeAsync(request.StartDate, request.EndDate);
        return _mapper.Map<List<InboundEntryDTO.CommandDTO>?>(list);
    }
}
