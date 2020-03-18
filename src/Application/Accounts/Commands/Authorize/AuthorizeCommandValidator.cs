using System;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Accounts.Commands.Authorize
{
    public class AuthorizeCommandValidator : AbstractValidator<AuthorizeCommand>
    {
        public AuthorizeCommandValidator(
            IStringLocalizer<AccountsResource> accountLocalizer)
        {
            RuleFor(v => v.Type)
                .Must(BeValidType)
                .WithMessage(accountLocalizer["LoginType"
                    , Token.TypePassword
                    , Token.TypeRefresh]);

            var emailValid = accountLocalizer["EmailValid"];
            RuleFor(v => v.Email)
                .NotNull()
                .WithMessage(emailValid)
                .EmailAddress()
                .WithMessage(emailValid);

            var enterPassword = accountLocalizer["EnterPassword"];
            RuleFor(v => v.Password)
                .NotEmpty()
                .When(c => !string.IsNullOrWhiteSpace(c.Type)
                           && c.Type.Equals(Token.TypePassword
                               , StringComparison.OrdinalIgnoreCase))
                .WithMessage(enterPassword);

            var enterRefreshToken = accountLocalizer["EnterRefreshToken"];
            RuleFor(v => v.RefreshToken)
                .NotEmpty()
                .When(c => !string.IsNullOrWhiteSpace(c.Type)
                           && c.Type.Equals(Token.TypeRefresh
                               , StringComparison.OrdinalIgnoreCase))
                .WithMessage(enterRefreshToken);
        }

        public bool BeValidType(string type)
        {
            return !string.IsNullOrWhiteSpace(type)
                   && (type.Equals(Token.TypePassword, StringComparison.OrdinalIgnoreCase)
                       || type.Equals(Token.TypeRefresh,
                           StringComparison.OrdinalIgnoreCase));
        }
    }
}