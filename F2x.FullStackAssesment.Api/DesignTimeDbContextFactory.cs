
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using DBContextF2xF2xFullStackAssesment.Domain;

namespace F2xFullStackAssesment.Api
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DBContextF2xFullStackAssesment>
    {
        public DBContextF2xFullStackAssesment CreateDbContext(string[] args)
        {

            var parentDir = Directory.GetParent(Directory.GetCurrentDirectory());
            var path = string.Concat(parentDir.FullName, "secrets/appsettings.secrets.json");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(path, optional: true, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<DBContextF2xFullStackAssesment>()
                .UseSqlServer(configuration.GetConnectionString("StringConnection"),
                x => x.MigrationsHistoryTable("__MigrationHistoryF2xAssesment", configuration.GetConnectionString("SchemaName")));
            return new DBContextF2xFullStackAssesment(builder.Options, configuration.GetConnectionString("SchemaName"));

        }
    }
}
