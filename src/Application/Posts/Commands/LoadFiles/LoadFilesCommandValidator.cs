using Application.Common.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Posts.Commands.LoadFiles
{
    public class LoadFilesCommandValidator : AbstractValidator<LoadFilesCommand>
    {
        public LoadFilesCommandValidator(FileValidator fileValidator
            , IStringLocalizer<PostsResource> postLocalizer
            , IdRangeValidator idValidator)
        {
            RuleFor(v => v.Files)
                .NotEmpty()
                .WithMessage(postLocalizer["FilesEmpty"]);

            RuleForEach(v => v.Files)
                .SetValidator(fileValidator);

            RuleFor(v => (int?) v.PostId)
                .SetValidator(idValidator);
        }
    }
}