using System.Threading.Tasks;
using Application.Accounts.Commands.CreateAccount;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/account")]
    public class AccountsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<CreateAccountUserInfo>> CreateAccount(
            [FromBody] CreateAccountCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}