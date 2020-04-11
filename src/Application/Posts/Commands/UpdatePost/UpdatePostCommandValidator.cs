using Application.Common.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator(IdRangeValidator idValidator
            , IStringLocalizer<PostsResource> postLocalizer)
        {
            RuleFor(v => v.Text)
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage(postLocalizer["EnterText"]);

            RuleFor(v => (int?) v.PostId)
                .SetValidator(idValidator);
        }
    }
}