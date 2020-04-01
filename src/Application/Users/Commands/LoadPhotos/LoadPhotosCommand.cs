using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.LoadPhotos
{
    public class LoadPhotosCommand : IRequest<LoadPhotosResponseDto>
    {
        public List<IFormFile> Photos { get; set; }

        public class LoadPhotosCommandHandler
            : IRequestHandler<LoadPhotosCommand, LoadPhotosResponseDto>
        {
            private readonly IDateTime _dateTime;

            private readonly IFileService _fileService;

            private readonly IUserAccessor _userAccessor;

            private readonly IAppDbContext _context;

            private readonly IMapper _mapper;

            private readonly RootFileFolderDirectory _rootDirectory;

            private readonly PhotosDirectory _photosDirectory;

            public LoadPhotosCommandHandler(IDateTime dateTime
                , IOptions<RootFileFolderDirectory> rootDirectory
                , IOptions<PhotosDirectory> photosDirectory
                , IFileService fileService
                , IUserAccessor userAccessor
                , IAppDbContext context
                , IMapper mapper)
            {
                _dateTime = dateTime;
                _fileService = fileService;
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;
                _rootDirectory = rootDirectory.Value;
                _photosDirectory = photosDirectory.Value;
            }

            public async Task<LoadPhotosResponseDto> Handle(LoadPhotosCommand request
                , CancellationToken cancellationToken)
            {
                var newPhotos = new List<AppUserPhoto>();
                foreach (var uploadedPhoto in request.Photos)
                {
                    var photoPath =
                        $"{_dateTime.NowUtc:yyyyMMddHHmmss}_{Guid.NewGuid():N}_{uploadedPhoto.FileName}";

                    var fullPath = Path.Combine(_rootDirectory.RootFileFolder,
                        _photosDirectory.Users, photoPath);

                    await _fileService.WriteToStorageAsync(uploadedPhoto, fullPath);

                    newPhotos.Add(new AppUserPhoto
                    {
                        UserId = _userAccessor.UserId,
                        Name = uploadedPhoto.FileName,
                        Path = photoPath
                    });
                }

                _context.UserPhotos.AddRange(newPhotos);
                await _context.SaveChangesAsync(cancellationToken);

                return new LoadPhotosResponseDto
                {
                    Photos = _mapper.Map<List<LoadPhotosUserPhotoDto>>(newPhotos)
                };
            }
        }
    }
}