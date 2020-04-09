using Application.Common.Validators;
using FluentValidation;

namespace Application.Users.Queries.GetUserInfo
{
    public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
    {
        public GetUserInfoQueryValidator(IdRangeValidator idValidator)
        {
            RuleFor(v => (int?) v.UserId)
                .SetValidator(idValidator);
        }
    }
}