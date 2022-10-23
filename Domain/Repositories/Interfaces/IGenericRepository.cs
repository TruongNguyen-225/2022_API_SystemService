using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SystemServiceAPICore3.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        #region Get

        IEnumerable<T> GetAll();

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        T Get(object[] ids);

        T Single(Expression<Func<T, bool>> predicate);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetAsync(object[] ids);

        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        #endregion

        #region Insert

        T Insert(T entity, bool saveChanges = false);

        T[] Insert(T[] entities, bool saveChanges = false);

        Task<T> InsertAsync(T entity, bool saveChanges = false);

        Task<T[]> InsertAsync(T[] entities, bool saveChanges = false);

        #endregion

        #region Update

        T Update(T entity, bool saveChanges = false);

        T[] Update(T[] entities, bool saveChanges = false);

        Task<T> UpdateAsync(T entity, bool saveChanges = false);

        Task<T[]> UpdateAsync(T[] entities, bool saveChanges = false);

        #endregion

        #region Delete

        bool Delete(T entity, bool saveChanges = false);

        bool Delete(object[] ids, bool saveChanges = false);

        bool Delete(T[] entities, bool saveChanges = false);

        bool Delete(Expression<Func<T, bool>> predicate, bool saveChanges = false);

        Task<bool> DeleteAsync(T entity, bool saveChanges = false);

        Task<bool> DeleteAsync(object[] ids, bool saveChanges = false);

        Task<bool> DeleteAsync(T[] entities, bool saveChanges = false);

        Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, bool saveChanges = false);

        #endregion

        #region Aggregates

        int Count();

        int Count(Expression<Func<T, bool>> predicate);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        #endregion

        #region Paging

        IEnumerable<T> GetListPaged(Expression<Func<T, bool>> predicate, int pageNo, int pageSize, bool ascending = true, params Expression<Func<T, object>>[] sortingExpression);

        IEnumerable<T> GetListPaged(Expression<Func<T, bool>> predicate, int pageNo, int pageSize);

        Task<IEnumerable<T>> GetListPagedAsync(Expression<Func<T, bool>> predicate, int pageNo, int pageSize, bool ascending = true, params Expression<Func<T, object>>[] sortingExpression);

        Task<IEnumerable<T>> GetListPagedAsync(Expression<Func<T, bool>> predicate, int pageNo, int pageSize);

        #endregion

        #region Others

        bool Save();

        Task<bool> SaveAsync();

        Task<bool> SaveAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
