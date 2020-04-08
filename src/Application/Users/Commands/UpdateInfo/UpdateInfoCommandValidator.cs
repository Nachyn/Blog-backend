using System.Text.RegularExpressions;
using Application.Common.AppSettingHelpers.Entities;
using Application.Common.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.UpdateInfo
{
    public class UpdateInfoCommandValidator : AbstractValidator<UpdateInfoCommand>
    {
        private readonly AppUserOptions _appUserOptions;

        public UpdateInfoCommandValidator(IdRangeValidator idValidator
            , IOptions<AppUserOptions> appUserOptions
            , IStringLocalizer<UsersResource> userLocalizer)
        {
            _appUserOptions = appUserOptions.Value;

            RuleFor(v => v.AvatarPhotoId)
                .SetValidator(idValidator);

            RuleFor(v => v.UserName)
                .Must(BeValidUsername)
                .WithMessage(userLocalizer["UsernameInvalid"
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