using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Users.Queries.GetUserInfo
{
    public class GetUserInfoResponseDto : IMapFrom<AppUser>
    {
        public string UserName { get; set; }

        public int? AvatarPhotoId { get; set; }
    }
}