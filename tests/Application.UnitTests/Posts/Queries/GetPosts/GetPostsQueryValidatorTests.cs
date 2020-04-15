using Application.Common.Validators;
using Application.Posts.Queries.GetPosts;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Queries.GetPosts
{
    public class GetPostsQueryValidatorTests : PostsTestBase
    {
        private GetPostsQueryValidator GetNewValidator()
        {
            return new GetPostsQueryValidator(new IdRangeValidator(CommonLocalizer)
                , new PaginationValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.NumberPage
                , new GetPostsQuery {NumberPage = 1});

            validator.ShouldNotHaveValidationErrorFor(v => v.PageSize
                , new GetPostsQuery {PageSize = 1});

            validator.ShouldNotHaveValidationErrorFor(v => v.UserId
                , new GetPostsQuery {UserId = 1});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.NumberPage
                , new GetPostsQuery {NumberPage = 0});

            validator.ShouldHaveValidationErrorFor(v => v.NumberPage
                , new GetPostsQuery {NumberPage = -1});

            validator.ShouldHaveValidationErrorFor(v => v.PageSize
                , new GetPostsQuery {PageSize = 0});

            validator.ShouldHaveValidationErrorFor(v => v.PageSize
                , new GetPostsQuery {PageSize = -1});

            validator.ShouldHaveValidationErrorFor(v => v.UserId
                , new GetPostsQuery {UserId = 0});

            validator.ShouldHaveValidationErrorFor(v => v.UserId
                , new GetPostsQuery {UserId = -1});
        }
    }
}