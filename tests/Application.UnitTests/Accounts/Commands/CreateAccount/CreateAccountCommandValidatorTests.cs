using Application.Accounts.Commands.CreateAccount;
using Application.Common.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidatorTests : AccountsTestBase
    {
        private CreateAccountCommandValidator GetNewValidator()
        {
            return new CreateAccountCommandValidator(new UserNameValidator(CommonLocalizer
                    , AppUserSettingOptions)
                , new EmailValidator(AppUserSettingOptions
                    , CommonLocalizer)
                , AccountLocalizer);
        }

        [Test]
        public void IsValid_ShouldBeTrue()
        {
            var command = new CreateAccountCommand
            {
                Email = DefaultUserEmail,
                Password = DefaultUserPassword,
                Username = "admin"
            };

            var validator = GetNewValidator();

            var result = validator.Validate(command);

            Assert.That(result.IsValid);
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Email
                , new CreateAccountCommand {Email = string.Empty});

            validator.ShouldHaveValidationErrorFor(v => v.Email
                , new CreateAccountCommand {Email = "usa@mlg"});

            validator.ShouldHaveValidationErrorFor(v => v.Password
                , new CreateAccountCommand {Password = string.Empty});

            validator.ShouldHaveValidationErrorFor(v => v.Username
                , new CreateAccountCommand {Username = string.Empty});

            validator.ShouldHaveValidationErrorFor(v => v.Username
                , new CreateAccountCommand {Username = "AAAAAAAAAAAAAAAAAAAAA"});

            validator.ShouldHaveValidationErrorFor(v => v.Username
                , new CreateAccountCommand {Username = "A"});
        }
    }
}