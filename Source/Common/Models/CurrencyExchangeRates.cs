namespace Taxalo
{
    public class CurrencyExchangeRates
    {
        public required string Currency { get; init; }

        public required IEnumerable<ExchangeRate> ExchangeRates { get; init; }
    }
}
