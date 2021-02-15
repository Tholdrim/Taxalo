using System;
using System.IO;

namespace Taxalo.ExchangeRates.Extensions
{
    internal static class TextWriterExtensions
    {
        public static void WriteLineWithColor(this TextWriter textWriter, string message, ConsoleColor foregroundColor)
        {
            textWriter.WriteWithColor(message, foregroundColor);
            textWriter.WriteLine();
        }

        public static void WriteWithColor(this TextWriter textWriter, string message, ConsoleColor foregroundColor)
        {
            var defaultColorEscapeCode = GetForegroundColorEscapeCode(ConsoleColor.Gray);
            var foregroundColorEscapeCode = GetForegroundColorEscapeCode(foregroundColor);

            textWriter.Write($"{foregroundColorEscapeCode}{message}{defaultColorEscapeCode}");
        }

        private static string GetForegroundColorEscapeCode(ConsoleColor color) => color switch
        {
            ConsoleColor.DarkGreen   => "\x1B[32m",
            ConsoleColor.DarkRed     => "\x1B[31m",
            ConsoleColor.DarkYellow  => "\x1B[33m",
            ConsoleColor.Gray        => "\x1B[37m",
            _                        => throw new NotImplementedException()
        };
    }
}
