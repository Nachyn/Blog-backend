using Application.Common.Validators;
using FluentValidation;

namespace Application.Accounts.Commands.SendRestoreCode
{
    public class SendRestoreCodeCommandValidator : AbstractValidator<SendRestoreCodeCommand>
    {
        public SendRestoreCodeCommandValidator(EmailValidator emailValidator)
        {
            RuleFor(v => v.Email)
                .SetValidator(emailValidator);
        }
    }
}