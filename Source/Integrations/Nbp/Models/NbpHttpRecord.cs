using System.Text.Json.Serialization;

namespace Taxalo.Integrations.Nbp
{
    internal class NbpHttpRecord
    {
        [JsonPropertyName("no")]
        public required string TableNumber { get; set; }

        [JsonPropertyName("effectiveDate")]
        public required DateOnly PublicationDate { get; set; }

        [JsonPropertyName("mid")]
        public required decimal AverageExchangeRate { get; set; }
    }
}
