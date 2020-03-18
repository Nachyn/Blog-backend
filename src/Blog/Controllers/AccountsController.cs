using System.Threading.Tasks;
using Application.Accounts.Commands.Authorize;
using Application.Accounts.Commands.CreateAccount;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/account")]
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
    }
}