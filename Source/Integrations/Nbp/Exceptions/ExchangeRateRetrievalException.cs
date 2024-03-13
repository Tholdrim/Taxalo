namespace Taxalo.Integrations.Nbp
{
    internal class ExchangeRateRetrievalException(string currency, DateOnly startDate, DateOnly endDate)
        : Exception($"The exchange rates for {currency} in the period from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd} could not be retrieved.")
    {
    }
}
