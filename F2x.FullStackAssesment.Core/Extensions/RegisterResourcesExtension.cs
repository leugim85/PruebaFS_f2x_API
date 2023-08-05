using Autofac;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using F2xFullStackAssesment.Core.Helpers;
using F2xFullStackAssesment.Domain;
using F2xFullStackAssesment.Infraestructure.IResources;
using F2xFullStackAssesment.Infraestructure.Resources;
using DBContextF2xF2xFullStackAssesment.Domain;
using F2xF2xFullStackAssesment.Core.Extensions;

namespace F2xFullStackAssesment.Core.Extensions
{
    public static class RegisterResourcesExtension
    {
        public static void RegisterResources(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<DBContextF2xFullStackAssesment>().As<IQueryableUnitOfWork>()
                .WithParameter("options", new DbContextOptionsBuilder<DBContextF2xFullStackAssesment>()
                .UseSqlServer(configuration.GetConnectionString("StringConnection"), x => x.MigrationsHistoryTable("__MigrationHistoryFx2Assesment", configuration.GetConnectionString("SchemaName")))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options)
                .WithParameter("schema", configuration.GetConnectionString("SchemaName")).InstancePerLifetimeScope();

            builder.RegisterType<AutoMigrateDbF2xFullStackAssesment>().As<IStartable>().SingleInstance();


            builder.RegisterType<ConfigProvider>().As<IConfigProvider>();
            builder.RegisterType<Resource>().As<IResource>();
            builder.RegisterType<MicroClientHelper>().As<IMicroClientHelper>();

        }
    }
}
