using System.Collections.Generic;

namespace Application.Users.Commands.LoadPhotos
{
    public class LoadPhotosResponseDto
    {
        public List<LoadPhotosUserPhotoDto> Photos { get; set; }
    }
}