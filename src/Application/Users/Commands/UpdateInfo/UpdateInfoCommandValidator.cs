using Application.Common.Validators;
using FluentValidation;

namespace Application.Users.Commands.UpdateInfo
{
    public class UpdateInfoCommandValidator : AbstractValidator<UpdateInfoCommand>
    {
        public UpdateInfoCommandValidator(IdRangeValidator idValidator
            , UserNameValidator userNameValidator)
        {
            RuleFor(v => v.AvatarPhotoId)
                .SetValidator(idValidator);

            RuleFor(v => v.UserName)
                .SetValidator(userNameValidator);
        }
    }
}