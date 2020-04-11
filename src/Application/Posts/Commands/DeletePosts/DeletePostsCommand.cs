using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Posts.Commands.DeletePosts
{
    public class DeletePostsCommand : IRequest<DeletePostsResponseDto>
    {
        public List<int> Ids { get; set; }

        public class DeletePostsCommandHandler
            : IRequestHandler<DeletePostsCommand, DeletePostsResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly FilesDirectory _filesDirectory;

            private readonly IFileService _fileService;

            private readonly IUserAccessor _userAccessor;

            public DeletePostsCommandHandler(IAppDbContext context
                , IUserAccessor userAccessor
                , IFileService fileService
                , IOptions<FilesDirectory> filesDirectory)
            {
                _context = context;
                _userAccessor = userAccessor;
                _fileService = fileService;
                _filesDirectory = filesDirectory.Value;
            }

            public async Task<DeletePostsResponseDto> Handle(DeletePostsCommand request
                , CancellationToken cancellationToken)
            {
                var foundPosts = await _context.Posts
                    .Where(p => p.UserId == _userAccessor.UserId
                                && request.Ids.Contains(p.Id))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var foundFiles = await _context.PostFiles
                    .Where(f => foundPosts.Select(p => p.Id).Contains(f.PostId))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                _fileService.DeleteFilesFromStorage(_filesDirectory.Posts
                    , foundFiles.Select(f => f.Path).ToArray());

                _context.PostFiles.RemoveRange(foundFiles);
                _context.Posts.RemoveRange(foundPosts);
                await _context.SaveChangesAsync(cancellationToken);

                return new DeletePostsResponseDto
                {
                    Ids = foundPosts.Select(p => p.Id).ToList()
                };
            }
        }
    }
}