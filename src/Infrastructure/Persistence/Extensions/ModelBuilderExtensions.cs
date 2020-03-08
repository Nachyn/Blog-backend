using System;
using System.Linq;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddRoles(this ModelBuilder modelBuilder)
        {
            var enumValues = Enum.GetValues(typeof(Roles)).Cast<Roles>();

            var roles = enumValues
                .Select(@enum =>
                {
                    var id = Convert.ToInt32(@enum);
                    var enumDescription = @enum.GetEnumDescription();
                    return new IdentityRole<int>
                    {
                        Id = id,
                        Name = enumDescription,
                        NormalizedName = enumDescription.ToUpper()
                    };
                })
                .ToArray();

            modelBuilder
                .Entity<IdentityRole<int>>()
                .HasData(roles);

            return modelBuilder;
        }
    }
}