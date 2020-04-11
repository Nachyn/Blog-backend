using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Posts.Commands.UpdatePost
{
    public class UpdatePostResponseDto : IMapFrom<Post>
    {
        public string Text { get; set; }

        public DateTime LastModifiedUtc { get; set; }
    }
}