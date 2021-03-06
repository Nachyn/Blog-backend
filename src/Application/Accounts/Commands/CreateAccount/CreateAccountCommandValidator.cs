﻿using Application.Common.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator(UserNameValidator userNameValidator
            , EmailValidator emailValidator
            , IStringLocalizer<AccountsResource> accountLocalizer)
        {
            RuleFor(v => v.Email)
                .SetValidator(emailValidator);

            RuleFor(v => v.Username)
                .SetValidator(userNameValidator);

            RuleFor(v => v.Password)
                .Must(p => !string.IsNullOrWhiteSpace(p))
                .WithMessage(accountLocalizer["EnterPassword"]);
        }
    }
}