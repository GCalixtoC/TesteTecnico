using FluentValidation;

namespace TesteFullStack.Application.DTOs.Requests.Validators
{
    public class UpdatePersonRequestValidator : AbstractValidator<UpdatePersonRequest>
    {
        public UpdatePersonRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("O Id é obrigatório.")
                .NotEmpty()
                .WithMessage("O Id não pode estar vazio.");

            RuleFor(x => x.Name)
                .MaximumLength(200);

            RuleFor(x => x.Age)
                .GreaterThan(0)
                .When(x => x.Age.HasValue)
                .WithMessage("A idade deve ser maior que zero.");
        }
    }
}
