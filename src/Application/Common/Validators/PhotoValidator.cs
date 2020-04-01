using Application.Common.AppSettingHelpers.Main;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Common.Validators
{
    public class PhotoValidator : AbstractValidator<IFormFile>
    {
        private readonly PhotoSettings _photoSettings;

        private readonly IStringLocalizer<CommonValidatorsResource> _commonLocalizer;

        public PhotoValidator(IOptions<PhotoSettings> photoSettings
            , IStringLocalizer<CommonValidatorsResource> commonLocalizer)
        {
            _photoSettings = photoSettings.Value;
            _commonLocalizer = commonLocalizer;

            RuleFor(v => v)
                .Custom(BeValidUploadedPhoto);
        }

        public void BeValidUploadedPhoto(IFormFile uploadedPhoto
            , CustomContext context)
        {
            if (uploadedPhoto == null)
            {
                context.AddFailure(_commonLocalizer["PhotoNull"]);
                return;
            }

            if (string.IsNullOrWhiteSpace(uploadedPhoto.ContentType)
                || !_photoSettings.MimeContentTypes.Contains(uploadedPhoto
                    .ContentType))
            {
                context.AddFailure(_commonLocalizer["PhotoErrorType"]);
                return;
            }

            if (uploadedPhoto.Length > _photoSettings.MaxLengthBytes)
            {
                context.AddFailure(_commonLocalizer["PhotoMaxLength"
                    , _photoSettings.MaxLengthBytes / 1048576]);
            }

            if (uploadedPhoto.FileName.Length > _photoSettings.FileNameMaxLength)
            {
                context.AddFailure(_commonLocalizer["FileNameMaxLength"
                    , _photoSettings.FileNameMaxLength
                    , uploadedPhoto.FileName]);
            }
        }
    }
}