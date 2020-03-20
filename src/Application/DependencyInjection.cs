using System.Reflection;
using Application.Common.AppSettingHelpers.Configurations;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Services;
using AutoMapper;
using FluentValidation;
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

            //Для проверки конфига на старте, а не в момент обращения к IMapper.
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            var mapperConfiguration = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            mapperConfiguration.AssertConfigurationIsValid();
            services.AddSingleton(mapperConfiguration.CreateMapper());

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>)
                , typeof(RequestPerformanceBehaviour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>)
                , typeof(RequestValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddHttpContextAccessor();
            services.AddTransient<IUserAccessor, UserAccessor>();

            return services;
        }
    }
}