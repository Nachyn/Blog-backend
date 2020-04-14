using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Posts.Commands.CreatePost;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.CreatePost
{
    public class CreatePostCommandTests : PostsTestBase
    {
        private CreatePostCommand.CreatePostCommandHandler GetNewHandler()
        {
            return new CreatePostCommand.CreatePostCommandHandler(Mapper
                , Context
                , UserAccessor);
        }

        [Test]
        public async Task Handle_ShouldBeCreatePost()
        {
            var command = new CreatePostCommand
            {
                Text = Guid.NewGuid().ToString()
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(DefaultUserId, result.UserId);
            Assert.AreEqual(command.Text, result.Text);
            Assert.That(DateTimeService.NowUtc > result.LoadedUtc);
            Assert.That(Context.Posts.Any(p => p.Id == result.Id));
        }
    }
}