using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Users.Commands.UpdateInfo
{
    public class UpdateInfoResponseDto : IMapFrom<AppUser>
    {
        public int? AvatarPhotoId { get; set; }

        public string UserName { get; set; }
    }
}