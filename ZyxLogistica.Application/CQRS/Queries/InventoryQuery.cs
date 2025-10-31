using MediatR;
using ZyxLogistica.Application.DTOs;

namespace ZyxLogistica.Application.CQRS.Queries;

public record InventoryQuery
{
    public record GetInventoriesByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<List<InventoryDTO.CommandDTO>?>;
    public record GetAvailableInventoriesQuery() : IRequest<List<InventoryDTO.CommandDTO>?>;
}
