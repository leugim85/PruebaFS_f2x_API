using Autofac;
using Microsoft.EntityFrameworkCore;
using F2xF2xFullStackAssesment.Domain;
using F2xFullStackAssesment.Domain;

namespace F2xF2xFullStackAssesment.Core.Extensions
{
    public class AutoMigrateDbF2xFullStackAssesment : IStartable
    {
        private readonly IQueryableUnitOfWork context;

        public AutoMigrateDbF2xFullStackAssesment(IQueryableUnitOfWork context)
        {
            this.context = context;
        }

        public void Start()
        {
            if (!context.AllMigrationsApplied())
            {
                context.GetContext().Database.Migrate();
            }

            context.EnsureSeeded();
        }
    }
}
