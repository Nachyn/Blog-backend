using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface IUserAccessor
    {
        public ClaimsPrincipal User { get; }
    }
}