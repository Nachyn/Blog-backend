using System.Text.RegularExpressions;
using Application.Common.AppSettingHelpers.Entities;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Common.Validators
{
    public class UserNameValidator : PropertyValidator
    {
        private readonly AppUserOptions _appUserOptions;

        public UserNameValidator(IStringLocalizer<CommonValidatorsResource> commonLocalizer
            , IOptions<AppUserOptions> appUserOptions)
            : base(commonLocalizer["UsernameInvalid"
                , appUserOptions.Value.UsernameMinLength
                , appUserOptions.Value.UsernameMaxLength])
        {
            _appUserOptions = appUserOptions.Value;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var username = (string) context.PropertyValue;

            return !string.IsNullOrWhiteSpace(username)
                   && username.Length >= _appUserOptions.UsernameMinLength
                   && username.Length <= _appUserOptions.UsernameMaxLength
                   && Regex.IsMatch(username, _appUserOptions.UsernameRegex);
        }
    }
}