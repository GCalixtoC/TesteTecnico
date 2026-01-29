using FluentValidation;
using TesteFullStack.Application.DTOs.Requests;

namespace TesteFullStack.Application.Validators
{
    public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .WithMessage("A descrição é obrigatória.")
                .MaximumLength(400);

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");

            RuleFor(x => x.Type)
                .NotEmpty().NotNull()
                .WithMessage("O tipo é obrigatório.")
                .Must(x => x.ToLower() == "despesa" || x.ToLower() == "receita")
                .WithMessage("O tipo deve ser 'despesa' ou 'receita'.");

            RuleFor(x => x.PersonId)
                .NotEmpty()
                .NotNull()
                .WithMessage("O identificador da pessoa é obrigatório.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .NotNull()
                .WithMessage("O identificador da categoria é obrigatório.");
        }
    }
}
