using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Posts.Queries.DownloadFile;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Queries.DownloadFile
{
    public class DownloadFileQueryTests : PostsTestBase
    {
        private DownloadFileQuery.DownloadFileQueryHandler GetNewHandler()
        {
            return new DownloadFileQuery.DownloadFileQueryHandler(Context
                , Mapper
                , PostLocalizer);
        }

        [Test]
        public async Task Handle_ShouldBeReturnFile()
        {
            var query = new DownloadFileQuery
            {
                FileId = DefaultFileIds.First()
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.Name, IsNotNullOrEmpty);
            Assert.That(result.Path, IsNotNullOrEmpty);
        }

        [Test]
        public void Handle_GivenInvalidFileId_ThrowsException()
        {
            var query = new DownloadFileQuery
            {
                FileId = 1007
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(query, CancellationToken.None));
        }
    }
}