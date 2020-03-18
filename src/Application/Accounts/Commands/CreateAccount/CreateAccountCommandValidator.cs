using System.Text.RegularExpressions;
using Application.Common.AppSettingHelpers.Entities;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        private readonly AppUserOptions _appUserOptions;

        public CreateAccountCommandValidator(
            IStringLocalizer<AccountsResource> accountLocalizer
            , IOptions<AppUserOptions> appUserOptions)
        {
            _appUserOptions = appUserOptions.Value;

            var invalidEmailMessage =
                accountLocalizer["EmailValid", _appUserOptions.EmailMaxLength];

            RuleFor(v => v.Email)
                .NotNull()
                .WithMessage(invalidEmailMessage)
                .EmailAddress()
                .WithMessage(invalidEmailMessage)
                .MaximumLength(_appUserOptions.EmailMaxLength)
                .WithMessage(invalidEmailMessage);

            RuleFor(v => v.Username)
                .Must(BeValidUsername)
                .WithMessage(accountLocalizer["UsernameValid"
                    , _appUserOptions.UsernameMinLength
                    , _appUserOptions.UsernameMaxLength]);
        }

        public bool BeValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username)
                   && username.Length >= _appUserOptions.UsernameMinLength
                   && username.Length <= _appUserOptions.UsernameMaxLength
                   && Regex.IsMatch(username, _appUserOptions.UsernameRegex);
        }
    }
}