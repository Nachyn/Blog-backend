using Application.Posts.Commands.CreatePost;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidatorTests : PostsTestBase
    {
        private CreatePostCommandValidator GetNewValidator()
        {
            return new CreatePostCommandValidator(PostLocalizer);
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.Text
                , new CreatePostCommand {Text = "simple text"});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Text
                , new CreatePostCommand {Text = "  "});

            validator.ShouldHaveValidationErrorFor(v => v.Text
                , new CreatePostCommand {Text = string.Empty});

            validator.ShouldHaveValidationErrorFor(v => v.Text
                , new CreatePostCommand {Text = null});
        }
    }
}