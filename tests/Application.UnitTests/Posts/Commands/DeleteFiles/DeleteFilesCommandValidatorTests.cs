using System.Collections.Generic;
using Application.Common.Validators;
using Application.Posts.Commands.DeleteFiles;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.DeleteFiles
{
    public class DeleteFilesCommandValidatorTests : PostsTestBase
    {
        private DeleteFilesCommandValidator GetNewValidator()
        {
            return new DeleteFilesCommandValidator(new IdsCountValidator(CommonLocalizer)
                , new IdRangeValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.Ids
                , new DeleteFilesCommand {Ids = new List<int> {1, 2, 3, 4, 5}});

            validator.ShouldNotHaveValidationErrorFor(v => v.PostId
                , new DeleteFilesCommand {PostId = 1});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Ids
                , new DeleteFilesCommand {Ids = new List<int>()});

            validator.ShouldHaveValidationErrorFor(v => v.Ids
                , new DeleteFilesCommand {Ids = null});

            validator.ShouldHaveValidationErrorFor(v => v.PostId
                , new DeleteFilesCommand {PostId = 0});

            validator.ShouldHaveValidationErrorFor(v => v.PostId
                , new DeleteFilesCommand {PostId = -1});
        }
    }
}