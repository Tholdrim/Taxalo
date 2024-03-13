using System.Text.Json.Serialization;

namespace Taxalo.Integrations.Nbp
{
    internal class NbpHttpRecord
    {
        [JsonRequired]
        [JsonPropertyName("no")]
        public string TableNumber { get; set; } = null!;

        [JsonRequired]
        [JsonPropertyName("effectiveDate")]
        public DateOnly PublicationDate { get; set; }

        [JsonRequired]
        [JsonPropertyName("mid")]
        public decimal AverageExchangeRate { get; set; }
    }
}
