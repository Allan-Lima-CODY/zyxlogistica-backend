using AutoMapper;
using MediatR;
using System.Text.Json;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;

namespace ZyxLogistica.Application.CQRS.Queries.Handlers;

public class OrderQueryHandler(IOrderService _orderService, IOrderRepository _orderRepository, IMapper _mapper) : 
    IRequestHandler<OrderQuery.GetOrdersByDateRangeQuery, List<OrderDTO.CommandDTO>?>,
    IRequestHandler<OrderQuery.GetPendingOrdersQuery, List<OrderDTO.ListDTO>?>
{
    public async Task<List<OrderDTO.CommandDTO>?> Handle(OrderQuery.GetOrdersByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetOrdersByDateRangeAsync(request.StartDate, request.EndDate);

        return _mapper.Map<List<OrderDTO.CommandDTO>?>(orders);
    }

    public async Task<List<OrderDTO.ListDTO>?> Handle(OrderQuery.GetPendingOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = await _orderRepository.GetPendingOrdersAsync();
        return _mapper.Map<List<OrderDTO.ListDTO>>(result);
    }
}
