using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using F2xFullStackAssesment.Infraestructure.Exceptions;
using F2xFullStackAssesment.Infraestructure.IResources;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace F2xFullStackAssesment.Core.Helpers
{
    public class MicroClientHelper : IMicroClientHelper
    {
        private readonly IConfigProvider configProvider;
        private readonly IResource resource;

        public MicroClientHelper(IConfigProvider configProvider, IResource resource)
        {
            this.configProvider = configProvider;
            this.resource = resource;
        }

        public async Task<string> GetTokenMicroServiceAsync()
        {
            JObject result = null;

            using (HttpClient restClient = new HttpClient())
            {
                restClient.DefaultRequestHeaders.Add(configProvider.Header, configProvider.HeaderValue);
                var credentials = new { userName = configProvider.UserName, password = configProvider.Password };
                HttpContent content = ObjectAsStringContent(credentials);

                result = await resource.GetRetryPolicy("GetTokenMicroServiceAsync").ExecuteAsync(async () =>
                {
                    var response = await restClient.PostAsync(configProvider.TokenPath, content).ConfigureAwait(false);
                    return await ProcessResponseAsync<JObject>(response).ConfigureAwait(false);
                }).ConfigureAwait(false);
            }

            if (!result.TryGetValue("token", StringComparison.InvariantCultureIgnoreCase, out JToken token))
            {
                throw new KeyNotFoundException("No se retornó token del servicio  para consumir peticiones a microservicios");
            }

            return token.Value<string>();
        }

        public async Task<T> GetDataAsync<T>(string path, string token = "", string schema = "Bearer") where T : class, new()
        {
            using (var restClient = new HttpClient())
            {
                AddHeadersMicroServices(token, schema, restClient);

                return await resource.GetRetryPolicy("GetDataAsync").ExecuteAsync(async () =>
                {
                    var response = await restClient.GetAsync(path).ConfigureAwait(true);
                    response.EnsureSuccessStatusCode();
                    return await ProcessResponseAsync<T>(response).ConfigureAwait(true);
                }).ConfigureAwait(false);
            }
        }

        #region Private Members
        private static StringContent ObjectAsStringContent(object obj)
        {
            string objectJSON = JsonConvert.SerializeObject(obj);
            return new StringContent(objectJSON, Encoding.UTF8, "application/json");
        }

        private static void AddHeadersMicroServices(string token, string schema, HttpClient restClient)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (schema.Equals("Bearer"))
                {
                    restClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    restClient.DefaultRequestHeaders.Add(schema, token);
                }
                
            }
            restClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        }

        private static async Task<T> ProcessResponseAsync<T>(HttpResponseMessage response) where T : class, new()
        {
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomRestClientException($"ha ocurrido un error al tratar de realizar la petición {response.RequestMessage.Method} a {response.RequestMessage.RequestUri} " +
                    $"StatusCode: {response.StatusCode.GetHashCode()}, {result}"
                    , response);
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        #endregion
    }
}
