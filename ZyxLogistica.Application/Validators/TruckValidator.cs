using FluentValidation;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Application.CQRS.Commands.TruckCommand;

namespace ZyxLogistica.Application.Validators;

public class TruckValidator
{
    public class TruckInputValidator : AbstractValidator<CreateTruckCommand>
    {
        public TruckInputValidator(ITruckService truckService)
        {
            RuleFor(t => t.TruckInput.LicensePlate)
                .NotNull()
                    .WithMessage("Placa não pode ser nula")
                .NotEmpty()
                    .WithMessage("Placa não pode ser vazia")
                .MaximumLength(20)
                    .WithMessage("Placa não pode ter mais que 20 caracteres")
                .Must(plate =>
                {
                    var existing = truckService.GetTruckByLicensePlate(plate);
                    return existing == null;
                })
                    .WithMessage("Já existe um caminhão com esta placa");

            RuleFor(t => t.TruckInput.Model)
                .NotNull()
                    .WithMessage("Modelo não pode ser nulo")
                .NotEmpty()
                    .WithMessage("Modelo não pode ser vazio")
                .MaximumLength(100)
                    .WithMessage("Modelo não pode ter mais que 100 caracteres");

            RuleFor(t => t.TruckInput.Year)
                .GreaterThan(1900)
                    .WithMessage("Ano inválido")
                .LessThanOrEqualTo(DateTime.UtcNow.Year)
                    .WithMessage("Ano inválido");

            RuleFor(t => t.TruckInput.CapacityKg)
                .GreaterThan(0)
                    .WithMessage("Capacidade deve ser maior que 0");
        }
    }

    public class TruckUpdateValidator : AbstractValidator<UpdateTruckCommand>
    {
        public TruckUpdateValidator()
        {
            RuleFor(t => t.TruckUpdate.Model)
                .NotNull()
                    .WithMessage("Modelo não pode ser nulo")
                .NotEmpty()
                    .WithMessage("Modelo não pode ser vazio")
                .MaximumLength(100)
                    .WithMessage("Modelo não pode ter mais que 100 caracteres");

            RuleFor(t => t.TruckUpdate.Year)
                .GreaterThan(1900)
                    .WithMessage("Ano inválido")
                .LessThanOrEqualTo(DateTime.UtcNow.Year)
                    .WithMessage("Ano inválido");

            RuleFor(t => t.TruckUpdate.CapacityKg)
                .GreaterThan(0)
                    .WithMessage("Capacidade deve ser maior que 0");
        }
    }
}
