using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Posts.Commands.UpdatePost;
using NUnit.Framework;

namespace Application.UnitTests.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandTests : PostsTestBase
    {
        private UpdatePostCommand.UpdatePostCommandHandler GetNewHandler()
        {
            return new UpdatePostCommand.UpdatePostCommandHandler(Mapper
                , PostLocalizer
                , Context
                , UserAccessor);
        }

        [Test]
        public async Task Handle_ShouldBeUpdatePost()
        {
            var command = new UpdatePostCommand
            {
                PostId = DefaultPostId,
                Text = Guid.NewGuid().ToString()
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(command.Text, result.Text);
            Assert.That(DateTimeService.NowUtc > result.LastModifiedUtc);
        }

        [Test]
        public void Handle_GivenInvalidPostId_ThrowsException()
        {
            var command = new UpdatePostCommand
            {
                PostId = 1004,
                Text = Guid.NewGuid().ToString()
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}