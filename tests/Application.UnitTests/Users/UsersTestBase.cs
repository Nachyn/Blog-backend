using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using Application.Users;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Application.UnitTests.Users
{
    public class UsersTestBase : CommonTestBase
    {
        protected List<int> DefaultPhotoIds;

        protected IFileService FileService;

        protected IOptions<PhotosDirectory> PhotosDirectoryOptions;

        protected IOptions<PhotoSettings> PhotoSettingsOptions;

        protected IOptions<RootFileFolderDirectory> RootDirectoryOptions;

        protected IStringLocalizer<UsersResource> UserLocalizer;

        public UsersTestBase()
        {
            UserLocalizer = TestHelpers.MockLocalizer<UsersResource>();
            FileService = Substitute.For<IFileService>();

            PhotosDirectoryOptions = Options.Create(Configuration
                .GetSection(nameof(PhotosDirectory))
                .Get<PhotosDirectory>());

            PhotoSettingsOptions = Options.Create(Configuration
                .GetSection(nameof(PhotoSettings))
                .Get<PhotoSettings>());

            RootDirectoryOptions = Options.Create(new RootFileFolderDirectory
            {
                RootFileFolder = "rootFiles"
            });
        }

        protected List<IFormFile> CreateDefaultPhotoFormFiles()
        {
            var formFiles = new List<IFormFile>();
            for (var i = 0; i < 5; i++)
            {
                var formFile = Substitute.For<IFormFile>();
                formFile.FileName.Returns($"{Guid.NewGuid():N}.png");
                formFile.ContentType.Returns("image/png");
                formFile.Length.Returns(5000000);
                formFiles.Add(formFile);
            }

            return formFiles;
        }

        public override async Task InitializeDatabase()
        {
            await base.InitializeDatabase();
            SeedDefaultData();
        }

        private void SeedDefaultData()
        {
            var defaultPhotos = new List<AppUserPhoto>();
            for (var i = 0; i < 5; i++)
            {
                defaultPhotos.Add(new AppUserPhoto
                {
                    Name = Guid.NewGuid().ToString(),
                    Path = Guid.NewGuid().ToString(),
                    UserId = DefaultUserId
                });
            }

            Context.UserPhotos.AddRange(defaultPhotos);
            Context.SaveChanges();

            DefaultPhotoIds = defaultPhotos.Select(f => f.Id).ToList();
            DetachAllEntities();
        }
    }
}