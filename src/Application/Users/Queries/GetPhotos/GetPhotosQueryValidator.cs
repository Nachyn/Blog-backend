using Application.Common.Validators;
using FluentValidation;

namespace Application.Users.Queries.GetPhotos
{
    public class GetPhotosQueryValidator : AbstractValidator<GetPhotosQuery>
    {
        public GetPhotosQueryValidator(PaginationValidator paginationValidator
            , IdRangeValidator idValidator)
        {
            RuleFor(v => v)
                .SetValidator(paginationValidator);
            RuleFor(v => (int?) v.UserId)
                .SetValidator(idValidator);
        }
    }
}