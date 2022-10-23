using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SystemServiceAPICore3.Bo.Interface
{
    public interface IEntityBo<T>
        where T : class
    {
        #region Get

        T Get(object[] ids);

        IEnumerable<T> GetList();

        IEnumerable<T> FindBy(Expression<Func<T, bool>> propertyPredicate);

        T FirstOrDefault(Expression<Func<T, bool>> propertyPredicate);

        Task<T> GetAsync(object[] ids);

        Task<IEnumerable<T>> GetListAsync();

        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> propertyPredicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> propertyPredicate);

        bool Exists(object[] ids);

        #endregion

        #region Insert

        T Insert(T entity);

        T[] Insert(T[] entities);

        Task<T> InsertAsync(T entity);

        Task<T[]> InsertAsync(T[] entities);

        #endregion

        #region Update

        T Update(T entity);

        T[] Update(T[] entities);

        Task<T> UpdateAsync(T entity);

        Task<T[]> UpdateAsync(T[] entities);

        #endregion

        #region Delete

        bool Delete(object[] ids);

        bool Delete(T entity);

        bool Delete(T[] entities);

        bool Delete(Expression<Func<T, bool>> propertyPredicate);

        Task<bool> DeleteAsync(object[] ids);

        Task<bool> DeleteAsync(T entity);

        Task<bool> DeleteAsync(T[] entities);

        Task<bool> DeleteAsync(Expression<Func<T, bool>> propertyPredicate);

        #endregion

        #region Aggregates

        int Count();

        int Count(Expression<Func<T, bool>> propertyPredicate);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> propertyPredicate);

        #endregion

        #region Paging

        IEnumerable<T> GetListPaged(
            Expression<Func<T, bool>> propertyPredicate,
            int pageNo,
            int pageSize,
            bool ascending = true,
            params Expression<Func<T, object>>[] sortingExpression);

        IEnumerable<T> GetListPaged(
            Expression<Func<T, bool>> propertyPredicate,
            int pageNo,
            int pageSize);

        Task<IEnumerable<T>> GetListPagedAsync(
            Expression<Func<T, bool>> propertyPredicate,
            int pageNo,
            int pageSize,
            bool ascending = true,
            params Expression<Func<T, object>>[] sortingExpression);

        Task<IEnumerable<T>> GetListPagedAsync(
           Expression<Func<T, bool>> propertyPredicate,
           int pageNo,
           int pageSize);

        #endregion

        #region Execution

        int ExecuteSqlRaw(string sql, params object[] parameters);

        int ExecuteSqlInterpolated(FormattableString sql);

        IEnumerable<TAny> FromSqlRaw<TAny>(string sql, params object[] parameters) where TAny : class;

        IEnumerable<TAny> FromSqlInterpolated<TAny>(FormattableString sql) where TAny : class;

        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);

        Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql);

        Task<IEnumerable<TAny>> FromSqlRawAsync<TAny>(string sql, params object[] parameters) where TAny : class;

        Task<IEnumerable<TAny>> FromSqlInterpolatedAsync<TAny>(FormattableString sql) where TAny : class;

        IQueryable<TAny> ExecuteStore<TAny>(string sql, object param) where TAny : class;

        DataSet ExecuteSpDataSet(string sql, object param);

        DataTable ExecuteSpDataTable(string sql, object param);

        #endregion

        #region Others
        #endregion
    }
}

