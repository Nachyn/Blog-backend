using System.Collections.Generic;
using Application.Common.Validators;
using Application.Posts.Commands.DeletePosts;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.DeletePosts
{
    public class DeletePostsCommandValidatorTests : PostsTestBase
    {
        private DeletePostsCommandValidator GetNewValidator()
        {
            return new DeletePostsCommandValidator(new IdsCountValidator(CommonLocalizer)
                , new IdRangeValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.Ids
                , new DeletePostsCommand {Ids = new List<int> {1, 2, 3, 4, 5}});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Ids
                , new DeletePostsCommand {Ids = new List<int>()});

            validator.ShouldHaveValidationErrorFor(v => v.Ids
                , new DeletePostsCommand {Ids = null});
        }
    }
}