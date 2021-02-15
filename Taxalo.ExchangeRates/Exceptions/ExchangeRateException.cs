using System;

namespace Taxalo.ExchangeRates.Exceptions
{
    internal sealed class ExchangeRateException : Exception
    {
        public ExchangeRateException(string currency, DateTime from, DateTime to)
            : base($"The {currency} exchange rates for the period {from:yyyy-MM-dd} – {to:yyyy-MM-dd} could not be retrieved.")
        {
            Currency = currency;
            From = from;
            To = to;
        }

        public string Currency { get; }

        public DateTime From { get; }

        public DateTime To { get; }
    }
}
