using Autofac;
using F2x.FullStackAssesment.Core.IServices;
using F2x.FullStackAssesment.Core.Services;

namespace F2xFullStackAssesment.Core.Extensions
{
    public static class RegisterServiceExtension
    {
        public static void RegisterService(this ContainerBuilder builder)
        {
            builder.RegisterType<VehicleCountService>().As<IVehicleCountService>();
        }
    }
}
