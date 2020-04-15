using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Posts.Queries.GetPosts;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Queries.GetPosts
{
    public class GetPostsQueryTests : PostsTestBase
    {
        private GetPostsQuery.GetPostsQueryHandler GetNewHandler()
        {
            return new GetPostsQuery.GetPostsQueryHandler(Mapper
                , Context
                , PostLocalizer);
        }

        [Test]
        public async Task Handle_ShouldBeReturnPosts()
        {
            var query = new GetPostsQuery
            {
                NumberPage = 1,
                PageSize = 1,
                Sort = GetPostsPostSort.LoadedAsc,
                UserId = DefaultUserId
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(query.NumberPage, result.CurrentPage);
            Assert.AreEqual(1, result.CountAllPages);
            Assert.That(result.Posts.Count == 1);
            var post = result.Posts.First();
            Assert.That(post.Id > 0);
            Assert.That(post.Files.Count == DefaultFileIds.Count);
            Assert.That(DateTimeService.NowUtc > post.LastModifiedUtc
                        || post.LastModifiedUtc == null);
            Assert.That(DateTimeService.NowUtc > post.LoadedUtc);
            Assert.That(post.Text, IsNotNullOrEmpty);
            post.Files.ForEach(f =>
            {
                Assert.That(f.Id > 0);
                Assert.That(f.Name, IsNotNullOrEmpty);
                Assert.That(DateTimeService.NowUtc > f.LoadedUtc);
            });
        }

        [Test]
        public void Handle_GivenInvalidUserId_ThrowsException()
        {
            var query = new GetPostsQuery
            {
                NumberPage = 1,
                PageSize = 1,
                Sort = GetPostsPostSort.LoadedAsc,
                UserId = 2020
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(query, CancellationToken.None));
        }
    }
}