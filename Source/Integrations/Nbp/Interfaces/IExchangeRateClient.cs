namespace Taxalo.Integrations.Nbp
{
    internal interface IExchangeRateClient
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string currency, int year);
    }
}
