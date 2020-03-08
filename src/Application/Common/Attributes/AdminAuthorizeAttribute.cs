using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using UserRoles = Domain.Enums.Roles;

namespace Application.Common.Attributes
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            Roles = UserRoles.Admin.GetEnumDescription();
        }
    }
}