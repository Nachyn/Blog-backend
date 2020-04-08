using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Users.Commands.UpdateInfo
{
    public class UpdateInfoCommand : IRequest<UpdateInfoResponseDto>
    {
        public int? AvatarPhotoId { get; set; }

        public string UserName { get; set; }

        public class UpdateInfoCommandHandler
            : IRequestHandler<UpdateInfoCommand, UpdateInfoResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly IUserAccessor _userAccessor;

            private readonly IStringLocalizer<UsersResource> _userLocalizer;

            private readonly UserManager<AppUser> _userManager;

            public UpdateInfoCommandHandler(IAppDbContext context
                , UserManager<AppUser> userManager
                , IUserAccessor userAccessor
                , IStringLocalizer<UsersResource> userLocalizer
                , IMapper mapper)
            {
                _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
                _userLocalizer = userLocalizer;
                _mapper = mapper;
            }

            public async Task<UpdateInfoResponseDto> Handle(UpdateInfoCommand request
                , CancellationToken cancellationToken)
            {
                if (request.AvatarPhotoId.HasValue
                    && !await _context.UserPhotos.AnyAsync(p => p.Id == request.AvatarPhotoId
                        , cancellationToken))
                {
                    throw new ValidationException(_userLocalizer["PhotoNotFound"]);
                }

                var user = await _context.Users.FirstAsync(u => u.Id == _userAccessor.UserId
                    , cancellationToken);
                user.AvatarPhotoId = request.AvatarPhotoId;

                var userNameResult = await _userManager.SetUserNameAsync(user, request.UserName);
                if (!userNameResult.Succeeded)
                {
                    throw new ValidationException(userNameResult);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<UpdateInfoResponseDto>(user);
            }
        }
    }
}