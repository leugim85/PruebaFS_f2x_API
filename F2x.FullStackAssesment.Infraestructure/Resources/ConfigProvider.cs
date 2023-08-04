using Microsoft.Extensions.Configuration;
using F2xFullStackAssesment.Infraestructure.IResources;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace F2xFullStackAssesment.Infraestructure.Resources
{
    [ExcludeFromCodeCoverage]
    public class ConfigProvider : IConfigProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region RetryPolicy

        public int GetRetryCount()
        {
            var retryCount = _configuration.GetSection("RetryPolicy:RetryCount").Value;

            if (!int.TryParse(retryCount, out int result))
            {
                throw new InvalidCastException($"El valor de la variable de configuracion RetryCount: {retryCount} no es valido ");
            }

            return result;
        }

        public double GetRetrySeconds()
        {
            var retrySeconds = _configuration.GetSection("RetryPolicy:RetrySeconds").Value;

            if (!double.TryParse(retrySeconds, out double result))
            {
                throw new InvalidCastException($"El valor de la variable de configuracion RetrySeconds: {retrySeconds} no es valido");
            }

            return result;
        }
        #endregion

        #region ApiService
        private string BasePath => _configuration.GetSection("ApiService:BaseUrl").Value;
        public string Header => _configuration.GetSection("ApiService:Header").Value;
        public string HeaderValue => _configuration.GetSection("ApiService:HeaderValue").Value;

        public string UserName => _configuration.GetSection("ApiService:userName").Value;
        public string Password => _configuration.GetSection("ApiService:password").Value;
        public string TokenPath => $"{BasePath}{_configuration.GetSection("ApiService:ResourcePath:Token").Value}";
        public string VehiclesCounterPath => $"{BasePath}{_configuration.GetSection("ApiService:ResourcePath:Counter").Value}";
        public string VehiclesCounterValuePath => $"{BasePath}{_configuration.GetSection("ApiService:ResourcePath:CounterValue").Value}";
        #endregion

        public List<int> ExecutionDays => _configuration.GetSection($"ExecutionDays").Get<List<int>>();
        public string ExecutionHour => _configuration.GetSection($"ExecutionHour").Value;

    }
}
