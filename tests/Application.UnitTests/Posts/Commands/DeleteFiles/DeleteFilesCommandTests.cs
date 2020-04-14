using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Posts.Commands.DeleteFiles;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.DeleteFiles
{
    public class DeleteFilesCommandTests : PostsTestBase
    {
        private DeleteFilesCommand.DeleteFilesCommandHandler GetNewHandler()
        {
            return new DeleteFilesCommand.DeleteFilesCommandHandler(Context
                , UserAccessor
                , FileService
                , FilesDirectory
                , PostLocalizer);
        }

        [Test]
        public async Task Handle_ShouldBeDeleteFiles()
        {
            var command = new DeleteFilesCommand
            {
                PostId = DefaultPostId,
                Ids = DefaultFileIds
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Ids, IsNotNullOrEmpty);
            Assert.That(command.Ids.All(id => result.Ids.Contains(id)));
        }

        [Test]
        public void Handle_GivenInvalidPostId_ThrowsException()
        {
            var command = new DeleteFilesCommand
            {
                PostId = 10005,
                Ids = DefaultFileIds
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}