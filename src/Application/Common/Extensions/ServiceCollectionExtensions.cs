using System.Linq;
using System.Reflection;
using FluentValidation.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPropertyValidatorsFromAssembly(
            this IServiceCollection services)
        {
            var validators = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof(PropertyValidator)))
                .ToList();

            validators.ForEach(v => services.AddSingleton(v));

            return services;
        }
    }
}