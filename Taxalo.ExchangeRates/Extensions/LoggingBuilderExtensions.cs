using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Taxalo.ExchangeRates.Formatters;

namespace Taxalo.ExchangeRates.Extensions
{
    internal static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddConsoleWithColors(this ILoggingBuilder builder)
        {
            return builder
                .AddConsole(options => options.FormatterName = nameof(CustomConsoleFormatter))
                .AddConsoleFormatter<CustomConsoleFormatter, ConsoleFormatterOptions>();
        }
    }
}
