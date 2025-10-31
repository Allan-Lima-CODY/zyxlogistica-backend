using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;

namespace ZyxLogistica.Application.CQRS.Commands;

public record OrderCommand
{
    public record CreateOrderCommand(OrderModel.OrderInput OrderInput) : IRequest<OrderDTO.CommandDTO>;
}
