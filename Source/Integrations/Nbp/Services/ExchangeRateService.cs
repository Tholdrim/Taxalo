using System.Collections.Concurrent;

using GroupingKey = (string Currency, int Year);

namespace Taxalo.Integrations.Nbp
{
    internal class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateClient _exchangeRateClient;

        public ExchangeRateService(IExchangeRateClient exchangeRateClient)
        {
            _exchangeRateClient = exchangeRateClient;
        }

        private ConcurrentDictionary<GroupingKey, IEnumerable<ExchangeRate>> Cache { get; } = [];

        public IEnumerable<CurrencyExchangeRates> GetAllExchangeRates()
        {
            return Cache
                .SelectMany(p => p.Value.Select(r => new { p.Key.Currency, ExchangeRate = r }))
                .GroupBy(e => e.Currency)
                .Select(g => new CurrencyExchangeRates
                {
                    Currency = g.Key,
                    ExchangeRates = g.Select(e => e.ExchangeRate).Distinct().OrderBy(r => r.Date)
                })
                .ToList();
        }

        public async Task<ExchangeRate> GetExchangeRateAsync(string currency, DateOnly date)
        {
            if (currency == ExchangeRate.DefaultCurrency)
            {
                return new(Table: "-", Date: date, Value: 1.00m);
            }

            var groupingKey = (currency, date.Year);

            if (!Cache.TryGetValue(groupingKey, out var exchangeRates))
            {
                exchangeRates = Cache.GetOrAdd(groupingKey, await _exchangeRateClient.GetExchangeRatesAsync(currency, date.Year));
            }

            return exchangeRates.Where(r => r.Date < date).First();
        }
    }
}
