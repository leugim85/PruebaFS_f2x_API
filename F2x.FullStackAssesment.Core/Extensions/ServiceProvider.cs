using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace F2xFullStackAssesment.Core.Extensions
{
    public static class ServiceProvider
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.Load("F2x.FullStackAssesment.Core"));
        }
    }
}
