using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.FindUser
{
    public class FindUserQuery : IRequest<FindUserResponseDto>
    {
        public string UserName { get; set; }

        public class FindUserQueryHandler : IRequestHandler<FindUserQuery, FindUserResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            public FindUserQueryHandler(IAppDbContext context
                , IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FindUserResponseDto> Handle(FindUserQuery request
                , CancellationToken cancellationToken)
            {
                var foundUsers = await _context.Users.Where(u => EF.Functions.Like(u.UserName
                        , $"%{request.UserName}%"))
                    .Take(5)
                    .ToListAsync(cancellationToken);

                return new FindUserResponseDto
                {
                    Users = _mapper.Map<List<FindUserUserInfoDto>>(foundUsers)
                };
            }
        }
    }
}