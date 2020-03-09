using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AppUserPhotoConfiguration : IEntityTypeConfiguration<AppUserPhoto>
    {
        public void Configure(EntityTypeBuilder<AppUserPhoto> builder)
        {
            builder.ToTable("UserPhotos");

            builder.Property(p => p.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(p => p.Path)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasAlternateKey(p => p.Path);
        }
    }
}