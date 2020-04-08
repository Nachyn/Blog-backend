using Application.Common.AppSettingHelpers.Entities;
using Application.Common.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator(
            IStringLocalizer<AccountsResource> accountLocalizer
            , UserNameValidator userNameValidator
            , IOptions<AppUserOptions> appUserOptionsOption)
        {
            var appUserOptions = appUserOptionsOption.Value;

            var invalidEmailMessage =
                accountLocalizer["EmailValid", appUserOptions.EmailMaxLength];

            RuleFor(v => v.Email)
                .NotNull()
                .WithMessage(invalidEmailMessage)
                .EmailAddress()
                .WithMessage(invalidEmailMessage)
                .MaximumLength(appUserOptions.EmailMaxLength)
                .WithMessage(invalidEmailMessage);

            RuleFor(v => v.Username)
                .SetValidator(userNameValidator);
        }
    }
}