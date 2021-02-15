using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Taxalo.ExchangeRates.Enums;
using Taxalo.ExchangeRates.Exceptions;
using Taxalo.ExchangeRates.Interfaces;

namespace Taxalo.ExchangeRates.Providers
{
    internal sealed class ParametersProvider : IParametersProvider
    {
        private readonly Lazy<int> _year;
        private readonly Lazy<IEnumerable<string>> _currencies;

        public ParametersProvider(IConfiguration configuration)
        {
            _year = new Lazy<int>(() => GetYear(configuration));
            _currencies = new Lazy<IEnumerable<string>>(() => GetCurrencies(configuration));
        }

        public int Year => _year.Value;

        public IEnumerable<string> Currencies => _currencies.Value;

        private static IEnumerable<string> GetCurrencies(IConfiguration configuration)
        {
            const string separator = ",";

            var parameterValue = configuration.GetValue<string?>(Parameter.Currencies.ToString());
            var result = parameterValue?.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (result == null || !result.Any())
            {
                throw new ParameterException(Parameter.Currencies);
            }

            return result.Select(c => c.ToUpper());
        }

        private static int GetYear(IConfiguration configuration)
        {
            var parameterValue = configuration.GetValue<string?>(Parameter.Year.ToString());

            if (!int.TryParse(parameterValue, out var result))
            {
                throw new ParameterException(Parameter.Year);
            }

            return result;
        }
    }
}
