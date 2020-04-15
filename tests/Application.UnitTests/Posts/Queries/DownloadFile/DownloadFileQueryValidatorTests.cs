using Application.Common.Validators;
using Application.Posts.Queries.DownloadFile;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Queries.DownloadFile
{
    public class DownloadFileQueryValidatorTests : PostsTestBase
    {
        private DownloadFileQueryValidator GetNewValidator()
        {
            return new DownloadFileQueryValidator(new IdRangeValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.FileId
                , new DownloadFileQuery {FileId = 1});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.FileId
                , new DownloadFileQuery {FileId = 0});

            validator.ShouldHaveValidationErrorFor(v => v.FileId
                , new DownloadFileQuery {FileId = -1});
        }
    }
}