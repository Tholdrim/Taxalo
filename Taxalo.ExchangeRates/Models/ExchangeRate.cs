using System;

namespace Taxalo.ExchangeRates.Models
{
    internal sealed record ExchangeRate(string? Table, DateTime Date, decimal Value);
}
