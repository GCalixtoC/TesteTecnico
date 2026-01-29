using FluentValidation;
using TesteFullStack.Application.DTOs.Requests;

namespace TesteFullStack.Application.Validators
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("A descrição é obrigatória.")
                .MaximumLength(400);

            RuleFor(x => x.Purpose)
                .NotEmpty()
                .WithMessage("A finalidade é obrigatória.")
                .Must(x => x!.ToLower() == "despesa" || x.ToLower() == "receita" || x.ToLower() == "ambas")
                .WithMessage("A finalidade deve ser 'despesa', 'receita' ou 'ambas'.");
        }
    }
}
