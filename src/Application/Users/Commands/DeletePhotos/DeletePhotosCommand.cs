using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.DeletePhotos
{
    public class DeletePhotosCommand : IRequest<DeletePhotosResponseDto>
    {
        public List<int> Ids { get; set; }

        public class DeletePhotosCommandHandler
            : IRequestHandler<DeletePhotosCommand, DeletePhotosResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IFileService _fileService;

            private readonly PhotosDirectory _photoDirectory;

            private readonly IUserAccessor _userAccessor;

            public DeletePhotosCommandHandler(IAppDbContext context
                , IUserAccessor userAccessor
                , IFileService fileService
                , IOptions<PhotosDirectory> photoDirectory)
            {
                _context = context;
                _userAccessor = userAccessor;
                _fileService = fileService;
                _photoDirectory = photoDirectory.Value;
            }

            public async Task<DeletePhotosResponseDto> Handle(DeletePhotosCommand request
                , CancellationToken cancellationToken)
            {
                var foundPhotos = await _context.UserPhotos
                    .Where(p => p.UserId == _userAccessor.UserId
                                && request.Ids.Contains(p.Id))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                _fileService.DeleteFilesFromStorage(_photoDirectory.Users
                    , foundPhotos.Select(p => p.Path).ToArray());

                _context.UserPhotos.RemoveRange(foundPhotos);
                await _context.SaveChangesAsync(cancellationToken);

                return new DeletePhotosResponseDto
                {
                    Ids = foundPhotos.Select(p => p.Id).ToList()
                };
            }
        }
    }
}