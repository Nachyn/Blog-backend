using System.Threading.Tasks;
using Application.Posts.Commands.CreatePost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/posts")]
    public class PostsController : ApiController
    {
        [Authorize]
        [HttpPost]
        public async Task<CreatePostResponseDto> CreatePost(
            [FromBody] CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}