using Application.Common.Validators;
using Application.Users.Queries.DownloadPhoto;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Users.Queries.DownloadPhoto
{
    public class DownloadPhotoQueryValidatorTests : UsersTestBase
    {
        private DownloadPhotoQueryValidator GetNewValidator()
        {
            return new DownloadPhotoQueryValidator(new IdRangeValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.PhotoId
                , new DownloadPhotoQuery {PhotoId = 50});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.PhotoId
                , new DownloadPhotoQuery {PhotoId = 0});

            validator.ShouldHaveValidationErrorFor(v => v.PhotoId
                , new DownloadPhotoQuery {PhotoId = -1});
        }
    }
}