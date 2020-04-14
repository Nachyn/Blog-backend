using System.Threading;
using System.Threading.Tasks;
using Application.Users.Queries.FindUser;
using NUnit.Framework;

namespace Application.UnitTests.Users.Queries.FindUser
{
    public class FindUserQueryTests : UsersTestBase
    {
        private FindUserQuery.FindUserQueryHandler GetNewHandler()
        {
            return new FindUserQuery.FindUserQueryHandler(Context
                , Mapper);
        }

        [Test]
        public async Task Handle_ShouldBeFindUser()
        {
            var query = new FindUserQuery
            {
                UserName = DefaultUserUsername
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.Users, IsNotNullOrEmpty);
            result.Users.ForEach(u =>
            {
                Assert.That(u.Id > 0);
                Assert.That(u.UserName, IsNotNullOrEmpty);
            });
        }
    }
}