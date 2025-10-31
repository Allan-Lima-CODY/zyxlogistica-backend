using FluentValidation;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Application.Validators;

public class OrderValidator
{
    public class OrderInputValidator : AbstractValidator<OrderCommand.CreateOrderCommand>
    {
        public OrderInputValidator(IOrderRepository orderRepository)
        {
            RuleFor(o => o.OrderInput.OrderNumber)
                .NotEmpty()
                    .WithMessage("Número do pedido é obrigatório.");

            RuleFor(o => o.OrderInput.CustomerName)
                .NotEmpty()
                    .WithMessage("Nome do cliente é obrigatório.");

            RuleForEach(o => o.OrderInput.Items)
                .ChildRules(items =>
                {
                    items.RuleFor(i => i.Quantity)
                        .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");
                });

            RuleFor(o => o.OrderInput.OrderNumber)
                .Must(orderNumber =>
                {
                    var existing = orderRepository.GetByOrderNumber(orderNumber);
                    return existing == null;
                })
                    .WithMessage("Já existe um pedido com este número.");

        }
    }
}
