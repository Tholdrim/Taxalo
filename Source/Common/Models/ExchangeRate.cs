namespace Taxalo
{
    public record ExchangeRate(string Table, DateOnly Date, decimal Value)
    {
        public const string DefaultCurrency = "PLN";
    }
}
