using System.Collections.Generic;
using System.Threading.Tasks;
using Taxalo.ExchangeRates.Models;

namespace Taxalo.ExchangeRates.Interfaces
{
    internal interface INbpExchangeRateService
    {
        Task<ICollection<ExchangeRate>> GetExchangeRatesAsync(string currency, int year);
    }
}
