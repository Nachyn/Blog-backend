using Application.Common.Validators;
using FluentValidation;

namespace Application.Posts.Commands.DeleteFiles
{
    public class DeleteFilesCommandValidator : AbstractValidator<DeleteFilesCommand>
    {
        public DeleteFilesCommandValidator(IdsCountValidator idsCountValidator
            , IdRangeValidator idRangeValidator)
        {
            RuleFor(v => v.Ids)
                .SetValidator(idsCountValidator);

            RuleFor(v => (int?) v.PostId)
                .SetValidator(idRangeValidator);
        }
    }
}