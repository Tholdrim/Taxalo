using System.Text.Json.Serialization;

namespace Taxalo.Integrations.Nbp
{
    internal class NbpHttpResponse
    {
        [JsonRequired]
        [JsonPropertyName("rates")]
        public IEnumerable<NbpHttpRecord> Rates { get; set; } = null!;
    }
}
