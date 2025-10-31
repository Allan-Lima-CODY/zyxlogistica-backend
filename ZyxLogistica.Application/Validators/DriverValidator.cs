using FluentValidation;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Application.CQRS.Commands.DriverCommand;

namespace ZyxLogistica.Application.Validators;

public class DriverValidator
{
    public class DriverInputValidator : AbstractValidator<CreateDriverCommand>
    {
        public DriverInputValidator(IDriverService driverService)
        {
            RuleFor(d => d.DriverInput.Name)
                .NotNull()
                    .WithMessage("Nome não pode ser nulo")
                .NotEmpty()
                    .WithMessage("Nome não pode ser vazio")
                .MaximumLength(200)
                    .WithMessage("Nome não pode ter mais que 200 caracteres")
                .MinimumLength(2)
                    .WithMessage("Nome não pode ter menos que 2 caracteres");

            RuleFor(d => d.DriverInput.Phone)
                .NotNull()
                    .WithMessage("Telefone não pode ser nulo")
                .NotEmpty()
                    .WithMessage("Telefone não pode ser vazio")
                .MaximumLength(20)
                    .WithMessage("Telefone não pode ter mais que 20 caracteres")
                .Must(phone =>
                {
                    var existing = driverService.GetDriverByPhone(phone);
                    return existing == null;
                })
                    .WithMessage("Já existe um motorista com este telefone");

            RuleFor(d => d.DriverInput.Cnh)
                .NotNull()
                    .WithMessage("CNH não pode ser nula")
                .NotEmpty()
                    .WithMessage("CNH não pode ser vazia")
                .MaximumLength(20)
                    .WithMessage("CNH não pode ter mais que 20 caracteres")
                .Must(cnh =>
                {
                    var existing = driverService.GetDriverByCnh(cnh);
                    return existing == null;
                })
                    .WithMessage("Já existe um motorista com esta CNH");

            RuleFor(d => d.DriverInput.CnhCategory)
                .IsInEnum()
                    .WithMessage("Categoria de CNH inválida");
        }
    }

    public class DriverUpdateValidator : AbstractValidator<UpdateDriverCommand>
    {
        public DriverUpdateValidator(IDriverService driverService)
        {
            RuleFor(d => d.DriverUpdate.Name)
                .NotNull()
                    .WithMessage("Nome não pode ser nulo")
                .NotEmpty()
                    .WithMessage("Nome não pode ser vazio")
                .MaximumLength(200)
                    .WithMessage("Nome não pode ter mais que 200 caracteres")
                .MinimumLength(2)
                    .WithMessage("Nome não pode ter menos que 2 caracteres");

            RuleFor(d => d.DriverUpdate.Phone)
                .NotNull()
                    .WithMessage("Telefone não pode ser nulo")
                .NotEmpty()
                    .WithMessage("Telefone não pode ser vazio")
                .MaximumLength(20)
                    .WithMessage("Telefone não pode ter mais que 20 caracteres")
                .Must(phone =>
                {
                    var existing = driverService.GetDriverByPhone(phone);
                    return existing == null;
                })
                    .WithMessage("Já existe um motorista com este telefone");
        }
    }
}
