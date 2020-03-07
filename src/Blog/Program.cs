using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Blog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var environmentName =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                "Production";

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(config.GetSection("Serilog"))
                .WriteTo.ColoredConsole(
                    LogEventLevel.Verbose,
                    "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}"
                )
                .CreateLogger();

            try
            {
                var host = CreateHostBuilderUsingConfig(args, config).Build();

                using (var scope = host.Services.CreateScope())
                {
                    //var services = scope.ServiceProvider;
                    //try
                    //{
                    //    var context = services.GetRequiredService<ApplicationDbContext>();
                    //    context.Database.Migrate();

                    //    var userManager = services
                    //        .GetRequiredService<UserManager<AppUser>>();

                    //    await ApplicationDbContextSeed.SeedAsync(userManager);
                    //}
                    //catch (Exception ex)
                    //{
                    //    var logger = services.GetRequiredService<ILogger<Program>>();
                    //    logger.LogError(ex,
                    //        "An error occurred while seeding the database.");
                    //}
                }

                host.Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        public static IHostBuilder CreateHostBuilderUsingConfig(string[] args,
            IConfiguration config)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseConfiguration(config)
                        .UseStartup<Startup>();
                });
        }
    }
}