using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Taxalo.Integrations.Nbp
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceCollection AddNbpIntegration(this ServiceCollection services)
        {
            services
                .AddHttpClient<IExchangeRateClient, NbpExchangeRateClient>(ConfigureClient)
                .AddPolicyHandler(GetDefaultRetryPolicy());

            services
                .AddSingleton<IExchangeRateService, ExchangeRateService>()
                .AddSingleton<IXlsxWriter, NbpWriter>();

            return services;
        }

        private static void ConfigureClient(IServiceProvider _, HttpClient httpClient)
        {
            httpClient.BaseAddress = new("https://api.nbp.pl/api/");
        }

        private static AsyncRetryPolicy<HttpResponseMessage> GetDefaultRetryPolicy()
        {
            var firstRetryDelay = TimeSpan.FromSeconds(0.1);
            var retryDelays = Backoff.ExponentialBackoff(firstRetryDelay, retryCount: 5);

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryDelays);
        }
    }
}
