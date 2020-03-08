using System.IO;
using Application.Common.AppSettingHelpers.Main;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.AppSettingHelpers.Configurations
{
    public static class AppSettingHelperExtensions
    {
        public static IServiceCollection AddAppSettingHelpers(
            this IServiceCollection services
            , IConfiguration configuration
            , string hostEnvironmentContentRootPath)
        {
            services.AddOptions();

            services.Configure<AdminAccount>(
                configuration.GetSection(nameof(AdminAccount)));

            services.Configure<PasswordIdentitySettings>(
                configuration.GetSection(nameof(PasswordIdentitySettings)));

            services.Configure<RootFileFolderDirectory>(
                directory =>
                {
                    directory.RootFileFolder =
                        Path.Combine(hostEnvironmentContentRootPath, "files");
                }
            );

            return services;
        }
    }
}