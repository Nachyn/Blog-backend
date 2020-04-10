using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Application.Posts.Commands.LoadFiles
{
    public class LoadFilesCommand : IRequest<LoadFilesResponseDto>
    {
        [FromRoute]
        public int PostId { get; set; }

        public List<IFormFile> Files { get; set; }

        public class LoadFilesCommandHandler
            : IRequestHandler<LoadFilesCommand, LoadFilesResponseDto>
        {
            private readonly IAppDbContext _context;

            private readonly IDateTime _dateTime;

            private readonly FilesDirectory _filesDirectory;

            private readonly IFileService _fileService;

            private readonly IMapper _mapper;

            private readonly IStringLocalizer<PostsResource> _postLocalizer;

            private readonly RootFileFolderDirectory _rootDirectory;

            private readonly IUserAccessor _userAccessor;

            public LoadFilesCommandHandler(IDateTime dateTime
                , IOptions<RootFileFolderDirectory> rootDirectory
                , IOptions<FilesDirectory> filesDirectory
                , IFileService fileService
                , IUserAccessor userAccessor
                , IAppDbContext context
                , IMapper mapper
                , IStringLocalizer<PostsResource> postLocalizer)
            {
                _dateTime = dateTime;
                _rootDirectory = rootDirectory.Value;
                _filesDirectory = filesDirectory.Value;
                _fileService = fileService;
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;
                _postLocalizer = postLocalizer;
            }

            public async Task<LoadFilesResponseDto> Handle(LoadFilesCommand request
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

                var newFiles = new List<PostFile>();
                foreach (var uploadedFile in request.Files)
                {
                    var filePath =
                        $"{_dateTime.NowUtc:yyyyMMddHHmmss}_{Guid.NewGuid():N}_{uploadedFile.FileName}";

                    var fullPath = Path.Combine(_rootDirectory.RootFileFolder
                        , _filesDirectory.Posts
                        , filePath);

                    await _fileService.WriteToStorageAsync(uploadedFile, fullPath);

                    newFiles.Add(new PostFile
                    {
                        PostId = request.PostId,
                        Name = uploadedFile.FileName,
                        Path = filePath
                    });
                }

                _context.PostFiles.AddRange(newFiles);
                await _context.SaveChangesAsync(cancellationToken);

                return new LoadFilesResponseDto
                {
                    Files = _mapper.Map<List<LoadFilesPostFileDto>>(newFiles)
                };
            }
        }
    }
}