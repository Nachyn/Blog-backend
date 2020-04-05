using Application.Common.Dtos;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Common.Validators
{
    public class PaginationValidator : AbstractValidator<PaginationRequestDto>
    {
        public PaginationValidator(IStringLocalizer<CommonValidatorsResource> commonLocalizer)
        {
            RuleFor(v => v.NumberPage)
                .Must(n => n > 0)
                .WithMessage(commonLocalizer["NumberPageRange"]);

            RuleFor(v => v.PageSize)
                .Must(s => s > 0)
                .WithMessage(commonLocalizer["PageSizeRange"]);
        }
    }
}