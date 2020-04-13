using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Accounts.Commands.ConfirmRestoreCode;
using Application.Common.Exceptions;
using NUnit.Framework;

namespace Application.UnitTests.Accounts.Commands.ConfirmRestoreCode
{
    public class ConfirmRestoreCodeCommandTests : AccountsTestBase
    {
        private ConfirmRestoreCodeCommand.ConfirmRestoreCodeCommandHandler GetNewHandler()
        {
            return new ConfirmRestoreCodeCommand.ConfirmRestoreCodeCommandHandler(UserManager
                , AccountLocalizer);
        }

        [Test]
        public async Task Handle_ShouldGetSuccessInfo()
        {
            var user = Context.Users.First();
            var code = await UserManager.GeneratePasswordResetTokenAsync(user);

            var command = new ConfirmRestoreCodeCommand
            {
                Email = user.Email,
                Code = code,
                Password = "newPass123"
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Info, IsNotNullOrEmpty);
        }

        [Test]
        public void Handle_GivenNonexistentEmail_ThrowsException()
        {
            var command = new ConfirmRestoreCodeCommand
            {
                Email = "NonexistentEmail@email.org",
                Code = "12345678",
                Password = "newPass123"
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_GivenInvalidCodeAndPassword_ThrowsException()
        {
            var command = new ConfirmRestoreCodeCommand
            {
                Email = DefaultUserEmail,
                Code = Guid.NewGuid().ToString(),
                Password = string.Empty
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}