using F2xFullStackAssesment.Api.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace F2xF2xFullStackAssesment.Api.Authentication
{
    public class AzureB2CKeyValidation : IAzureB2CKeyValidation
    {
        private IEnumerable<RsaSecurityKey> KeyCache;
        private readonly IConfiguration B2CAuthenticationConfig;

        public AzureB2CKeyValidation(IConfiguration configuration)
        {
            B2CAuthenticationConfig = configuration.GetSection("B2CAuthentication") ?? throw new AzureB2CKeyValidationException("No se encontró la sección B2CAuthentication en el AppSettings.Json");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SecurityKey>> GetKeysAsync()
        {
            if (KeyCache != null)
            {
                return KeyCache;
            }

            HttpResponseMessage jwksResponse = null;
            string jwksText;

            using (var httpClient = new HttpClient())
            {
                var jwksRequest = new HttpRequestMessage(HttpMethod.Get, B2CAuthenticationConfig.GetValue<string>("KeysUrl"));
                jwksResponse = await httpClient.SendAsync(jwksRequest);
                jwksText = await jwksResponse.Content.ReadAsStringAsync();
            }

            if (!jwksResponse.IsSuccessStatusCode)
            {
                throw new AzureB2CKeyValidationException("Non-success status code while retrieving JWE Keys: " + jwksResponse.StatusCode);
            }

            var jwksObject = JObject.Parse(jwksText);
            var keysArray = jwksObject["keys"];
            IEnumerable<RsaSecurityKey> jwksKeys;

            if (keysArray == null || keysArray.Type != JTokenType.Array)
            {
                throw new AzureB2CKeyValidationException("Invalid JWKS payload, did you configure the JWKS url correctly?");
            }

            if (keysArray.Any())
            {
                jwksKeys = keysArray.Select(keyNode =>
                {
                    var keyId = (string)keyNode["kid"];
                    var encodedModulus = (string)keyNode["n"];
                    var encodedExponent = (string)keyNode["e"];

                    if (string.IsNullOrEmpty(keyId) || string.IsNullOrEmpty(encodedModulus) ||
                        string.IsNullOrEmpty(encodedExponent))
                    {
                        throw new AzureB2CKeyValidationException("Invalid JWKS payload, did you configure the JWKS url correctly?");
                    }

                    var keyModulus = DecodeBase64Url(encodedModulus);
                    var keyExponent = DecodeBase64Url(encodedExponent);

                    var keyParams = new RSAParameters
                    {
                        Modulus = keyModulus,
                        Exponent = keyExponent
                    };

                    var rsaKey = RSA.Create();
                    rsaKey.ImportParameters(keyParams);

                    var nodeKey = new RsaSecurityKey(rsaKey)
                    {
                        KeyId = keyId
                    };
                    return nodeKey;
                }).ToArray();
            }
            else
            {
                throw new AzureB2CKeyValidationException("Invalid JWKS payload, did you configure the JWKS url correctly?");
            }

            KeyCache = jwksKeys;
            return KeyCache;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InvalidateKeys()
        {
            KeyCache = null;
        }

        private static byte[] DecodeBase64Url(string base64Url)
        {
            var padded = base64Url.Length % 4 == 0 ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
            var base64 = padded.Replace("_", "/").Replace("-", "+");
            return Convert.FromBase64String(base64);
        }
    }
}
