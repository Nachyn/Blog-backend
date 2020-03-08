using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<AppUser> Users { get; set; }

        DbSet<AppUserPhoto> UserPhotos { get; set; }

        DbSet<Post> Posts { get; set; }

        DbSet<PostFile> PostFiles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}