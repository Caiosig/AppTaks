using Application.UserCQ.Commands;
using FluentValidation;

namespace Application.UserCQ.Validators
{
    public class CreateUserComandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserComandValidator()
        {
            // Definindo as regras de validação para o campo email
            RuleFor(x => x.Email).NotEmpty().WithMessage("O Campo 'email' não pode ser vazio.")
                .EmailAddress().WithMessage("O Campo 'email' não é valido.");

            // Definindo as regras de validação para o campo Username
            RuleFor(x => x.Username).NotEmpty().WithMessage("O Campo 'Username' não pode ser vazio.");
        }
    }
}
