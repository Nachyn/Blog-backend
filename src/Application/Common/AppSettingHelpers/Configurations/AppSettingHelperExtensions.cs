﻿using System.IO;
using Application.Common.AppSettingHelpers.Entities;
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
            , string webHostEnvironmentContentRootPath)
        {
            services.AddOptions();

            services.Configure<AdminAccount>(
                configuration.GetSection(nameof(AdminAccount)));

            services.Configure<EmailServiceSettings>(
                configuration.GetSection(nameof(EmailServiceSettings)));

            services.Configure<PasswordIdentitySettings>(
                configuration.GetSection(nameof(PasswordIdentitySettings)));

            services.Configure<RootFileFolderDirectory>(
                directory =>
                {
                    directory.RootFileFolder =
                        Path.Combine(webHostEnvironmentContentRootPath, "files");
                }
            );

            services.Configure<AuthOptions>(
                configuration.GetSection(nameof(AuthOptions)));

            services.Configure<FileSettings>(
                configuration.GetSection(nameof(FileSettings)));

            services.Configure<PhotoSettings>(
                configuration.GetSection(nameof(PhotoSettings)));

            services.Configure<PhotosDirectory>(
                configuration.GetSection(nameof(PhotosDirectory)));

            services.Configure<FilesDirectory>(
                configuration.GetSection(nameof(FilesDirectory)));

            ConfigureEntities(services, configuration);

            return services;
        }

        private static void ConfigureEntities(
            IServiceCollection services
            , IConfiguration configuration)
        {
            const string entitySection = "EntitySettings:";

            services.Configure<AppUserSettings>(
                configuration.GetSection($"{entitySection}AppUser"));
        }
    }
}