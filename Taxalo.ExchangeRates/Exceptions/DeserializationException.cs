using System;

namespace Taxalo.ExchangeRates.Exceptions
{
    internal sealed class DeserializationException : Exception
    {
        public DeserializationException()
            : base("An error occurred while deserializing the response from the server.")
        {
        }
    }
}
