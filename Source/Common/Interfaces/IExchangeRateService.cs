namespace Taxalo
{
    public interface IExchangeRateService
    {
        IEnumerable<CurrencyExchangeRates> GetAllExchangeRates();

        Task<ExchangeRate> GetExchangeRateAsync(string currency, DateOnly date);
    }
}
