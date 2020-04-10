using System.Threading.Tasks;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Commands.DeleteFiles;
using Application.Posts.Commands.LoadFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/posts")]
    public class PostsController : ApiController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreatePostResponseDto>> CreatePost(
            [FromBody] CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpPost("{postId}/files")]
        public async Task<ActionResult<LoadFilesResponseDto>> LoadFiles(
            [FromForm] LoadFilesCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpDelete("{postId}/files")]
        public async Task<ActionResult<DeleteFilesResponseDto>> DeleteFiles(
            [FromRoute] int postId, [FromBody] DeleteFilesCommand command)
        {
            command.PostId = postId;
            return await Mediator.Send(command);
        }
    }
}