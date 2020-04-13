using Application.Accounts.Commands.SendRestoreCode;
using Application.Common.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Accounts.Commands.SendRestoreCode
{
    public class SendRestoreCodeCommandValidatorTests : AccountsTestBase
    {
        private SendRestoreCodeCommandValidator GetNewValidator()
        {
            return new SendRestoreCodeCommandValidator(new EmailValidator(AppUserSettingOptions
                , CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldBeTrue()
        {
            var command = new SendRestoreCodeCommand {Email = DefaultUserEmail};

            var validator = GetNewValidator();

            var result = validator.Validate(command);

            Assert.That(result.IsValid);
        }

        [Test]
        public void IsValid_ShouldBeFalse()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Email
                , new SendRestoreCodeCommand {Email = string.Empty});

            validator.ShouldHaveValidationErrorFor(v => v.Email
                , new SendRestoreCodeCommand {Email = "e@mail"});
        }
    }
}