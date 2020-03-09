using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PostFileConfiguration : IEntityTypeConfiguration<PostFile>
    {
        public void Configure(EntityTypeBuilder<PostFile> builder)
        {
            builder.ToTable("PostFiles");

            builder.Property(f => f.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(f => f.Path)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasAlternateKey(f => f.Path);
        }
    }
}