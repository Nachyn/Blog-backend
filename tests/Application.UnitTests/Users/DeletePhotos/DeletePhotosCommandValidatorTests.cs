using System.Collections.Generic;
using Application.Common.Validators;
using Application.Users.Commands.DeletePhotos;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Users.DeletePhotos
{
    public class DeletePhotosCommandValidatorTests : UsersTestBase
    {
        private DeletePhotosCommandValidator GetNewValidator()
        {
            return new DeletePhotosCommandValidator(new IdsCountValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldBeTrue()
        {
            var command = new DeletePhotosCommand
            {
                Ids = new List<int> {1, 2, 3, 4, 5}
            };

            var validator = GetNewValidator();

            var result = validator.Validate(command);

            Assert.That(result.IsValid);
        }

        [Test]
        public void IsValid_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Ids
                , new DeletePhotosCommand {Ids = new List<int>()});

            validator.ShouldHaveValidationErrorFor(v => v.Ids
                , new DeletePhotosCommand {Ids = null});
        }
    }
}