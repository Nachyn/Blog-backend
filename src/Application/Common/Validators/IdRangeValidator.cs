using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Common.Validators
{
    public class IdRangeValidator : AbstractValidator<int?>
    {
        public IdRangeValidator(IStringLocalizer<CommonValidatorsResource> commonLocalizer)
        {
            RuleFor(v => v)
                .Must(id => id > 0)
                .WithMessage(commonLocalizer["IdRange"]);
        }
    }
}