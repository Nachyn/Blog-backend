using Application.Common.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Users.Commands.LoadPhotos
{
    public class LoadPhotosCommandValidator : AbstractValidator<LoadPhotosCommand>
    {
        public LoadPhotosCommandValidator(PhotoValidator photoValidator
            , IStringLocalizer<UsersResource> userLocalizer)
        {
            RuleFor(v => v.Photos)
                .NotEmpty()
                .WithMessage(userLocalizer["PhotosEmpty"]);

            RuleForEach(v => v.Photos)
                .SetValidator(photoValidator);
        }
    }
}