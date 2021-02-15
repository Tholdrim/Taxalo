using System;

namespace Taxalo.ExchangeRates.Exceptions
{
    internal sealed class FileAccessException : Exception
    {
        public FileAccessException(string filename)
            : base($"There was a problem creating or accessing the {filename} file.")
        {
            Filename = filename;
        }

        public string Filename { get; }
    }
}
