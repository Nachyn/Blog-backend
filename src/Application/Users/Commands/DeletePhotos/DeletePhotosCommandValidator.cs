using Application.Common.Validators;
using FluentValidation;

namespace Application.Users.Commands.DeletePhotos
{
    public class DeletePhotosCommandValidator : AbstractValidator<DeletePhotosCommand>
    {
        public DeletePhotosCommandValidator(IdsCountValidator idsValidator)
        {
            RuleFor(v => v.Ids).SetValidator(idsValidator);
        }
    }
}