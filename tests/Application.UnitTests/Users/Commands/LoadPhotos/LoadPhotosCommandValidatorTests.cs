using System;
using System.Collections.Generic;
using Application.Common.Validators;
using Application.Users.Commands.LoadPhotos;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Application.UnitTests.Users.Commands.LoadPhotos
{
    public class LoadPhotosCommandValidatorTests : UsersTestBase
    {
        private LoadPhotosCommandValidator GetNewValidator()
        {
            return new LoadPhotosCommandValidator(new PhotoValidator(PhotoSettingsOptions
                    , CommonLocalizer)
                , UserLocalizer);
        }

        [Test]
        public void IsValid_ShouldBeTrue()
        {
            var command = new LoadPhotosCommand
            {
                Photos = CreateDefaultPhotoFormFiles()
            };

            var validator = GetNewValidator();

            var result = validator.Validate(command);

            Assert.That(result.IsValid);
        }

        [Test]
        public void IsValid_GivenEmptyPhotos_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            var result = validator.TestValidate(new LoadPhotosCommand {Photos = null});
            result.ShouldHaveValidationErrorFor(v => v.Photos);

            result = validator.TestValidate(new LoadPhotosCommand {Photos = new List<IFormFile>()});
            result.ShouldHaveValidationErrorFor(v => v.Photos);
        }

        [Test]
        public void IsValid_GivenInvalidPhotos_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            var photos = CreateDefaultPhotoFormFiles();
            var command = new LoadPhotosCommand {Photos = photos};

            var longString = Guid.NewGuid().ToString();
            for (var i = 0; i < 10; i++)
            {
                longString += Guid.NewGuid().ToString();
            }

            photos[0].ContentType.Returns("application/pdf");
            photos[1].Length.Returns(20000000);
            photos[2].FileName.Returns(longString);

            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Photos[0]");
            result.ShouldHaveValidationErrorFor("Photos[1]");
            result.ShouldHaveValidationErrorFor("Photos[2]");
        }
    }
}