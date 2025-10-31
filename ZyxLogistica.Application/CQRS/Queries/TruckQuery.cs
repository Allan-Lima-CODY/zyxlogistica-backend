using MediatR;
using ZyxLogistica.Application.DTOs;
using static ZyxLogistica.Application.DTOs.TruckDTO;

namespace ZyxLogistica.Application.CQRS.Queries;

public record TruckQuery
{
    public record GetTrucksByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<List<CommandDTO>?>;
    public record GetAvailableTrucksQuery() : IRequest<List<ListDTO>?>;
}
