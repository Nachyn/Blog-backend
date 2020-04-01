using System.Threading.Tasks;
using Application.Users.Commands.LoadPhotos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/users")]
    public class UsersController : ApiController
    {
        /// <summary>
        ///     Form key = model.Photos
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("photos")]
        public async Task<ActionResult<LoadPhotosResponseDto>> LoadPhotos(
            [FromForm(Name = "model")] LoadPhotosCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}