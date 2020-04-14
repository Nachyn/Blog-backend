using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Users.Commands.UpdateInfo;
using Domain.Entities;
using NUnit.Framework;

namespace Application.UnitTests.Users.Commands.UpdateInfo
{
    public class UpdateInfoCommandTests : UsersTestBase
    {
        private UpdateInfoCommand.UpdateInfoCommandHandler GetNewHandler()
        {
            return new UpdateInfoCommand.UpdateInfoCommandHandler(Context
                , UserManager
                , UserAccessor
                , UserLocalizer
                , Mapper);
        }

        [Test]
        public async Task Handle_ShouldBeUpdateInfo()
        {
            var command = new UpdateInfoCommand
            {
                AvatarPhotoId = DefaultPhotoIds.First(),
                UserName = "adminChanged"
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(command.AvatarPhotoId, result.AvatarPhotoId);
            Assert.AreEqual(command.UserName, result.UserName);
        }

        [Test]
        public async Task Handle_GivenExistingUsername_ThrowsException()
        {
            await UserManager.CreateAsync(new AppUser
            {
                UserName = "existingName",
                Email = "existingName@mail.ru"
            }, "pass12345");

            var command = new UpdateInfoCommand
            {
                AvatarPhotoId = null,
                UserName = "existingName"
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}