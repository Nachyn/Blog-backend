using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Posts.Queries.DownloadFile
{
    public class DownloadFileQuery : IRequest<DownloadFileResponseDto>
    {
        public int FileId { get; set; }

        public class DownloadFileQueryHandler
            : IRequestHandler<DownloadFileQuery, DownloadFileResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly IStringLocalizer<PostsResource> _postLocalizer;

            public DownloadFileQueryHandler(IAppDbContext context
                , IMapper mapper
                , IStringLocalizer<PostsResource> postLocalizer)
            {
                _context = context;
                _mapper = mapper;
                _postLocalizer = postLocalizer;
            }

            public async Task<DownloadFileResponseDto> Handle(DownloadFileQuery request
                , CancellationToken cancellationToken)
            {
                var foundFile = await _context.PostFiles
                    .FirstOrDefaultAsync(f => f.Id == request.FileId
                        , cancellationToken);

                if (foundFile == null)
                {
                    throw new NotFoundException(_postLocalizer["FileNotFound"]);
                }

                return _mapper.Map<DownloadFileResponseDto>(foundFile);
            }
        }
    }
}