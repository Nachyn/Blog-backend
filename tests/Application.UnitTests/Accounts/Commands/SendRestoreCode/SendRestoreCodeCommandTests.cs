using System.Threading;
using System.Threading.Tasks;
using Application.Accounts.Commands.SendRestoreCode;
using NUnit.Framework;

namespace Application.UnitTests.Accounts.Commands.SendRestoreCode
{
    public class SendRestoreCodeCommandTests : AccountsTestBase
    {
        private SendRestoreCodeCommand.SendRestoreCodeCommandHandler GetNewHandler()
        {
            return new SendRestoreCodeCommand.SendRestoreCodeCommandHandler(EmailService
                , UserManager
                , AccountLocalizer);
        }

        [Test]
        public async Task Handle_ShouldBeSendCode()
        {
            var command = new SendRestoreCodeCommand {Email = DefaultUserEmail};
            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Info, IsNotNullOrEmpty);

            command.Email = "nonexistentUser@mail.org";

            result = await handler.Handle(command, CancellationToken.None);

            Assert.That(result.Info, IsNotNullOrEmpty);
        }
    }
}