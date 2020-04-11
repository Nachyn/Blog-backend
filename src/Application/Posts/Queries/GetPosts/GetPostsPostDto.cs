using System;
using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Posts.Queries.GetPosts
{
    public class GetPostsPostDto : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime? LastModifiedUtc { get; set; }

        public DateTime LoadedUtc { get; set; }

        public List<GetPostsPostFileDto> Files { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Post, GetPostsPostDto>()
                .ForMember(d => d.Files, opt => opt.Ignore());
        }
    }
}