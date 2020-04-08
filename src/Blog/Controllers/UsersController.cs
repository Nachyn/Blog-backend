﻿using System.Threading.Tasks;
using Application.Users.Commands.DeletePhotos;
using Application.Users.Commands.LoadPhotos;
using Application.Users.Commands.UpdateInfo;
using Application.Users.Queries.FindUser;
using Application.Users.Queries.GetPhotos;
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
    }
}