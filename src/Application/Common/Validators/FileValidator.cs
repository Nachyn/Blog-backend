using Application.Common.AppSettingHelpers.Main;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Common.Validators
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        private readonly FileSettings _fileSettings;

        private readonly IStringLocalizer<CommonValidatorsResource> _commonLocalizer;

        public FileValidator(IOptions<FileSettings> fileSettings
            , IStringLocalizer<CommonValidatorsResource> commonLocalizer)
        {
            _fileSettings = fileSettings.Value;
            _commonLocalizer = commonLocalizer;

            RuleFor(v => v)
                .Custom(BeValidUploadedFile);
        }

        public void BeValidUploadedFile(IFormFile uploadedFile
            , CustomContext context)
        {
            if (uploadedFile == null)
            {
                context.AddFailure(_commonLocalizer["FileNull"]);
                return;
            }

            if (uploadedFile.Length > _fileSettings.MaxLengthBytes)
            {
                context.AddFailure(_commonLocalizer["FileMaxLength"
                    , _fileSettings.MaxLengthBytes / 1048576]);

                return;
            }

            if (uploadedFile.FileName.Length > _fileSettings.FileNameMaxLength)
            {
                context.AddFailure(_commonLocalizer["FileNameMaxLength"
                    , _fileSettings.FileNameMaxLength
                    , uploadedFile.FileName]);
            }
        }
    }
}