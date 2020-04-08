using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Users.Queries.FindUser
{
    public class FindUserUserInfoDto : IMapFrom<AppUser>
    {
        public int Id { get; set; }

        public string UserName { get; set; }
    }
}