using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Posts.Commands.LoadFiles;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.LoadFiles
{
    public class LoadFilesCommandTests : PostsTestBase
    {
        private LoadFilesCommand.LoadFilesCommandHandler GetNewHandler()
        {
            return new LoadFilesCommand.LoadFilesCommandHandler(DateTimeService
                , RootDirectoryOptions
                , FilesDirectory
                , FileService
                , UserAccessor
                , Context
                , Mapper
                , PostLocalizer);
        }

        [Test]
        public async Task Handle_ShouldBeLoadFiles()
        {
            var command = new LoadFilesCommand
            {
                PostId = DefaultPostId,
                Files = CreateDefaultFormFiles()
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Files, IsNotNullOrEmpty);
            result.Files.ForEach(f =>
            {
                Assert.That(f.Id > 0);
                Assert.That(DateTimeService.NowUtc > f.LoadedUtc);
                Assert.That(f.Name, IsNotNullOrEmpty);
            });
            Assert.That(Context.PostFiles
                .Count(f => f.PostId == command.PostId) >= command.Files.Count);
        }

        [Test]
        public void Handle_GivenInvalidPostId_ThrowsException()
        {
            var command = new LoadFilesCommand
            {
                PostId = 1009,
                Files = CreateDefaultFormFiles()
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}