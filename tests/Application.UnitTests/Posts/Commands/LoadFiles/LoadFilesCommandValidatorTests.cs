using System;
using System.Collections.Generic;
using Application.Common.Validators;
using Application.Posts.Commands.LoadFiles;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.LoadFiles
{
    public class LoadFilesCommandValidatorTests : PostsTestBase
    {
        private LoadFilesCommandValidator GetNewValidator()
        {
            return new LoadFilesCommandValidator(new FileValidator(FileSettings
                    , CommonLocalizer)
                , PostLocalizer
                , new IdRangeValidator(CommonLocalizer));
        }

        [Test]
        public void IsValid_ShouldNotHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldNotHaveValidationErrorFor(v => v.Files
                , new LoadFilesCommand {Files = CreateDefaultFormFiles()});

            validator.ShouldNotHaveValidationErrorFor(v => v.PostId
                , new LoadFilesCommand {PostId = 1});
        }

        [Test]
        public void IsValid_GivenEmptyPhotos_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            validator.ShouldHaveValidationErrorFor(v => v.Files
                , new LoadFilesCommand {Files = null});

            validator.ShouldHaveValidationErrorFor(v => v.Files
                , new LoadFilesCommand {Files = new List<IFormFile>()});
        }

        [Test]
        public void IsValid_GivenInvalidPhotos_ShouldHaveValidationError()
        {
            var validator = GetNewValidator();

            var files = CreateDefaultFormFiles();
            var command = new LoadFilesCommand {Files = files};

            var longString = Guid.NewGuid().ToString();
            for (var i = 0; i < 10; i++)
            {
                longString += Guid.NewGuid().ToString();
            }

            files[0].Length.Returns(20000000);
            files[1].FileName.Returns(longString);

            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Files[0]");
            result.ShouldHaveValidationErrorFor("Files[1]");
        }
    }
}