using Application.Common.Validators;
using FluentValidation;

namespace Application.Posts.Commands.DeletePosts
{
    public class DeletePostsCommandValidator : AbstractValidator<DeletePostsCommand>
    {
        public DeletePostsCommandValidator(IdsCountValidator idsCountValidator
            , IdRangeValidator idValidator)
        {
            RuleFor(v => v.Ids)
                .SetValidator(idsCountValidator);
        }
    }
}