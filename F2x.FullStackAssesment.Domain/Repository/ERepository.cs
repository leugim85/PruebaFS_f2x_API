using Microsoft.EntityFrameworkCore;
using F2xF2xFullStackAssesment.Domain.Entities;
using F2xFullStackAssesment.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Domain.Repository
{
    public class ERepository<TId, TEntity> : IERepository<TId, TEntity>
        where TId : struct
        where TEntity : EntityBase<TId>
    {
        private readonly IQueryableUnitOfWork unitOfWork;

        public ERepository(IQueryableUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return await BuildQuery(filter, orderBy, includeProperties).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> GetAllPagedAsync(
            int take,
            int skip,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return await BuildQuery(filter, orderBy, includeProperties).Skip(skip).Take(take).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> GetAllPagedAsync(
            int take,
            int skip,
            List<Expression<Func<TEntity, bool>>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return await BuildQuery(filters, orderBy, includeProperties).Skip(skip).Take(take).ToListAsync().ConfigureAwait(false);
        }



        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await BuildQuery(filter).CountAsync();
        }

        public async Task<int> CountAsync(List<Expression<Func<TEntity, bool>>> filters = null)
        {
            return await BuildQuery(filters).CountAsync();
        }

        public bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            var item = unitOfWork.GetSet<TEntity, TId>().AsNoTracking();
            return filter == null ? item.Any() : item.Any(filter);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var item = unitOfWork.GetSet<TEntity, TId>().AsNoTracking();
            return filter == null ? await item.AnyAsync().ConfigureAwait(false) : await item.AnyAsync(filter).ConfigureAwait(false);
        }



        public async Task AddAsync(TEntity entity)
        {
            ValidateEntity(entity);

            try
            {
                await unitOfWork.GetSet<TEntity, TId>().AddAsync(entity).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await unitOfWork.CommitAsync().ConfigureAwait(false);
            }
        }



        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            ValidateRangeEntities(entities);
            try
            {
                await unitOfWork.GetSet<TEntity, TId>().AddRangeAsync(entities).ConfigureAwait(false);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                await unitOfWork.CommitAsync().ConfigureAwait(false);
            }
        }






        #region PrivateMethods
        private IQueryable<TEntity> BuildQuery(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = unitOfWork.GetSet<TEntity, TId>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(property =>
            {
                query = query.Include(property);
            });

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }

        private IQueryable<TEntity> BuildQuery(
            List<Expression<Func<TEntity, bool>>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = unitOfWork.GetSet<TEntity, TId>().AsNoTracking();

            if (filters is not null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(property =>
            {
                query = query.Include(property);
            });

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }

        private static void ValidateEntity(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "El objeto entidad no puede ser nulo");
            }
        }

        private static void ValidateRangeEntities(IEnumerable<TEntity> entities)
        {
            if (!entities?.Any() ?? true)
            {
                throw new ArgumentNullException(nameof(entities), "no se envió una lista de entidades a insertar");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
        }

        #endregion

    }
}
