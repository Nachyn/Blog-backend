using System.Text.RegularExpressions;
using Application.Common.AppSettingHelpers.Entities;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Common.Validators
{
    public class EmailValidator : PropertyValidator
    {
        private const string EmailRegex =
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]{2,}\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        private readonly AppUserSettings _appUserSettings;

        public EmailValidator(IOptions<AppUserSettings> appUserSettings
            , IStringLocalizer<CommonValidatorsResource> commonLocalizer)
            : base(commonLocalizer["EmailInvalid"])
        {
            _appUserSettings = appUserSettings.Value;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var email = (string) context.PropertyValue;

            return !string.IsNullOrWhiteSpace(email)
                   && email.Length <= _appUserSettings.EmailMaxLength
                   && Regex.IsMatch(email, EmailRegex);
        }
    }
}