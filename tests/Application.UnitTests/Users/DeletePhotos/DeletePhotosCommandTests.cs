using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Users.Commands.DeletePhotos;
using NUnit.Framework;

namespace Application.UnitTests.Users.DeletePhotos
{
    public class DeletePhotosCommandTests : UsersTestBase
    {
        private DeletePhotosCommand.DeletePhotosCommandHandler GetNewHandler()
        {
            return new DeletePhotosCommand.DeletePhotosCommandHandler(Context
                , UserAccessor
                , FileService
                , PhotosDirectoryOptions);
        }

        [Test]
        public async Task Handle_ShouldBeDeletePhotos()
        {
            var command = new DeletePhotosCommand
            {
                Ids = DefaultPhotoIds
            };
            command.Ids.AddRange(new[] {1001, 1002, 1003});

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Ids.All(id => DefaultPhotoIds.Contains(id)));
        }
    }
}