using System.Collections.Generic;
using Application.Common.Dtos;

namespace Application.Posts.Queries.GetPosts
{
    public class GetPostsResponseDto : PaginationResponseDto
    {
        public List<GetPostsPostDto> Posts { get; set; }
    }
}