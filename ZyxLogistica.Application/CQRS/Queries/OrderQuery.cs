using MediatR;
using ZyxLogistica.Application.DTOs;

namespace ZyxLogistica.Application.CQRS.Queries;

public record OrderQuery
{
    public record GetOrdersByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<List<OrderDTO.CommandDTO>?>;
    public record GetPendingOrdersQuery() : IRequest<List<OrderDTO.ListDTO>?>;
}