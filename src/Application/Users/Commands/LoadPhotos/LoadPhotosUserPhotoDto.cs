using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Users.Commands.LoadPhotos
{
    public class LoadPhotosUserPhotoDto : IMapFrom<AppUserPhoto>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}