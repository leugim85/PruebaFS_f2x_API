using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using F2xFullStackAssesment.Api.Authentication;
using F2xFullStackAssesment.Api.Filters;
using System.Collections.Generic;
using System.IO;
using F2xF2xFullStackAssesment.Api.Authentication;
using F2xFullStackAssesment.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = null;

builder.Host
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

            });

// Add services to the container.
builder.Services.AddCors(opt =>
{

    opt.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders();
    });
});

//B2C Authentication
//builder.Services.AddSingleton<IAzureB2CKeyValidation>(new AzureB2CKeyValidation(configuration));
//builder.Services.AddB2CAuthentication(configuration);

//register Services
builder.Services.AddServices(configuration);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(typeof(CustomExceptionFilterAttribute));
}).AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "V1",
        Title = "F2xFullStackAssesment.Api",
        Description = "Servicios Rest del proyecto Prueba FullStack"
    });

    //setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    In = ParameterLocation.Header,
    //    Description = "Por favor ingrese su JWT con Bearer en el campo",
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.ApiKey,
    //    Scheme = "Bearer"
    //});

    //setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    //            {
    //                {
    //                    new OpenApiSecurityScheme
    //                    {
    //                        Reference = new OpenApiReference
    //                        {
    //                            Type = ReferenceType.SecurityScheme,
    //                            Id = "Bearer"
    //                        },
    //                        Scheme = "oauth2",
    //                        Name = "Bearer",
    //                        In = ParameterLocation.Header
    //                    },
    //                    new List<string>()
    //                }
    //            });

    setupAction.CustomSchemaIds(schema => schema.FullName);
});

builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

app.UseCors("AllowAll");

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "master-only");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Cache-Control", "no-cache,no-store,must-revalidate");
    context.Response.Headers.Add("Pragma", "no-cache");
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Remove("Server");
    await next();
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

public partial class Program { }

