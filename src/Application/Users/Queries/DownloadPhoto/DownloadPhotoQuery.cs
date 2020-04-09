using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Users.Queries.DownloadPhoto
{
    public class DownloadPhotoQuery : IRequest<DownloadPhotoResponseDto>
    {
        public int PhotoId { get; set; }

        public class DownloadPhotoQueryHandler
            : IRequestHandler<DownloadPhotoQuery, DownloadPhotoResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly IStringLocalizer<UsersResource> _userLocalizer;

            public DownloadPhotoQueryHandler(IMapper mapper
                , IAppDbContext context
                , IStringLocalizer<UsersResource> userLocalizer)
            {
                _mapper = mapper;
                _context = context;
                _userLocalizer = userLocalizer;
            }

            public async Task<DownloadPhotoResponseDto> Handle(DownloadPhotoQuery request
                , CancellationToken cancellationToken)
            {
                var foundPhoto = await _context.UserPhotos
                    .FirstOrDefaultAsync(p => p.Id == request.PhotoId
                        , cancellationToken);

                if (foundPhoto == null)
                {
                    throw new NotFoundException(_userLocalizer["PhotoNotFound"]);
                }

                return _mapper.Map<DownloadPhotoResponseDto>(foundPhoto);
            }
        }
    }
}