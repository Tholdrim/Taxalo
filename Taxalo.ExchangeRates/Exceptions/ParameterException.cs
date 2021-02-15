using System;
using Taxalo.ExchangeRates.Enums;

namespace Taxalo.ExchangeRates.Exceptions
{
    internal sealed class ParameterException : Exception
    {
        public ParameterException(Parameter parameter)
            : base($"The parameter '{parameter}' is not specified or is invalid.")
        {
            Parameter = parameter;
        }

        public Parameter Parameter { get; }
    }
}
