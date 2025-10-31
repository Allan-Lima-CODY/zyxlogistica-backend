using FluentValidation;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Domain.Interfaces.Repositories;

namespace ZyxLogistica.Application.Validators;

public class InboundEntryValidator
{
    public class InboundEntryCreateValidator : AbstractValidator<InboundEntryCommand.CreateInboundEntryCommand>
    {
        public InboundEntryCreateValidator(IInventoryRepository inventoryRepository)
        {
            RuleFor(c => c.Input.Reference)
                .NotEmpty().WithMessage("Reference é obrigatório.")
                .MaximumLength(100).WithMessage("Reference não pode ter mais que 100 caracteres.");

            RuleFor(c => c.Input.SupplierName)
                .NotEmpty().WithMessage("SupplierName é obrigatório.")
                .MaximumLength(200).WithMessage("SupplierName não pode ter mais que 200 caracteres.");

            RuleFor(c => c.Input.Observation)
                .MaximumLength(500).WithMessage("Observation não pode ter mais que 500 caracteres.");

            RuleFor(c => c.Input.InventoryInput.ProductCode)
                .NotEmpty().WithMessage("ProductCode é obrigatório.")
                .MaximumLength(50).WithMessage("ProductCode não pode ter mais que 50 caracteres.");

            RuleFor(c => c.Input.InventoryInput.Description)
                .NotEmpty().WithMessage("Descrição é obrigatória.")
                .MaximumLength(200).WithMessage("Descrição não pode ter mais que 200 caracteres.");

            RuleFor(c => c.Input.InventoryInput.Quantity)
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que 0.");
        }
    }

    public class InboundEntryUpdateValidator : AbstractValidator<InboundEntryCommand.UpdateInboundEntryCommand>
    {
        public InboundEntryUpdateValidator()
        {
            RuleFor(c => c.Update.Reference)
                .NotEmpty().WithMessage("Reference é obrigatório.")
                .MaximumLength(100).WithMessage("Reference não pode ter mais que 100 caracteres.");

            RuleFor(c => c.Update.SupplierName)
                .NotEmpty().WithMessage("SupplierName é obrigatório.")
                .MaximumLength(200).WithMessage("SupplierName não pode ter mais que 200 caracteres.");

            RuleFor(c => c.Update.Observation)
                .MaximumLength(500).WithMessage("Observation não pode ter mais que 500 caracteres.");
        }
    }
}
