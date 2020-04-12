using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Accounts.Commands.Authorize;
using Application.Common.Exceptions;
using Domain.Entities;
using NUnit.Framework;

namespace Application.UnitTests.Accounts.Commands.Authorize
{
    public class AuthorizeCommandTests : AccountsTestBase
    {
        private AuthorizeCommand.AuthorizeCommandHandler GetNewHandler()
        {
            return new AuthorizeCommand.AuthorizeCommandHandler(Mapper
                , AuthOptionsOptions
                , AccountLocalizer
                , UserManager
                , Context
                , DateTimeService);
        }

        [Test]
        public async Task Handle_MustAuthorize()
        {
            var command = new AuthorizeCommand
            {
                Email = DefaultUserEmail,
                Password = DefaultUserPassword,
                Type = Token.TypePassword
            };

            var handler = GetNewHandler();

            var result = await handler.Handle(command, CancellationToken.None);
            AssertAuthorize(command, result);

            command.Type = Token.TypeRefresh;
            command.RefreshToken = result.RefreshToken;

            result = await handler.Handle(command, CancellationToken.None);
            AssertAuthorize(command, result);
        }

        private void AssertAuthorize(AuthorizeCommand command, AuthorizeResponseDto result)
        {
            Assert.AreEqual(command.Email, result.Email);
            Assert.That(DateTimeService.NowUtc < result.ExpirationRefreshTokenUtc);
            Assert.That(DateTimeService.NowUtc < result.ExpirationTokenUtc);
            Assert.That(result.RefreshToken, IsNotNullOrEmpty);
            Assert.That(result.Token, IsNotNullOrEmpty);
            Assert.That(result.Roles, IsNotNullOrEmpty);
            Assert.AreEqual(DefaultUserId, result.UserId);
            Assert.That(result.Username, IsNotNullOrEmpty);
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new AuthorizeCommand
            {
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
                Type = Token.TypePassword
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_GivenInvalidPassword_ThrowsException()
        {
            var command = new AuthorizeCommand
            {
                Email = DefaultUserEmail,
                Password = Guid.NewGuid().ToString(),
                Type = Token.TypePassword
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_GivenInvalidType_ThrowsException()
        {
            var command = new AuthorizeCommand
            {
                Email = DefaultUserEmail,
                Password = DefaultUserPassword,
                Type = Guid.NewGuid().ToString()
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_GivenInvalidRefreshToken_ThrowsException()
        {
            var command = new AuthorizeCommand
            {
                Email = DefaultUserEmail,
                RefreshToken = Guid.NewGuid().ToString(),
                Type = Token.TypeRefresh
            };

            var handler = GetNewHandler();

            Assert.ThrowsAsync<ValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}