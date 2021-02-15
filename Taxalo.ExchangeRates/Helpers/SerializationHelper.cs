using System.Text.Json;

namespace Taxalo.ExchangeRates.Helpers
{
    internal static class SerializationHelper
    {
        public static JsonSerializerOptions Options => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
