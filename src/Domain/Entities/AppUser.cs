using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public int? AvatarPhotoId { get; set; }
        public AppUserPhoto AvatarPhoto { get; set; }


        public List<AppUserPhoto> UserPhotos { get; set; }

        public List<Post> Posts { get; set; }
    }
}