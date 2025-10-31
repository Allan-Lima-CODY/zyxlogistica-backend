using FluentValidation;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Domain.Enums;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Application.Validators;

public class ExpeditionValidator
{
    public class ExpeditionInputValidator : AbstractValidator<ExpeditionCommand.CreateExpeditionCommand>
    {
        public ExpeditionInputValidator(
            IExpeditionRepository expeditionRepository,
            IOrderRepository orderRepository,
            ITruckRepository truckRepository,
            IDriverRepository driverRepository)
        {
            RuleFor(c => c.Input.OrderId)
                .NotEmpty()
                .WithMessage("OrderId é obrigatório.")
                .MustAsync(async (cmd, orderId, ct) =>
                {
                    var order = await orderRepository.GetById(orderId);
                    return order != null;
                })
                .WithMessage("Pedido não encontrado.");

            RuleFor(c => c.Input.DeliveryForecast)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("DeliveryForecast não pode ser no passado.")
                .Must((cmd, df) => df <= DateTime.UtcNow.Date.AddDays(30))
                .WithMessage("DeliveryForecast muito distante (máx 30 dias).");

            RuleFor(c => c.Input.Observation)
                .MaximumLength(500)
                .WithMessage("Observação não pode ter mais que 500 caracteres.");

            RuleFor(c => c.Input.DriverId)
                .NotEmpty()
                .WithMessage("DriverId é obrigatório.")
                .MustAsync(async (cmd, driverId, ct) =>
                {
                    var driver = await driverRepository.GetById(driverId);
                    return driver != null;
                })
                .WithMessage("Motorista não encontrado.");

            RuleFor(c => c.Input.TruckId)
                .NotEmpty()
                .WithMessage("TruckId é obrigatório.")
                .MustAsync(async (cmd, truckId, ct) =>
                {
                    var truck = await truckRepository.GetById(truckId);
                    return truck != null;
                })
                .WithMessage("Caminhão não encontrado.");

            RuleFor(c => c.Input)
                .MustAsync(async (input, ct) =>
                {
                    var existingByOrder = await expeditionRepository.GetByOrderId(input.OrderId);
                    if (existingByOrder != null)
                    {
                        var status = existingByOrder.Order.Status;
                        if (status is not (OrderStatus.Delivered or OrderStatus.Canceled))
                            return false;
                    }
                    return true;
                })
                .WithMessage("Já existe uma expedição ativa para esse pedido.");

            RuleFor(c => c.Input)
                .MustAsync(async (input, ct) =>
                {
                    var conflictDriver = await expeditionRepository.GetActiveByDriver(input.DriverId);
                    return conflictDriver == null;
                })
                .WithMessage("Motorista está ocupado em outra expedição ativa.");

            RuleFor(c => c.Input)
                .MustAsync(async (input, ct) =>
                {
                    var conflictTruck = await expeditionRepository.GetActiveByTruck(input.TruckId);
                    return conflictTruck == null;
                })
                .WithMessage("Caminhão está ocupado em outra expedição ativa.");
        }
    }

    public class ExpeditionUpdateValidator : AbstractValidator<ExpeditionCommand.UpdateExpeditionCommand>
    {
        public ExpeditionUpdateValidator(
            IExpeditionRepository expeditionRepository,
            IOrderRepository orderRepository,
            ITruckRepository truckRepository,
            IDriverRepository driverRepository)
        {
            RuleFor(c => c.Update.DriverId)
                .NotEmpty()
                .WithMessage("DriverId é obrigatório.")
                .MustAsync(async (cmd, driverId, ct) =>
                {
                    var driver = await driverRepository.GetById(driverId);
                    return driver != null;
                })
                .WithMessage("Motorista não encontrado.");

            RuleFor(c => c.Update.TruckId)
                .NotEmpty()
                .WithMessage("TruckId é obrigatório.")
                .MustAsync(async (cmd, truckId, ct) =>
                {
                    var truck = await truckRepository.GetById(truckId);
                    return truck != null;
                })
                .WithMessage("Caminhão não encontrado.");

            RuleFor(c => c.Update.DeliveryForecast)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("DeliveryForecast não pode ser no passado.")
                .Must((cmd, df) => df <= DateTime.UtcNow.Date.AddDays(30))
                .WithMessage("DeliveryForecast muito distante (máx 30 dias).");

            RuleFor(c => c.Update.Observation)
                .MaximumLength(500)
                .WithMessage("Observação não pode ter mais que 500 caracteres.");

            RuleFor(c => c)
                .MustAsync(async (cmd, ct) =>
                {
                    var expedition = await expeditionRepository.GetById(cmd.Id);
                    if (expedition == null) return false;

                    var conflictDriver = await expeditionRepository.GetActiveByDriver(cmd.Update.DriverId);
                    if (conflictDriver != null && conflictDriver.Id != cmd.Id) return false;

                    var conflictTruck = await expeditionRepository.GetActiveByTruck(cmd.Update.TruckId);
                    if (conflictTruck != null && conflictTruck.Id != cmd.Id) return false;

                    var order = expedition.Order;
                    if (order.Status is not (OrderStatus.Pending or OrderStatus.InSeparation))
                        return false;

                    return true;
                })
                .WithMessage("Não é possível atualizar expedição: conflito de recursos ou status do pedido não permite atualização.");
        }
    }
}
