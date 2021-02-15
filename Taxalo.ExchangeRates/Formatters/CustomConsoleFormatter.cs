using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using System;
using System.IO;
using System.Text;
using Taxalo.ExchangeRates.Extensions;

namespace Taxalo.ExchangeRates.Formatters
{
    internal sealed class CustomConsoleFormatter : ConsoleFormatter
    {
        public CustomConsoleFormatter()
            : base(nameof(CustomConsoleFormatter))
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
        {
            var prefix = $"[{logEntry.LogLevel}]";
            var message = logEntry.Formatter(logEntry.State, logEntry.Exception);

            if (message == null)
            {
                return;
            }

            if (Console.IsOutputRedirected)
            {
                textWriter.Write(prefix.PadRight(14));
                textWriter.WriteLine(message);
            }
            else
            {
                var prefixColor = GetPrefixColorByLogLevel(logEntry.LogLevel);
                var messageColor = GetMessageColorByLogLevel(logEntry.LogLevel);

                textWriter.WriteWithColor(prefix.PadRight(14), prefixColor);
                textWriter.WriteLineWithColor(message, messageColor);
            }
        }

        private static ConsoleColor GetMessageColorByLogLevel(LogLevel level) => level switch
        {
            LogLevel.Information => ConsoleColor.Gray,
            _                    => GetPrefixColorByLogLevel(level)
        };

        private static ConsoleColor GetPrefixColorByLogLevel(LogLevel level) => level switch
        {
            LogLevel.Information => ConsoleColor.DarkGreen,
            LogLevel.Warning     => ConsoleColor.DarkYellow,
            LogLevel.Error       => ConsoleColor.DarkRed,
            _                    => throw new NotImplementedException()
        };
    }
}
