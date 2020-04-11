using Application.Common.Validators;
using FluentValidation;

namespace Application.Posts.Queries.GetPosts
{
    public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator(IdRangeValidator idValidator
            , PaginationValidator paginationValidator)
        {
            RuleFor(v => v)
                .SetValidator(paginationValidator);

            RuleFor(v => (int?) v.UserId)
                .SetValidator(idValidator);
        }
    }
}