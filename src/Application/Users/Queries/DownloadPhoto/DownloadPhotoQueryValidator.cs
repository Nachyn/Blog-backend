using Application.Common.Validators;
using FluentValidation;

namespace Application.Users.Queries.DownloadPhoto
{
    public class DownloadPhotoQueryValidator : AbstractValidator<DownloadPhotoQuery>
    {
        public DownloadPhotoQueryValidator(IdRangeValidator idValidator)
        {
            RuleFor(v => (int?) v.PhotoId)
                .SetValidator(idValidator);
        }
    }
}