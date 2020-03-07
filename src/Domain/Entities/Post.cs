using System;
using Domain.Common.Interfaces;

namespace Domain.Entities
{
    public class Post : IModifiableEntity, ILoadableEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }

        public string Text { get; set; }


        public DateTime? LastModifiedUtc { get; set; }

        public DateTime LoadedUtc { get; set; }
    }
}