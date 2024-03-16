using System.Text.Json.Serialization;

namespace Taxalo.Integrations.Nbp
{
    internal class NbpHttpResponse
    {
        [JsonPropertyName("rates")]
        public required IEnumerable<NbpHttpRecord> Rates { get; set; }
    }
}
