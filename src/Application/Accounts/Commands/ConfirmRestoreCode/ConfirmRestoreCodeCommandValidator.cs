using Application.Common.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Accounts.Commands.ConfirmRestoreCode
{
    public class ConfirmRestoreCodeCommandValidator : AbstractValidator<ConfirmRestoreCodeCommand>
    {
        public ConfirmRestoreCodeCommandValidator(EmailValidator emailValidator
            , IStringLocalizer<AccountsResource> accountLocalizer)
        {
            RuleFor(v => v.Email)
                .SetValidator(emailValidator);

            RuleFor(v => v.Code)
                .Must(c => !string.IsNullOrWhiteSpace(c))
                .WithMessage(accountLocalizer["EnterCode"]);

            RuleFor(v => v.Password)
                .Must(p => !string.IsNullOrWhiteSpace(p))
                .WithMessage(accountLocalizer["EnterPassword"]);
        }
    }
}