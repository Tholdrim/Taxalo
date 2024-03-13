namespace Taxalo.Integrations.Nbp
{
    internal static class NbpHttpRecordMappings
    {
        public static ExchangeRate ToExchangeRate(this NbpHttpRecord response) => new
        (
            Table: response.TableNumber,
            Date: response.PublicationDate,
            Value: response.AverageExchangeRate
        );
    }
}
