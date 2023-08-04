using Autofac.Extensions.DependencyInjection;
using Autofac;
using F2xFullStackAssesment.TimerWorker;
using F2xFullStackAssesment.Core.Extensions;

IConfiguration configuration = null;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        var parentDir = Directory.GetParent(hostContext.HostingEnvironment.ContentRootPath);
        var path = string.Concat(parentDir.FullName, "secrets/appsettings.secrets.json");
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddJsonFile(path, optional: true, reloadOnChange: true);
        configuration = config.Build();
    })
     .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterService();
                builder.RegisterRepositories(configuration);
                builder.RegisterResources(configuration);
            })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();

        services.AddCors(opt =>
        {

            opt.AddPolicy("AllowAll", p =>
            {
                p.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders();
            });
        });
        //register Services
        services.AddServices(configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
