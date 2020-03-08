using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public static class AppDbContextSeed
    {
        public static async Task InitializeAsync(UserManager<AppUser> userManager,
            RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration,
            AppDbContext context)
        {
            await context.Database.MigrateAsync();

            //For .OrderBy(i => Guid.NewGuid())
            await context.Database.ExecuteSqlRawAsync(
                "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");

            var adminAccount =
                configuration.GetSection(nameof(AdminAccount)).Get<AdminAccount>();

            if (adminAccount == null)
            {
                throw new NullReferenceException(nameof(adminAccount));
            }

            var adminEmail = adminAccount.Email;
            var password = adminAccount.Password;

            var adminRoleName = Roles.Admin.GetEnumDescription();
            var userRoleName = Roles.User.GetEnumDescription();

            if (await roleManager.FindByNameAsync(adminRoleName) != null
                && await roleManager.FindByNameAsync(userRoleName) != null
                && !context.Users.Any())
            {
                var admin = new AppUser {Email = adminEmail, UserName = adminRoleName};

                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRoleName);
                    await userManager.AddToRoleAsync(admin, userRoleName);
                }
            }
        }
    }
}