using Autofac;
using Microsoft.Extensions.Configuration;
using F2xFullStackAssesment.Domain.IRepository;
using F2xFullStackAssesment.Domain.Repository;

namespace F2xFullStackAssesment.Core.Extensions
{
    public static class RegisterRepositoriesExtension
    {
        public static void RegisterRepositories(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<VehicleCounterRepository>().As<IVehicleCounterRepository>();
            builder.RegisterType<VehicleCounterQueryHistoryRepository>().As<IVehicleCounterQueryHistoryRepository>();
            builder.RegisterType<VehicleCounterWithAmountRepository>().As<IVehicleCounterWithAmountRepository>();
        }
    }
}
