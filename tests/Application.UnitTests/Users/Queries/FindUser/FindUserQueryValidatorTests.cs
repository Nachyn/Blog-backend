using Application.Users.Queries.FindUser;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Users.Queries.FindUser
{
    public class FindUserQueryValidatorTests : UsersTestBase
    {
        private FindUserQueryValidator GetNewValidator()
        {
            return new FindUserQueryValidator(UserLocalizer);
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.UserName
                , new FindUserQuery {UserName = "UserName789"});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.UserName
                , new FindUserQuery {UserName = string.Empty});

            validator.ShouldHaveValidationErrorFor(v => v.UserName
                , new FindUserQuery {UserName = null});

            validator.ShouldHaveValidationErrorFor(v => v.UserName
                , new FindUserQuery {UserName = "   "});
        }
    }
}