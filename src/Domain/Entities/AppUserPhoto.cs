using System;
using System.Collections.Generic;
using Domain.Common.Interfaces;

namespace Domain.Entities
{
    public class AppUserPhoto : ILoadableEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }


        public List<AppUser> AvatarOwners { get; set; }

        public DateTime LoadedUtc { get; set; }
    }
}