using MediatR;
using ZyxLogistica.Application.DTOs;

namespace ZyxLogistica.Application.CQRS.Queries;

public record InboundEntryQuery
{
    public record GetInboundEntryByIdQuery(Guid Id) : IRequest<InboundEntryDTO.CommandDTO?>;
    public record GetInboundEntriesByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<List<InboundEntryDTO.CommandDTO>?>;
}
