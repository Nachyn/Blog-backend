using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Users.Queries.GetPhotos;
using NUnit.Framework;

namespace Application.UnitTests.Users.Queries.GetPhotos
{
    public class GetPhotosQueryTests : UsersTestBase
    {
        private GetPhotosQuery.GetPhotosQueryHandler GetNewHandler()
        {
            return new GetPhotosQuery.GetPhotosQueryHandler(Context
                , UserLocalizer
                , Mapper);
        }

        [Test]
        public async Task Handle_ShouldBeGetPhotos()
        {
            var query = new GetPhotosQuery
            {
                NumberPage = 1,
                PageSize = DefaultPhotoIds.Count,
                Sort = GetPhotosPhotoSort.LoadedAsc,
                UserId = DefaultUserId
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(query.NumberPage, result.CurrentPage);
            Assert.AreEqual(1, result.CountAllPages);
            Assert.That(result.Photos, IsNotNullOrEmpty);
            result.Photos.ForEach(p =>
            {
                Assert.That(p.Id > 0);
                Assert.That(DateTimeService.NowUtc > p.LoadedUtc);
                Assert.That(p.Name, IsNotNullOrEmpty);
            });
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var query = new GetPhotosQuery
            {
                NumberPage = 1,
                PageSize = 2,
                Sort = GetPhotosPhotoSort.LoadedDesc,
                UserId = 10008
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(query, CancellationToken.None));
        }
    }
}