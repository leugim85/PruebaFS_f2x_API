
using Microsoft.EntityFrameworkCore;
using F2xF2xFullStackAssesment.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using F2xFullStackAssesment.Domain;
using F2x.FullStackAssesment.Domain.Entities;

namespace DBContextF2xF2xFullStackAssesment.Domain
{
    public class DBContextF2xFullStackAssesment : DbContext, IQueryableUnitOfWork
    {
        private readonly string schema;

        public DBContextF2xFullStackAssesment(DbContextOptions<DBContextF2xFullStackAssesment> options, string schema) : base(options)
        {
            this.schema = schema;
        }

        public DbContext GetContext()
        {
            return this;
        }

        public DbSet<TEntity> GetSet<TEntity, TId>() where TId : struct where TEntity : EntityBase<TId>
        {
            return Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            modelBuilder.HasDefaultSchema(schema);

            modelBuilder.Entity<VehicleCounterInformation>()
                .ToTable("TblVehicleCount")
                .HasKey(k => k.Id)
                .IsClustered();

            modelBuilder.Entity<VehicleCounterQueryHistory>()
                .ToTable("TblVehicleCounterQueryHistory")
                .HasKey(k => k.Id)
                .IsClustered();

            modelBuilder.Entity<VehicleCounterWithAmount>()
            .ToTable("TblVehicleCounterWithAmount")
            .HasKey(k => k.Id)
            .IsClustered();

            base.OnModelCreating(modelBuilder);
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload(); ;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
        }

        public void DetachLocal<TEntity>(TEntity entity, EntityState state) where TEntity : class
        {

            if (entity is null)
            {
                return;
            }

            var local = Set<TEntity>().Local.ToList();

            if (local?.Any() ?? false)
            {
                local.ForEach(item =>
                {
                    Entry(item).State = EntityState.Detached;
                });
            }

            Entry(entity).State = state;
        }
    }
}
