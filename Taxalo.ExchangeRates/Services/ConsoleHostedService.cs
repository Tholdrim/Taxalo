using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Taxalo.ExchangeRates.Exceptions;
using Taxalo.ExchangeRates.Helpers;
using Taxalo.ExchangeRates.Interfaces;
using Taxalo.ExchangeRates.Models;

namespace Taxalo.ExchangeRates.Services
{
    internal sealed class ConsoleHostedService : IHostedService
    {
        private readonly IParametersProvider _parametersProvider;
        private readonly INbpExchangeRateService _nbpExchangeRateService;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly ILogger _logger;

        public ConsoleHostedService(
            IParametersProvider parametersProvider,
            INbpExchangeRateService nbpExchangeRateService,
            IHostApplicationLifetime applicationLifetime,
            ILogger<ConsoleHostedService> logger)
        {
            _parametersProvider = parametersProvider;
            _nbpExchangeRateService = nbpExchangeRateService;
            _applicationLifetime = applicationLifetime;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.ApplicationStarted.Register(async () => await OnStartedAsync());

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task OnStartedAsync()
        {
            try
            {
                var year = _parametersProvider.Year;

                foreach (var currency in _parametersProvider.Currencies)
                {
                    _logger.LogInformation($"Generating a file with {currency} rates…");

                    var filename = $"{currency}-{year}.json";
                    var exchangeRates = await _nbpExchangeRateService.GetExchangeRatesAsync(currency, year);

                    await GenerateExchangeRateFileAsync(exchangeRates, filename);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
            finally
            {
                _applicationLifetime.StopApplication();
            }
        }

        private static async Task GenerateExchangeRateFileAsync(ICollection<ExchangeRate> exchangeRates, string filename)
        {
            try
            {
                using var fileStream = File.OpenWrite(filename);

                await JsonSerializer.SerializeAsync(fileStream, exchangeRates, SerializationHelper.Options);
            }
            catch
            {
                throw new FileAccessException(filename);
            }
        }
    }
}
