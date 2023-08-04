using F2xF2xFullStackAssesment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Domain
{
    public interface IQueryableUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();
        DbContext GetContext();
        void DetachLocal<TEntity>(TEntity entity, EntityState state) where TEntity : class;
        DbSet<TEntity> GetSet<TEntity, TId>()
            where TEntity : EntityBase<TId>
            where TId : struct;
    }
}
