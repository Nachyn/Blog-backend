using System.Collections.Generic;
using Application.Common.Dtos;

namespace Application.Users.Queries.GetPhotos
{
    public class GetPhotosResponseDto : PaginationResponseDto
    {
        public List<GetPhotosUserPhotoDto> Photos { get; set; }
    }
}