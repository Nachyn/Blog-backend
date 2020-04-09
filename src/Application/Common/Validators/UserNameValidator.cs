using System.Text.RegularExpressions;
using Application.Common.AppSettingHelpers.Entities;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Common.Validators
{
    public class UserNameValidator : PropertyValidator
    {
        private readonly AppUserSettings _appUserSettings;

        public UserNameValidator(IStringLocalizer<CommonValidatorsResource> commonLocalizer
            , IOptions<AppUserSettings> appUserOptions)
            : base(commonLocalizer["UsernameInvalid"
                , appUserOptions.Value.UsernameMinLength
                , appUserOptions.Value.UsernameMaxLength])
        {
            _appUserSettings = appUserOptions.Value;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var username = (string) context.PropertyValue;

            return !string.IsNullOrWhiteSpace(username)
                   && username.Length >= _appUserSettings.UsernameMinLength
                   && username.Length <= _appUserSettings.UsernameMaxLength
                   && Regex.IsMatch(username, _appUserSettings.UsernameRegex);
        }
    }
}