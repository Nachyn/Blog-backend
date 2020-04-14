using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Users.Commands.LoadPhotos;
using NUnit.Framework;

namespace Application.UnitTests.Users.Commands.LoadPhotos
{
    public class LoadPhotosCommandTests : UsersTestBase
    {
        private LoadPhotosCommand.LoadPhotosCommandHandler GetNewHandler()
        {
            return new LoadPhotosCommand.LoadPhotosCommandHandler(DateTimeService
                , RootDirectoryOptions
                , PhotosDirectoryOptions
                , FileService
                , UserAccessor
                , Context
                , Mapper);
        }

        [Test]
        public async Task Handle_ShouldBeLoadPhotos()
        {
            var command = new LoadPhotosCommand
            {
                Photos = CreateDefaultPhotoFormFiles()
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Photos, IsNotNullOrEmpty);
            result.Photos.ForEach(p =>
            {
                Assert.That(p.Id > 0);
                Assert.That(DateTimeService.NowUtc > p.LoadedUtc);
                Assert.That(command.Photos.Select(f => f.FileName).Contains(p.Name));
                Assert.That(Context.UserPhotos.Any(f => f.Id == p.Id));
            });
        }
    }
}