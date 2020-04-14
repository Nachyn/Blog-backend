using Application.Common.Validators;
using Application.Users.Commands.UpdateInfo;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Users.Commands.UpdateInfo
{
    public class UpdateInfoCommandValidatorTests : UsersTestBase
    {
        private UpdateInfoCommandValidator GetNewValidator()
        {
            return new UpdateInfoCommandValidator(new IdRangeValidator(CommonLocalizer)
                , new UserNameValidator(CommonLocalizer
                    , AppUserSettingOptions));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.AvatarPhotoId
                , new UpdateInfoCommand {AvatarPhotoId = 10});

            validator.ShouldNotHaveValidationErrorFor(v => v.AvatarPhotoId
                , new UpdateInfoCommand {AvatarPhotoId = null});

            validator.ShouldNotHaveValidationErrorFor(v => v.UserName
                , new UpdateInfoCommand {UserName = "UserName"});
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.AvatarPhotoId
                , new UpdateInfoCommand {AvatarPhotoId = 0});

            validator.ShouldHaveValidationErrorFor(v => v.UserName
                , new UpdateInfoCommand {UserName = null});

            validator.ShouldHaveValidationErrorFor(v => v.UserName
                , new UpdateInfoCommand {UserName = "A"});

            validator.ShouldHaveValidationErrorFor(v => v.UserName
                , new UpdateInfoCommand {UserName = "AAAAAAAAAAAAAAAAAAAAA"});
        }
    }
}