using FluentValidation;

namespace TesteFullStack.Application.DTOs.Requests.Validators
{
    public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
    {
        public CreatePersonRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("O nome é obrigatório.")
                .NotEmpty()
                .WithMessage("O nome não pode estar vazio.")
                .MaximumLength(200);

            RuleFor(x => x.Age)
                .GreaterThan(0)
                .WithMessage("A idade deve ser maior que zero.");
        }
    }
}
