using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Users.Queries.GetUserInfo;
using NUnit.Framework;

namespace Application.UnitTests.Users.Queries.GetUserInfo
{
    public class GetUserInfoQueryTests : UsersTestBase
    {
        private GetUserInfoQuery.GetUserInfoQueryHandler GetNewHandler()
        {
            return new GetUserInfoQuery.GetUserInfoQueryHandler(Context
                , UserLocalizer
                , Mapper);
        }

        [Test]
        public async Task Handle_ShouldBeGetUserInfo()
        {
            var query = new GetUserInfoQuery
            {
                UserId = DefaultUserId
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(DefaultUserUsername, result.UserName);
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var query = new GetUserInfoQuery
            {
                UserId = 10007
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(query, CancellationToken.None));
        }
    }
}