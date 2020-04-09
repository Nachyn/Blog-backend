using System.Threading.Tasks;
using Application.Accounts.Commands.Authorize;
using Application.Accounts.Commands.ConfirmRestoreCode;
using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.SendRestoreCode;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/accounts")]
    public class AccountsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<CreateAccountUserInfoDto>> CreateAccount(
            [FromBody] CreateAccountCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("auth")]
        public async Task<ActionResult<AuthorizeResponseDto>> Authorize(
            [FromBody] AuthorizeCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("restore/sendCode")]
        public async Task<SendRestoreCodeResponseDto> SendRestoreCode(
            [FromBody] SendRestoreCodeCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("restore/confirmCode")]
        public async Task<ConfirmRestoreCodeResponseDto> ConfirmRestoreCode(
            [FromBody] ConfirmRestoreCodeCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}