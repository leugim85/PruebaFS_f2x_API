using Polly;
using Polly.Retry;
using F2xFullStackAssesment.Infraestructure.IResources;
using Serilog;
using System;
using System.Globalization;

namespace F2xFullStackAssesment.Infraestructure.Resources
{
    public class Resource : IResource
    {
        private readonly IConfigProvider configProvider;
        private const int EXPONENT = 2;

        public Resource(IConfigProvider configProvider)
        {
            this.configProvider = configProvider;
        }

        public AsyncRetryPolicy GetRetryPolicy(string method)
        {
            return Policy.Handle<Exception>().
                WaitAndRetryAsync(configProvider.GetRetryCount(), retryCount => TimeSpan.FromSeconds(Math.Pow(EXPONENT, configProvider.GetRetrySeconds())), onRetry: (timespan, retryCount) =>
                {

                });
        }

        public int GetCurentDateInFormatyyyyMMdd(DateTime date)
        {
            if (!int.TryParse(date.ToString("yyyyMMdd", CultureInfo.InvariantCulture), out int result))
            {
                throw new InvalidCastException($"No fue posible convertir la fecha {date} a un entero");
            }

            return result;
        }
    }
}
