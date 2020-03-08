using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using UserRoles = Domain.Enums.Roles;

namespace Application.Common.Attributes
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public UserAuthorizeAttribute()
        {
            Roles = UserRoles.User.GetEnumDescription();
        }
    }
}