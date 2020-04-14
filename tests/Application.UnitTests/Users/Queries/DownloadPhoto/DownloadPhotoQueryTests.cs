using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Users.Queries.DownloadPhoto;
using NUnit.Framework;

namespace Application.UnitTests.Users.Queries.DownloadPhoto
{
    public class DownloadPhotoQueryTests : UsersTestBase
    {
        private DownloadPhotoQuery.DownloadPhotoQueryHandler GetNewHandler()
        {
            return new DownloadPhotoQuery.DownloadPhotoQueryHandler(Mapper
                , Context
                , UserLocalizer);
        }

        [Test]
        public async Task Handle_ShouldBeGetPhoto()
        {
            var query = new DownloadPhotoQuery
            {
                PhotoId = DefaultPhotoIds.First()
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.Name, IsNotNullOrEmpty);
            Assert.That(result.Path, IsNotNullOrEmpty);
        }

        [Test]
        public void Handle_GivenInvalidPhotoId_ThrowsException()
        {
            var query = new DownloadPhotoQuery
            {
                PhotoId = 10005
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(query, CancellationToken.None));
        }
    }
}