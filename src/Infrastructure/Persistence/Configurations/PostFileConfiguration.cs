using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PostFileConfiguration : IEntityTypeConfiguration<PostFile>
    {
        public void Configure(EntityTypeBuilder<PostFile> builder)
        {
            builder.Property(f => f.Name).HasMaxLength(250);
            builder.Property(f => f.Path).HasMaxLength(250);

            builder.HasAlternateKey(f => f.Path);
        }
    }
}