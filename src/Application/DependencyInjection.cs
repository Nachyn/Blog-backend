using System.Reflection;
using Application.Common.AppSettingHelpers.Configurations;
using Application.Common.Behaviours;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services
            , IConfiguration configuration
            , string webHostEnvironmentContentRootPath)
        {
            services.AddAppSettingHelpers(configuration, webHostEnvironmentContentRootPath);
            services.AddLogging(configure =>
            {
                configure.AddDebug();
                configure.AddSerilog();
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>)
                , typeof(RequestPerformanceBehaviour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>)
                , typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}