using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Accounts.Commands.CreateAccount;
using Application.Common.Exceptions;
using NUnit.Framework;

namespace Application.UnitTests.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandTests : AccountsTestBase
    {
        private CreateAccountCommand.CreateAccountCommandHandler GetNewHandler()
        {
            return new CreateAccountCommand.CreateAccountCommandHandler(UserManager, Mapper);
        }

        [Test]
        public async Task Handle_ShouldPersistAccount()
        {
            var command = new CreateAccountCommand
            {
                Email = "USER938@tomsk.ru",
                Password = "pass12345",
                Username = "User938"
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(command.Email.ToLower(), result.Email);
            Assert.AreEqual(command.Username, result.Username);
            Assert.That(result.Id > DefaultUserId);
            Assert.That(result.Roles, IsNotNullOrEmpty);
            Assert.That(Context.Users.Any(u => u.Id == result.Id));
        }

        [Test]
        public void Handle_GivenInvalidPassword_ThrowsException()
        {
            var command = new CreateAccountCommand
            {
                Email = "USER777@tomsk.ru",
                Password = string.Empty,
                Username = "User777"
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_GivenExistAccountData_ThrowsException()
        {
            var command = new CreateAccountCommand
            {
                Email = DefaultUserEmail,
                Password = DefaultUserPassword,
                Username = "admin"
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}