using System.Collections.Generic;

namespace Application.Users.Queries.FindUser
{
    public class FindUserResponseDto
    {
        public List<FindUserUserInfoDto> Users { get; set; }
    }
}