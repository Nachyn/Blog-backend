using Application.Accounts.Commands.ConfirmRestoreCode;
using Application.Common.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Accounts.Commands.ConfirmRestoreCode
{
    public class ConfirmRestoreCodeCommandValidatorTests : AccountsTestBase
    {
        private ConfirmRestoreCodeCommandValidator GetNewValidator()
        {
            return new ConfirmRestoreCodeCommandValidator(new EmailValidator(AppUserSettingOptions
                    , CommonLocalizer)
                , AccountLocalizer);
        }

        [Test]
        public void IsValid_ShouldBeTrue()
        {
            var command = new ConfirmRestoreCodeCommand
            {
                Code = "177602",
                Email = DefaultUserEmail,
                Password = DefaultUserPassword
            };

            var validator = GetNewValidator();

            var result = validator.Validate(command);

            Assert.That(result.IsValid);
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Password
                , new ConfirmRestoreCodeCommand {Password = string.Empty});

            validator.ShouldHaveValidationErrorFor(v => v.Email
                , new ConfirmRestoreCodeCommand {Email = "admin@adil"});

            validator.ShouldHaveValidationErrorFor(v => v.Email
                , new ConfirmRestoreCodeCommand {Email = string.Empty});

            validator.ShouldHaveValidationErrorFor(v => v.Code
                , new ConfirmRestoreCodeCommand {Code = string.Empty});
        }
    }
}