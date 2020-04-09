using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Users.Queries.GetUserInfo
{
    public class GetUserInfoQuery : IRequest<GetUserInfoResponseDto>
    {
        public int UserId { get; set; }

        public class GetUserInfoQueryHandler
            : IRequestHandler<GetUserInfoQuery, GetUserInfoResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly IStringLocalizer<UsersResource> _userLocalizer;

            public GetUserInfoQueryHandler(IAppDbContext context
                , IStringLocalizer<UsersResource> userLocalizer
                , IMapper mapper)
            {
                _context = context;
                _userLocalizer = userLocalizer;
                _mapper = mapper;
            }

            public async Task<GetUserInfoResponseDto> Handle(GetUserInfoQuery request
                , CancellationToken cancellationToken)
            {
                var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId
                    , cancellationToken);

                if (foundUser == null)
                {
                    throw new NotFoundException(_userLocalizer["UserNull"]);
                }

                return _mapper.Map<GetUserInfoResponseDto>(foundUser);
            }
        }
    }
}