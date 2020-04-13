using System;
using System.Collections;
using Application.Accounts.Commands.Authorize;
using Application.Common.Validators;
using Domain.Entities;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Accounts.Commands.Authorize
{
    public class AuthorizeCommandValidatorTests : AccountsTestBase
    {
        private AuthorizeCommandValidator GetNewValidator()
        {
            return new AuthorizeCommandValidator(AccountLocalizer
                , new EmailValidator(AppUserSettingOptions
                    , CommonLocalizer));
        }

        [TestCaseSource(typeof(AuthorizeCommandData), nameof(AuthorizeCommandData.Valid))]
        public void IsValid_ShouldBeTrue(AuthorizeCommand command)
        {
            var validator = GetNewValidator();

            var result = validator.Validate(command);

            Assert.That(result.IsValid);
        }

        [Test]
        public void IsValid_ShouldBeFalse()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Email
                , new AuthorizeCommand {Email = Guid.NewGuid().ToString()});

            validator.ShouldHaveValidationErrorFor(v => v.Type
                , new AuthorizeCommand {Type = Guid.NewGuid().ToString()});

            validator.ShouldHaveValidationErrorFor(v => v.Password
                , new AuthorizeCommand {Password = string.Empty, Type = Token.TypePassword});

            validator.ShouldHaveValidationErrorFor(v => v.RefreshToken
                , new AuthorizeCommand {RefreshToken = string.Empty, Type = Token.TypeRefresh});
        }
    }

    public static class AuthorizeCommandData
    {
        public static IEnumerable Valid
        {
            get
            {
                yield return new TestCaseData(new AuthorizeCommand
                {
                    Email = "user@mail.ru",
                    Password = "pass123",
                    Type = Token.TypePassword
                });

                yield return new TestCaseData(new AuthorizeCommand
                {
                    Email = "bob1245@gmail.com",
                    RefreshToken = Guid.NewGuid().ToString(),
                    Type = Token.TypeRefresh
                });
            }
        }
    }
}