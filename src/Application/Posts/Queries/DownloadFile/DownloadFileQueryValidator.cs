using Application.Common.Validators;
using FluentValidation;

namespace Application.Posts.Queries.DownloadFile
{
    public class DownloadFileQueryValidator : AbstractValidator<DownloadFileQuery>
    {
        public DownloadFileQueryValidator(IdRangeValidator idValidator)
        {
            RuleFor(v => (int?) v.FileId)
                .SetValidator(idValidator);
        }
    }
}