using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Posts.Queries.DownloadFile
{
    public class DownloadFileResponseDto : IMapFrom<PostFile>
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }
}