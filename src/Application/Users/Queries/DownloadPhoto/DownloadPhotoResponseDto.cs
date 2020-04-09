using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Users.Queries.DownloadPhoto
{
    public class DownloadPhotoResponseDto : IMapFrom<AppUserPhoto>
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }
}