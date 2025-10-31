using FluentValidation;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Application.CQRS.Commands.InventoryCommand;

namespace ZyxLogistica.Application.Validators;

public class InventoryValidator
{
    public class InventoryInputValidator : AbstractValidator<CreateInventoryCommand>
    {
        public InventoryInputValidator(IInventoryService inventoryService)
        {
            RuleFor(x => x.InventoryInput.ProductCode)
                .NotNull()
                    .WithMessage("ProductCode não pode ser nulo")
                .NotEmpty()
                    .WithMessage("ProductCode não pode ser vazio")
                .MaximumLength(50)
                    .WithMessage("ProductCode não pode ter mais que 50 caracteres")
                .Must(code =>
                {
                    var existing = inventoryService.GetInventoryByProductCode(code);
                    return existing == null;
                })
                    .WithMessage("Já existe um item com este código de produto");

            RuleFor(x => x.InventoryInput.Description)
                .NotNull()
                    .WithMessage("Descrição não pode ser nula")
                .NotEmpty()
                    .WithMessage("Descrição não pode ser vazia")
                .MaximumLength(200)
                    .WithMessage("Descrição não pode ter mais que 200 caracteres");

            RuleFor(x => x.InventoryInput.Quantity)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Quantidade deve ser >= 0");

            RuleFor(x => x.InventoryInput.Price)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Preço deve ser >= 0");

        }
    }

    public class InventoryUpdateValidator : AbstractValidator<UpdateInventoryCommand>
    {
        public InventoryUpdateValidator()
        {
            RuleFor(x => x.InventoryUpdate.Description)
                .NotNull()
                    .WithMessage("Descrição não pode ser nula")
                .NotEmpty()
                    .WithMessage("Descrição não pode ser vazia")
                .MaximumLength(200)
                    .WithMessage("Descrição não pode ter mais que 200 caracteres");

            RuleFor(x => x.InventoryUpdate.Price)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Preço deve ser >= 0");

        }
    }
}
