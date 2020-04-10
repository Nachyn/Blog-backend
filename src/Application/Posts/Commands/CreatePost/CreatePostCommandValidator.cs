using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator(IStringLocalizer<PostsResource> postLocalizer)
        {
            RuleFor(v => v.Text)
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage(postLocalizer["EnterText"]);
        }
    }
}