using System.Threading.Tasks;
using Application.Posts.Commands.CreatePost;
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
        public async Task<CreatePostResponseDto> CreatePost(
            [FromBody] CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpPost("{postId}/files")]
        public async Task<LoadFilesResponseDto> LoadFiles(
            [FromForm] LoadFilesCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}