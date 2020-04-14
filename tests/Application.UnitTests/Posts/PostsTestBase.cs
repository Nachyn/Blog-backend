using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using Application.Posts;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Application.UnitTests.Posts
{
    public class PostsTestBase : CommonTestBase
    {
        protected List<int> DefaultFileIds;

        protected int DefaultPostId;

        protected IFileService FileService;

        protected IStringLocalizer<PostsResource> PostLocalizer;

        protected IOptions<FilesDirectory> FilesDirectory;

        protected IOptions<FileSettings> FileSettings;

        protected IOptions<RootFileFolderDirectory> RootDirectoryOptions;

        public PostsTestBase()
        {
            PostLocalizer = TestHelpers.MockLocalizer<PostsResource>();
            FileService = Substitute.For<IFileService>();

            FilesDirectory = Options.Create(Configuration
                .GetSection(nameof(FilesDirectory))
                .Get<FilesDirectory>());

            RootDirectoryOptions = Options.Create(new RootFileFolderDirectory
            {
                RootFileFolder = "rootFiles"
            });

            FileSettings = Options.Create(Configuration
                .GetSection(nameof(FileSettings))
                .Get<FileSettings>());
        }

        public override async Task InitializeDatabase()
        {
            await base.InitializeDatabase();
            SeedDefaultData();
        }

        protected List<IFormFile> CreateDefaultFormFiles()
        {
            var formFiles = new List<IFormFile>();
            for (var i = 0; i < 5; i++)
            {
                var formFile = Substitute.For<IFormFile>();
                formFile.FileName.Returns($"{Guid.NewGuid():N}.pdf");
                formFile.ContentType.Returns("application/pdf");
                formFile.Length.Returns(7000000);
                formFiles.Add(formFile);
            }

            return formFiles;
        }

        private void SeedDefaultData()
        {
            var post = new Post
            {
                Text = Guid.NewGuid().ToString(),
                UserId = DefaultUserId
            };

            Context.Posts.Add(post);
            Context.SaveChanges();

            var defaultFiles = new List<PostFile>();
            for (var i = 0; i < 5; i++)
            {
                defaultFiles.Add(new PostFile
                {
                    Name = Guid.NewGuid().ToString(),
                    Path = Guid.NewGuid().ToString(),
                    PostId = post.Id
                });
            }

            Context.PostFiles.AddRange(defaultFiles);
            Context.SaveChanges();

            DefaultFileIds = defaultFiles.Select(f => f.Id).ToList();
            DefaultPostId = post.Id;
            DetachAllEntities();
        }
    }
}