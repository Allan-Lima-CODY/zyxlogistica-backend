using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Interfaces.Services;

namespace ZyxLogistica.Application.CQRS.Queries.Handlers;

public class ExpeditionQueryHandler(IExpeditionService _expeditionService, IMapper _mapper) :
    IRequestHandler<ExpeditionQuery.GetExpeditionsByDateRangeQuery, List<ExpeditionDTO.CommandDTO>?>,
    IRequestHandler<ExpeditionQuery.GetExpeditionByIdQuery, ExpeditionDTO.CommandDTO?>
{
    public async Task<List<ExpeditionDTO.CommandDTO>?> Handle(ExpeditionQuery.GetExpeditionsByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var list = await _expeditionService.GetByDateRangeAsync(request.StartDate, request.EndDate);
        return _mapper.Map<List<ExpeditionDTO.CommandDTO>?>(list);
    }

    public async Task<ExpeditionDTO.CommandDTO?> Handle(ExpeditionQuery.GetExpeditionByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _expeditionService.GetByIdAsync(request.Id);
        return _mapper.Map<ExpeditionDTO.CommandDTO?>(item);
    }
}
