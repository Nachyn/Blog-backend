using System.Threading;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Accounts.Commands.SendRestoreCode
{
    public class SendRestoreCodeCommand : IRequest<SendRestoreCodeResponseDto>
    {
        public string Email { get; set; }

        public class SendRestoreCodeCommandHandler
            : IRequestHandler<SendRestoreCodeCommand, SendRestoreCodeResponseDto>
        {
            private readonly IStringLocalizer<AccountsResource> _accountLocalizer;

            private readonly IEmailService _emailService;

            private readonly UserManager<AppUser> _userManager;

            public SendRestoreCodeCommandHandler(IEmailService emailService
                , UserManager<AppUser> userManager
                , IStringLocalizer<AccountsResource> accountLocalizer)
            {
                _emailService = emailService;
                _userManager = userManager;
                _accountLocalizer = accountLocalizer;
            }

            public async Task<SendRestoreCodeResponseDto> Handle(SendRestoreCodeCommand request
                , CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    await _emailService.SendEmailAsync(new EmailMessageDto
                    {
                        Email = request.Email,
                        MessageHtml = code,
                        Subject = _accountLocalizer["RestoreAccountSubject"]
                    });
                }

                return new SendRestoreCodeResponseDto
                {
                    Info = _accountLocalizer["CodeSent"]
                };
            }
        }
    }
}