using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Users.Queries.FindUser
{
    public class FindUserQueryValidator : AbstractValidator<FindUserQuery>
    {
        public FindUserQueryValidator(IStringLocalizer<UsersResource> userResource)
        {
            var errorInfo = userResource["UserNameNull"];

            RuleFor(v => v.UserName)
                .Must(u => !string.IsNullOrWhiteSpace(u))
                .WithMessage(errorInfo);
        }
    }
}