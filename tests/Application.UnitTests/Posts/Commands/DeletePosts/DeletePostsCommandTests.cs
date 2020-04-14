using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Posts.Commands.DeletePosts;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.DeletePosts
{
    public class DeletePostsCommandTests : PostsTestBase
    {
        private DeletePostsCommand.DeletePostsCommandHandler GetNewHandler()
        {
            return new DeletePostsCommand.DeletePostsCommandHandler(Context
                , UserAccessor
                , FileService
                , FilesDirectory);
        }

        [Test]
        public async Task Handle_ShouldBeDeletePostAndPostFiles()
        {
            var command = new DeletePostsCommand
            {
                Ids = new List<int> {DefaultPostId}
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Ids, IsNotNullOrEmpty);
            Assert.That(result.Ids.Contains(DefaultPostId));
            Assert.That(!Context.Posts.Any(p => p.Id == DefaultPostId));
            Assert.That(!Context.PostFiles.Any(p => DefaultFileIds.Contains(p.Id)));
        }
    }
}