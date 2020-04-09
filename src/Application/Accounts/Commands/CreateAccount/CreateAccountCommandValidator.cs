using Application.Common.Validators;
using FluentValidation;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator(UserNameValidator userNameValidator
            , EmailValidator emailValidator)
        {
            RuleFor(v => v.Email)
                .SetValidator(emailValidator);

            RuleFor(v => v.Username)
                .SetValidator(userNameValidator);
        }
    }
}