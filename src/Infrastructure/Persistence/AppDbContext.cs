using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
        , IAppDbContext
    {
        private readonly IDateTime _dateTime;

        public AppDbContext(DbContextOptions options
            , IDateTime dateTime)
            : base(options)
        {
            _dateTime = dateTime;
        }

        public DbSet<AppUserPhoto> UserPhotos { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostFile> PostFiles { get; set; }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.AddRoles();
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker?.Entries();

            if (entries == null)
            {
                return;
            }

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added
                        when entry.Entity is ILoadableEntity loadableEntity:
                        loadableEntity.LoadedUtc = _dateTime.NowUtc;
                        break;
                    case EntityState.Modified
                        when entry.Entity is IModifiableEntity modifiableEntity:
                        modifiableEntity.LastModifiedUtc = _dateTime.NowUtc;
                        break;
                }

                var propertyValues =
                    entry.CurrentValues.Properties.Where(p =>
                        p.ClrType == typeof(string));

                foreach (var property in propertyValues)
                {
                    if (entry.CurrentValues[property.Name] != null)
                    {
                        entry.CurrentValues[property.Name] =
                            entry.CurrentValues[property.Name].ToString().Trim();
                    }
                }
            }
        }
    }
}