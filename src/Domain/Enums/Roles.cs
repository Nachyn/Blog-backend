using System.ComponentModel;

namespace Domain.Enums
{
    public enum Roles
    {
        [Description("admin")] 
        Admin = 1,

        [Description("user")] 
        User = 2
    }
}