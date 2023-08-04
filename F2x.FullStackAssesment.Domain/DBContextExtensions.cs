using F2x.FullStackAssesment.Domain.Entities;
using F2xFullStackAssesment.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F2xF2xFullStackAssesment.Domain
{
    public static class DBContextExtensions
    {
        public static bool AllMigrationsApplied(this IQueryableUnitOfWork context)
        {
            var applied = context.GetContext().GetService<IHistoryRepository>()
             .GetAppliedMigrations()
             .Select(m => m.MigrationId);

            var total = context.GetContext().GetService<IMigrationsAssembly>()
             .Migrations
             .Select(m => m.Key);
            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this IQueryableUnitOfWork context)
        {
            if (!context.GetSet<VehicleCounterQueryHistory, Guid>().AsNoTracking().Any())
            {
                context.GetSet<VehicleCounterQueryHistory, Guid>().AddAsync(new VehicleCounterQueryHistory
                {
                    Date = new DateTime(2021,07,31),
                    Quantity = 0,
                });
                context.Commit();
            }
        }
    }
}
