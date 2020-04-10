using System.IO;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using Application.Users.Commands.DeletePhotos;
using Application.Users.Commands.LoadPhotos;
using Application.Users.Commands.UpdateInfo;
using Application.Users.Queries.DownloadPhoto;
using Application.Users.Queries.FindUser;
using Application.Users.Queries.GetPhotos;
using Application.Users.Queries.GetUserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Controllers
{
    [Route("api/users")]
    public class UsersController : ApiController
    {
        private readonly IFileService _fileService;

        private readonly PhotosDirectory _photosDirectory;

        public UsersController(IFileService fileService
            , IOptions<PhotosDirectory> photosDirectory)
        {
            _fileService = fileService;
            _photosDirectory = photosDirectory.Value;
        }

        [Authorize]
        [HttpPost("photos")]
        public async Task<ActionResult<LoadPhotosResponseDto>> LoadPhotos(
            [FromForm] LoadPhotosCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("{userId}/photos")]
        public async Task<ActionResult<GetPhotosResponseDto>> GetPhotos(
            [FromQuery] GetPhotosQuery query)
        {
            return await Mediator.Send(query);
        }

        [Authorize]
        [HttpDelete("photos")]
        public async Task<ActionResult<DeletePhotosResponseDto>> DeletePhotos(
            [FromBody] DeletePhotosCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("photos/{photoId}")]
        public async Task<ActionResult> DownloadPhoto(
            [FromRoute] DownloadPhotoQuery query)
        {
            var photo = await Mediator.Send(query);

            return _fileService.GetFileFromStorage(
                Path.Combine(_photosDirectory.Users, photo.Path), photo.Name);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UpdateInfoResponseDto>> UpdateInfo(
            [FromBody] UpdateInfoCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<FindUserResponseDto>> FindUser(
            [FromQuery] FindUserQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserInfoResponseDto>> GetUserInfo(
            [FromRoute] GetUserInfoQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}