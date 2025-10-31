using MediatR;
using ZyxLogistica.Application.DTOs;

namespace ZyxLogistica.Application.CQRS.Queries;

public record ExpeditionQuery
{
    public record GetExpeditionsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<List<ExpeditionDTO.CommandDTO>?>;
    public record GetExpeditionByIdQuery(Guid Id) : IRequest<ExpeditionDTO.CommandDTO?>;
}
