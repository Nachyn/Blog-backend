using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Posts.Queries.GetPosts
{
    public class GetPostsPostFileDto : IMapFrom<PostFile>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime LoadedUtc { get; set; }
    }
}