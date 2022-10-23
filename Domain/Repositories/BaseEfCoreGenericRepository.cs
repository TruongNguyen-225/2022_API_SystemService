using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemServiceAPICore3.Repositories.Interfaces;

namespace SystemServiceAPICore3.Repositories
{
    public abstract class BaseEfCoreGenericRepository<T> : IBaseEfCoreGenericRepository<T>
        where T : class
    {
        #region Implement IGenericRepository

        #region Get

        public IEnumerable<T> GetAll()
        {
            return AsQueryable();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            // Vaildate param.
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return AsQueryable().Where(predicate);
        }

        public abstract T Get(object[] ids);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await AsQueryable().ToListAsync();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            // Vaildate param.
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return AsQueryable().Single(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            // Vaildate param.
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return AsQueryable().FirstOrDefault(predicate);
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            // Vaildate param.
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await AsQueryable().Where(predicate).ToListAsync();
        }

        public abstract Task<T> GetAsync(object[] ids);

        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            // Vaildate param.
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await AsQueryable().SingleAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            // Vaildate param.
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await AsQueryable().FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region Insert

        public abstract T Insert(T entity, bool saveChanges = false);

        public abstract T[] Insert(T[] entities, bool saveChanges = false);

        public abstract Task<T> InsertAsync(T entity, bool saveChanges = false);

        public abstract Task<T[]> InsertAsync(T[] entities, bool saveChanges = false);

        #endregion

        #region Update

        public abstract T Update(T entity, bool saveChanges = false);

        public T[] Update(T[] entities, bool saveChanges = false)
        {
            if (entities == null || entities.Length == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var updates = new List<T>();
            foreach (var item in entities)
            {
                var model = Update(item, saveChanges);
                updates.Add(model);
            }

            return updates.ToArray();
        }

        public abstract Task<T> UpdateAsync(T entity, bool saveChanges = false);

        public async Task<T[]> UpdateAsync(T[] entities, bool saveChanges = false)
        {
            if (entities == null || entities.Length == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var updates = new List<T>();
            foreach (var item in entities)
            {
                var model = await UpdateAsync(item, saveChanges);
                updates.Add(model);
            }

            return updates.ToArray();
        }

        #endregion

        #region Delete

        public abstract bool Delete(T entity, bool saveChanges = false);

        public abstract bool Delete(object[] ids, bool saveChanges = false);

        public bool Delete(T[] entities, bool saveChanges = false)
        {
            if (entities == null || entities.Length == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var result = true;
            foreach (var entity in entities)
            {
                result = result && Delete(entity, saveChanges);
            }

            return result;
        }

        public bool Delete(Expression<Func<T, bool>> predicate, bool saveChanges = false)
        {
            var result = true;
            foreach (var entity in FindBy(predicate))
            {
                result = result && Delete(entity, saveChanges);
            }

            return result;
        }

        public abstract Task<bool> DeleteAsync(T entity, bool saveChanges = false);

        public abstract Task<bool> DeleteAsync(object[] ids, bool saveChanges = false);

        public async Task<bool> DeleteAsync(T[] entities, bool saveChanges = false)
        {
            if (entities == null || entities.Length == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var result = true;
            foreach (var entity in entities)
            {
                result = result && await DeleteAsync(entity, saveChanges);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, bool saveChanges = false)
        {
            var result = true;
            foreach (var entity in await FindByAsync(predicate))
            {
                result = result && await DeleteAsync(entity, saveChanges);
            }

            return result;
        }

        #endregion

        #region Aggregates

        public int Count()
        {
            return AsQueryable().Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return AsQueryable().Count(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await AsQueryable().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await AsQueryable().CountAsync(predicate);
        }

        #endregion

        #region Paging

        public abstract IEnumerable<T> GetListPaged(Expression<Func<T, bool>> predicate, int pageNo, int pageSize, bool ascending = true, params Expression<Func<T, object>>[] sortingExpression);

        public abstract IEnumerable<T> GetListPaged(Expression<Func<T, bool>> predicate, int pageNo, int pageSize);

        public abstract Task<IEnumerable<T>> GetListPagedAsync(Expression<Func<T, bool>> predicate, int pageNo, int pageSize, bool ascending = true, params Expression<Func<T, object>>[] sortingExpression);

        public abstract Task<IEnumerable<T>> GetListPagedAsync(Expression<Func<T, bool>> predicate, int pageNo, int pageSize);

        #endregion

        #region Others

        public abstract bool Save();

        public abstract Task<bool> SaveAsync();

        public abstract Task<bool> SaveAsync(CancellationToken cancellationToken = default);

        #endregion

        #endregion

        #region Implement IBaseEfCoreGenericRepository

        public bool AsNoTracking { get; set; } = false;

        public abstract IQueryable<T> AsQueryable();

        public abstract IQueryable<T> FromSqlRaw(string sql, params object[] parameters);

        public abstract IQueryable<T> FromSqlInterpolated(FormattableString sql);

        #endregion
    }
}
