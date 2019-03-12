using CronService.Base;
using CronService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CronService
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var host = new HostBuilder();
            host.ConfigureLogging(ConfigureLogging);
            host.ConfigureServices(ConfigureServices);

            if (isService)
            {
                await host.RunAsServiceAsync().ConfigureAwait(false);
            }
            else
            {
                await host.RunConsoleAsync().ConfigureAwait(false);
            }
        }

        #region Configuration Builder

        private static Configuration BuildConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();

            IConfigurationRoot configuration = configBuilder.Build();

            var settings = new Configuration();
            configuration.Bind(settings);
            return settings;
        }

        #endregion Configuration Builder

        #region Logging Builder

        private static void ConfigureLogging(HostBuilderContext _, ILoggingBuilder logging) => logging.AddConsole();

        #endregion Logging Builder

        #region Service Builder

        private static void ConfigureServices(HostBuilderContext _, IServiceCollection services)
        {
            services.AddSingleton(BuildConfiguration());
            services.AddHostedService<TestService>();
        }

        #endregion Service Builder
    }
}