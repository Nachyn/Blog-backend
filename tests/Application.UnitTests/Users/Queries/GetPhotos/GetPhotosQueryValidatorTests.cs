using Application.Common.Validators;
using Application.Users.Queries.GetPhotos;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Users.Queries.GetPhotos
{
    public class GetPhotosQueryValidatorTests : UsersTestBase
    {
        private GetPhotosQueryValidator GetNewValidator()
        {
            return new GetPhotosQueryValidator(new PaginationValidator(CommonLocalizer)
                , new IdRangeValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.NumberPage
                , new GetPhotosQuery {NumberPage = 1});

            validator.ShouldNotHaveValidationErrorFor(v => v.PageSize
                , new GetPhotosQuery {PageSize = 1});

            validator.ShouldNotHaveValidationErrorFor(v => v.UserId
                , new GetPhotosQuery {UserId = 1});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.UserId
                , new GetPhotosQuery {UserId = 0});

            validator.ShouldHaveValidationErrorFor(v => v.NumberPage
                , new GetPhotosQuery {NumberPage = 0});

            validator.ShouldHaveValidationErrorFor(v => v.PageSize
                , new GetPhotosQuery {PageSize = 0});
        }
    }
}