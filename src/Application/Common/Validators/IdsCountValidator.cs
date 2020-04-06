using System.Collections.Generic;
using Application.Common.Extensions;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;

namespace Application.Common.Validators
{
    public class IdsCountValidator : PropertyValidator
    {
        public IdsCountValidator(IStringLocalizer<CommonValidatorsResource> commonLocalizer)
            : base(commonLocalizer["IdsEmpty"])
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var list = (IEnumerable<int>) context.PropertyValue;
            return !list.IsNullOrEmpty();
        }
    }
}