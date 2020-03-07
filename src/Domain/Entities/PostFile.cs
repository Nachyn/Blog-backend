using System;
using Domain.Common.Interfaces;

namespace Domain.Entities
{
    public class PostFile : ILoadableEntity
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }


        public DateTime LoadedUtc { get; set; }
    }
}