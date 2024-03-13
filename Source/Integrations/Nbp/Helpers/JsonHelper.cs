using System.Text.Json;

namespace Taxalo.Integrations.Nbp
{
    internal static class JsonHelper
    {
        public static async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();

                var result = await JsonSerializer.DeserializeAsync<T>(stream);

                return result ?? throw new UnexpectedResponseException();
            }
            catch (JsonException exception)
            {
                throw new UnexpectedResponseException(exception);
            }
        }
    }
}
