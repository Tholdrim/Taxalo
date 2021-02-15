using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Taxalo.ExchangeRates.Extensions;
using Taxalo.ExchangeRates.Interfaces;
using Taxalo.ExchangeRates.Providers;
using Taxalo.ExchangeRates.Services;

namespace Taxalo.ExchangeRates
{
    internal class Program
    {
        private const string Arguments = nameof(Arguments);

        public static Task Main(string[] arguments)
        {
            var builder = new HostBuilder();

            builder.Properties.Add(Arguments, arguments);

            builder.ConfigureHostConfiguration(ConfigureHostConfiguration);
            builder.ConfigureAppConfiguration(ConfigureApplicationConfiguration);
            builder.ConfigureLogging(ConfigureLogging);
            builder.UseDefaultServiceProvider(ConfigureServiceProvider);
            builder.ConfigureServices(ConfigureServices);

            return builder.RunConsoleAsync();
        }

        private static void ConfigureApplicationConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            var arguments = context.Properties[Arguments] as string[];

            builder.AddCommandLine(arguments);
        }

        private static void ConfigureHostConfiguration(IConfigurationBuilder builder)
        {
            builder.AddEnvironmentVariables(prefix: "DOTNET_");
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.AddDebug();
            builder.AddConsoleWithColors();

            builder.SetMinimumLevel(LogLevel.Warning);
            builder.AddFilter(nameof(Taxalo), LogLevel.Information);
        }

        private static void ConfigureServiceProvider(HostBuilderContext context, ServiceProviderOptions options)
        {
            var isDevelopment = context.HostingEnvironment.IsDevelopment();

            options.ValidateOnBuild = isDevelopment;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHostedService<ConsoleHostedService>();

            services.AddTransient<INbpExchangeRateService, NbpExchangeRateService>();
            services.AddTransient<IParametersProvider, ParametersProvider>();
        }
    }
}
