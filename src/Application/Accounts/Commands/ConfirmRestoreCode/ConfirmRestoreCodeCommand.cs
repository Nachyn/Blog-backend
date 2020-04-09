using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Accounts.Commands.ConfirmRestoreCode
{
    public class ConfirmRestoreCodeCommand : IRequest<ConfirmRestoreCodeResponseDto>
    {
        public string Email { get; set; }

        public string Code { get; set; }

        public string Password { get; set; }

        public class ConfirmRestoreCodeCommandHandler
            : IRequestHandler<ConfirmRestoreCodeCommand, ConfirmRestoreCodeResponseDto>
        {
            private readonly IStringLocalizer<AccountsResource> _accountLocalizer;

            private readonly UserManager<AppUser> _userManager;

            public ConfirmRestoreCodeCommandHandler(UserManager<AppUser> userManager
                , IStringLocalizer<AccountsResource> accountLocalizer)
            {
                _userManager = userManager;
                _accountLocalizer = accountLocalizer;
            }

            public async Task<ConfirmRestoreCodeResponseDto> Handle(
                ConfirmRestoreCodeCommand request
                , CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new ValidationException(_accountLocalizer["UserNull"]);
                }

                var resultResetPassword = await _userManager.ResetPasswordAsync(user
                    , request.Code
                    , request.Password);

                if (!resultResetPassword.Succeeded)
                {
                    throw new ValidationException(resultResetPassword);
                }

                return new ConfirmRestoreCodeResponseDto
                {
                    Info = _accountLocalizer["ConfirmCodeSuccessfully"]
                };
            }
        }
    }
}