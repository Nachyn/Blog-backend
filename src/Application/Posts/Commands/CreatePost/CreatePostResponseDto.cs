using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Posts.Commands.CreatePost
{
    public class CreatePostResponseDto : IMapFrom<Post>
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        public DateTime LoadedUtc { get; set; }
    }
}