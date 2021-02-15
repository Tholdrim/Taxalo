using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Taxalo.ExchangeRates.Enums;
using Taxalo.ExchangeRates.Exceptions;
using Taxalo.ExchangeRates.Interfaces;
using Taxalo.ExchangeRates.Models;

namespace Taxalo.ExchangeRates.Services
{
    internal sealed class NbpExchangeRateService : INbpExchangeRateService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        public NbpExchangeRateService(
            IHttpClientFactory httpClientFactory,
            ILogger<NbpExchangeRateService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ICollection<ExchangeRate>> GetExchangeRatesAsync(string currency, int year)
        {
            var exchangeRatesForPreviousDecember = await GetExchangeRatesForPreviousDecemberAsync(currency, year);
            var exchangeRatesForYear = await GetExchangeRatesForYearAsync(currency, year);

            var result = exchangeRatesForPreviousDecember.TakeLast(1).Concat(exchangeRatesForYear).ToList();

            return result;
        }

        private async Task<Stream> DownloadExchangeRatesAsync(string currency, DateTime from, DateTime to)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            foreach (var table in Enum.GetValues<NbpTable>())
            {
                var url = GetUrl(currency, table, from, to);
                var stream = await GetHttpStreamAsync(httpClient, url);

                if (stream != null)
                {
                    return stream;
                }

                _logger.LogWarning($"The {currency} currency was not found in table {table} for the year {to.Year}.");
            }

            throw new ExchangeRateException(currency, from, to);
        }

        private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string currency, DateTime from, DateTime to)
        {
            using var stream = await DownloadExchangeRatesAsync(currency, from, to);

            try
            {
                return await DeserializeExchangeRatesAsync(stream);
            }
            catch
            {
                throw new DeserializationException();
            }
        }

        private Task<IEnumerable<ExchangeRate>> GetExchangeRatesForPreviousDecemberAsync(string currency, int year)
        {
            var (from, to) = (new DateTime(year - 1, 12, 1), new DateTime(year - 1, 12, 31));

            return GetExchangeRatesAsync(currency, from, to);
        }

        private Task<IEnumerable<ExchangeRate>> GetExchangeRatesForYearAsync(string currency, int year)
        {
            var (from, to) = (new DateTime(year, 1, 1), new DateTime(year, 12, 30));

            return GetExchangeRatesAsync(currency, from, to);
        }

        private static async Task<IEnumerable<ExchangeRate>> DeserializeExchangeRatesAsync(Stream stream)
        {
            using var jsonDocument = await JsonDocument.ParseAsync(stream);

            var exchangeRates = jsonDocument.RootElement
                .GetProperty("rates")
                .EnumerateArray()
                .Select(e => new ExchangeRate
                (
                    Table: e.GetProperty("no").GetString(),
                    Date: e.GetProperty("effectiveDate").GetDateTime(),
                    Value: e.GetProperty("mid").GetDecimal()
                ))
                .ToList();

            return exchangeRates;
        }

        private static async Task<Stream?> GetHttpStreamAsync(HttpClient httpClient, Uri url)
        {
            try
            {
                var httpResponseMessage = await httpClient.GetAsync(url);

                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                return await httpResponseMessage.Content.ReadAsStreamAsync();
            }
            catch
            {
                return null;
            }
        }

        private static Uri GetUrl(string currency, NbpTable table, DateTime from, DateTime to)
        {
            return new Uri($"https://api.nbp.pl/api/exchangerates/rates/{table}/{currency}/{from:yyyy-MM-dd}/{to:yyyy-MM-dd}/");
        }
    }
}
