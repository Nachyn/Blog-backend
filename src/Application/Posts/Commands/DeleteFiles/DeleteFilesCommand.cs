using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Application.Posts.Commands.DeleteFiles
{
    public class DeleteFilesCommand : IRequest<DeleteFilesResponseDto>
    {
        [JsonIgnore]
        public int PostId { get; set; }

        public List<int> Ids { get; set; }

        public class DeleteFilesCommandHandler
            : IRequestHandler<DeleteFilesCommand, DeleteFilesResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly FilesDirectory _filesDirectory;

            private readonly IFileService _fileService;

            private readonly IStringLocalizer<PostsResource> _postLocalizer;

            private readonly IUserAccessor _userAccessor;

            public DeleteFilesCommandHandler(IAppDbContext context
                , IUserAccessor userAccessor
                , IFileService fileService
                , IOptions<FilesDirectory> filesDirectory
                , IStringLocalizer<PostsResource> postLocalizer)
            {
                _context = context;
                _userAccessor = userAccessor;
                _fileService = fileService;
                _postLocalizer = postLocalizer;
                _filesDirectory = filesDirectory.Value;
            }

            public async Task<DeleteFilesResponseDto> Handle(DeleteFilesCommand request
                , CancellationToken cancellationToken)
            {
                var post = await _context.Posts
                    .FirstOrDefaultAsync(p => p.Id == request.PostId
                                              && p.UserId == _userAccessor.UserId
                        , cancellationToken);
                if (post == null)
                {
                    throw new ValidationException(_postLocalizer["PostNull"]);
                }

                var foundFiles = await _context.PostFiles
                    .Where(f => f.PostId == request.PostId
                                && request.Ids.Contains(f.Id))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                _fileService.DeleteFilesFromStorage(_filesDirectory.Posts
                    , foundFiles.Select(f => f.Path).ToArray());

                _context.PostFiles.RemoveRange(foundFiles);
                await _context.SaveChangesAsync(cancellationToken);

                return new DeleteFilesResponseDto
                {
                    Ids = foundFiles.Select(f => f.Id).ToList()
                };
            }
        }
    }
}