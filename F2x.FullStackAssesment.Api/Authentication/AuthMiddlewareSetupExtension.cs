using F2xFullStackAssesment.Api.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace F2xF2xFullStackAssesment.Api.Authentication
{
    public static class AuthMiddlewareSetupExtension
    {
        public static void AddB2CAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var ValidationKey = services.BuildServiceProvider().GetService<IAzureB2CKeyValidation>();
            var Keys = ValidationKey.GetKeysAsync().GetAwaiter().GetResult();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        RoleClaimType = "rc_role",
                        ValidIssuer = configuration.GetSection("B2CAuthentication").GetValue<string>("TokenIssuer"),
                        ValidAudiences = configuration.GetSection("B2CAuthentication:Audiences").Get<IEnumerable<string>>(),
                        IssuerSigningKeyResolver = (tokenText, securityToken, keyId, parameters) =>
                        {
                            return Keys;
                        }
                    };
                });
        }
    }
}
