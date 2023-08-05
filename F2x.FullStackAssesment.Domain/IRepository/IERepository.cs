using F2xF2xFullStackAssesment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Domain.IRepository
{
    public interface IERepository<TId, TEntity> : IDisposable
        where TId : struct
        where TEntity : EntityBase<TId>
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
      
        Task<IEnumerable<TEntity>> GetAllAsyncWithFilters(List<Expression<Func<TEntity, bool>>> filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task<int> CountAsync(List<Expression<Func<TEntity, bool>>> filters = null);
        Task<List<TEntity>> FindByAlternateKeyAsync(Expression<Func<TEntity, bool>> alternateKey, string includeProperties = "");
    }
}
