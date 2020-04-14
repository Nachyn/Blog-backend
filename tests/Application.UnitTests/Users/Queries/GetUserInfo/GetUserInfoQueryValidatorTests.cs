using Application.Common.Validators;
using Application.Users.Queries.GetUserInfo;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Users.Queries.GetUserInfo
{
    public class GetUserInfoQueryValidatorTests : UsersTestBase
    {
        private GetUserInfoQueryValidator GetNewValidator()
        {
            return new GetUserInfoQueryValidator(new IdRangeValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.UserId
                , new GetUserInfoQuery {UserId = 1});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.UserId
                , new GetUserInfoQuery {UserId = 0});
        }
    }
}