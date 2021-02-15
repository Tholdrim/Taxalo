using System.Collections.Generic;

namespace Taxalo.ExchangeRates.Interfaces
{
    internal interface IParametersProvider
    {
        public int Year { get; }

        public IEnumerable<string> Currencies { get; }
    }
}
