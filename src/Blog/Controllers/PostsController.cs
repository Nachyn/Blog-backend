using System.IO;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Commands.DeleteFiles;
using Application.Posts.Commands.LoadFiles;
using Application.Posts.Commands.UpdatePost;
using Application.Posts.Queries.DownloadFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Controllers
{
    [Route("api/posts")]
    public class PostsController : ApiController
    {
        private readonly FilesDirectory _filesDirectory;

        private readonly IFileService _fileService;

        public PostsController(IFileService fileService
            , IOptions<FilesDirectory> filesDirectory)
        {
            _fileService = fileService;
            _filesDirectory = filesDirectory.Value;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreatePostResponseDto>> CreatePost(
            [FromBody] CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpPut("{postId}")]
        public async Task<ActionResult<UpdatePostResponseDto>> UpdatePost(
            [FromRoute] int postId, [FromBody] UpdatePostCommand command)
        {
            command.PostId = postId;
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

        [HttpGet("files/{fileId}")]
        public async Task<ActionResult> DownloadFile(
            [FromRoute] DownloadFileQuery query)
        {
            var file = await Mediator.Send(query);

            return _fileService.GetFileFromStorage(
                Path.Combine(_filesDirectory.Posts, file.Path), file.Name);
        }
    }
}