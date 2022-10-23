using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SystemServiceAPICore3.Domain.Helpers;

namespace SystemServiceAPICore3.Repositories
{
    public class EfCoreGenericRepository<T> : BaseEfCoreGenericRepository<T>
        where T : class
    {
        #region -- Variables --

        internal DbContext context;

        internal DbSet<T> dbSet;

        #endregion

        #region -- Properties --

        #endregion

        #region -- Constructors --

        public EfCoreGenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        #endregion

        #region -- Overrides --

        #region Implement IGenericRepository

        #region Get

        public override T Get(object[] ids)
        {
            // Vaildate param.
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            return dbSet.Find(ids);
        }

        public override async Task<T> GetAsync(object[] ids)
        {
            // Vaildate param.
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            return await dbSet.FindAsync(ids);
        }

        #endregion

        #region Insert

        public override T Insert(T entity, bool saveChanges = false)
        {
            // Vaildate param.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            dbSet.Add(entity);
            if (saveChanges) context.SaveChanges(); // Test lại
            return entity;
        }

        public override T[] Insert(T[] entities, bool saveChanges = false)
        {
            // Vaildate param.
            if (entities == null || entities.Length == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            dbSet.AddRange(entities);
            if (saveChanges) context.SaveChanges(); // Test lại
            return entities;
        }

        public override async Task<T> InsertAsync(T entity, bool saveChanges = false)
        {
            // Vaildate param.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbSet.AddAsync(entity);
            if (saveChanges) await context.SaveChangesAsync(CancellationToken.None);
            return entity;
        }

        public override async Task<T[]> InsertAsync(T[] entities, bool saveChanges = false)
        {
            // Vaildate param.
            if (entities == null || entities.Length == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            dbSet.AddRange(entities);
            if (saveChanges) await context.SaveChangesAsync(CancellationToken.None);
            return entities;
        }

        #endregion

        #region Update

        public override T Update(T entity, bool saveChanges = false)
        {
            // Vaildate param.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Update param.
            AttachIfNot(entity);
            context.Entry(entity).State = EntityState.Modified;
            if (saveChanges) context.SaveChanges(); // Test lai
            return entity;
        }

        public override async Task<T> UpdateAsync(T entity, bool saveChanges = false)
        {
            // Vaildate param.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Update param.
            AttachIfNot(entity);
            context.Entry(entity).State = EntityState.Modified;
            if (saveChanges) await context.SaveChangesAsync(CancellationToken.None);
            return entity;
        }

        #endregion

        #region Delete

        public override bool Delete(T entity, bool saveChanges = false)
        {
            // Vaildate param.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Remove item.
            AttachIfNot(entity);
            var item = dbSet.Remove(entity);
            if (saveChanges) context.SaveChanges(); // Test lai

            return item != null;
        }

        public override bool Delete(object[] ids, bool saveChanges = false)
        {
            var entity = dbSet.Find(ids);
            return Delete(entity, saveChanges);
        }

        public override async Task<bool> DeleteAsync(T entity, bool saveChanges = false)
        {
            // Vaildate param.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Remove item.
            AttachIfNot(entity);
            var item = dbSet.Remove(entity);
            if (saveChanges) await context.SaveChangesAsync(CancellationToken.None);

            return item != null;
        }

        public override async Task<bool> DeleteAsync(object[] ids, bool saveChanges = false)
        {
            var entity = await dbSet.FindAsync(ids);
            return await DeleteAsync(entity, saveChanges);
        }

        #endregion

        #region Aggregates
        #endregion

        #region Paging

        public override IEnumerable<T> GetListPaged(Expression<Func<T, bool>> predicate, int pageNo, int pageSize, bool ascending = true, params Expression<Func<T, object>>[] sortingExpression)
        {
            var queryable = AsQueryable();

            // Get query able.
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            // Apply sort.
            if (sortingExpression != null)
            {
                foreach (var sortExp in sortingExpression)
                {
                    queryable = ascending ? queryable.OrderBy(sortExp) : queryable.OrderByDescending(sortExp);
                }
            }

            // Apply paging.
            queryable = queryable.Skip((pageNo - 1) * pageSize).Take(pageSize);

            // Return result.
            return queryable.AsEnumerable();
        }

        public override IEnumerable<T> GetListPaged(Expression<Func<T, bool>> predicate, int pageNo, int pageSize)
        {
            // Get sort based on keys.
            var sortExps = EntityHelper.GetKeySortExpression<T>();

            // Get data list.
            var data = GetListPaged(predicate, pageNo, pageSize, true, sortExps);

            return data;
        }

        public override async Task<IEnumerable<T>> GetListPagedAsync(Expression<Func<T, bool>> predicate, int pageNo, int pageSize, bool ascending = true, params Expression<Func<T, object>>[] sortingExpression)
        {
            return await Task.FromResult(GetListPaged(predicate, pageNo, pageSize, ascending, sortingExpression));
        }

        public override async Task<IEnumerable<T>> GetListPagedAsync(Expression<Func<T, bool>> predicate, int pageNo, int pageSize)
        {
            return await Task.FromResult(GetListPaged(predicate, pageNo, pageSize));
        }

        #endregion

        #region Others

        public override bool Save()
        {
            return (context.SaveChanges() >= 0);
        }

        public override async Task<bool> SaveAsync()
        {
            var result = await context.SaveChangesAsync();
            return (result >= 0);
        }

        public override async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            var result = await context.SaveChangesAsync(cancellationToken);
            return (result >= 0);
        }

        #endregion

        #endregion

        #region Implement IBaseEfCoreGenericRepository

        public override IQueryable<T> AsQueryable()
        {
            return AsNoTracking ? dbSet.AsQueryable() : dbSet;
        }

        public override IQueryable<T> FromSqlRaw(string sql, params object[] parameters)
        {
            return parameters == null ? this.dbSet.FromSqlRaw(sql) : this.dbSet.FromSqlRaw(sql, parameters);
        }

        public override IQueryable<T> FromSqlInterpolated(FormattableString sql)
        {
            return this.dbSet.FromSqlInterpolated(sql);
        }

        #endregion

        #endregion

        #region -- Methods --

        protected virtual void AttachIfNot(T entity)
        {
            if (!dbSet.Local.Contains(entity))
            {
                dbSet.Attach(entity);
            }
        }

        #endregion
    }
}
