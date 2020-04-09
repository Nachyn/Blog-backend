using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasOne(u => u.AvatarPhoto)
                .WithMany(p => p.AvatarOwners)
                .HasForeignKey(u => u.AvatarPhotoId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}