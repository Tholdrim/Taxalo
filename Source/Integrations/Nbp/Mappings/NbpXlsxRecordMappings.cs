namespace Taxalo.Integrations.Nbp
{
    internal static class NbpXlsxRecordMappings
    {
        public static NbpXlsxRecord ToXlsxRecord(this ExchangeRate exchangeRate) => new()
        {
            Date = exchangeRate.Date,
            Table = exchangeRate.Table,
            Value = exchangeRate.Value
        };
    }
}
