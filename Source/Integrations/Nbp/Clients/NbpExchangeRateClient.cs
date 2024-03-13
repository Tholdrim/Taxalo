using System.Net;

namespace Taxalo.Integrations.Nbp
{
    internal class NbpExchangeRateClient : IExchangeRateClient
    {
        private readonly HttpClient _httpClient;

        public NbpExchangeRateClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string currency, int year)
        {
            var previousYearExchangeRates = await GetExchangeRatesAsync(currency,
                startDate: new DateOnly(year - 1, 12, 1),
                endDate: new DateOnly(year - 1, 12, 31));

            var currentYearExchangeRates = await GetExchangeRatesAsync(currency,
                startDate: new DateOnly(year, 1, 1),
                endDate: new DateOnly(year, 12, 30));

            return currentYearExchangeRates.Concat(previousYearExchangeRates.Take(1)).ToList();
        }

        private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string currency, DateOnly startDate, DateOnly endDate)
        {
            var response = await GetNbpResponsesAsync(currency, startDate, endDate).OfType<NbpHttpResponse>().FirstOrDefaultAsync();

            if (response == null)
            {
                throw new ExchangeRateRetrievalException(currency, startDate, endDate);
            }

            return response.Rates.Select(r => r.ToExchangeRate()).OrderByDescending(r => r.Date);
        }

        private async IAsyncEnumerable<NbpHttpResponse?> GetNbpResponsesAsync(string currency, DateOnly startDate, DateOnly endDate)
        {
            var variableParameters = $"{currency}/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}/";

            yield return await GetNbpResponseAsync($"exchangerates/rates/A/{variableParameters}/");
            yield return await GetNbpResponseAsync($"exchangerates/rates/B/{variableParameters}/");
        }

        private async Task<NbpHttpResponse?> GetNbpResponseAsync(string route)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(route, UriKind.Relative));
            using var response = await _httpClient.SendAsync(request);

            return response.StatusCode switch
            {
                HttpStatusCode.OK => await JsonHelper.DeserializeAsync<NbpHttpResponse>(response),
                _                 => null
            };
        }
    }
}
