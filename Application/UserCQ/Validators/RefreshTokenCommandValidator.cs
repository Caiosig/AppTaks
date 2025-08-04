using Application.UserCQ.Commands;
using FluentValidation;

namespace Application.UserCQ.Validators
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username é obrigatório.")
                .MaximumLength(50).WithMessage("Username não pode exceder 50 caracteres.");
        }
    }
}
