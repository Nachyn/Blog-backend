using System;
using Application.Common.Validators;
using Application.Posts.Commands.UpdatePost;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandValidatorTests : PostsTestBase
    {
        private UpdatePostCommandValidator GetNewValidator()
        {
            return new UpdatePostCommandValidator(new IdRangeValidator(CommonLocalizer)
                , PostLocalizer);
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.PostId
                , new UpdatePostCommand {PostId = 1});

            validator.ShouldNotHaveValidationErrorFor(v => v.Text
                , new UpdatePostCommand {Text = Guid.NewGuid().ToString()});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.PostId
                , new UpdatePostCommand {PostId = 0});

            validator.ShouldHaveValidationErrorFor(v => v.PostId
                , new UpdatePostCommand {PostId = -1});

            validator.ShouldHaveValidationErrorFor(v => v.Text
                , new UpdatePostCommand {Text = "  "});

            validator.ShouldHaveValidationErrorFor(v => v.Text
                , new UpdatePostCommand {Text = null});
        }
    }
}