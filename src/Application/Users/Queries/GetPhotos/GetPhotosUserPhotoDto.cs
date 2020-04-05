using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Users.Queries.GetPhotos
{
    public class GetPhotosUserPhotoDto : IMapFrom<AppUserPhoto>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime LoadedUtc { get; set; }
    }
}