using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemServiceAPICore3.Repositories.Interfaces;

namespace SystemServiceAPICore3.Uow.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        bool Exist();

        void BeginTransaction();

        void Commit();

        void Rollback();

        void CreateSavepoint(string savePoint);

        void RollbackToSavepoint(string savePoint);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);

        Task CreateSavepointAsync(string savePoint, CancellationToken cancellationToken = default);

        Task RollbackToSavepointAsync(string savePoint, CancellationToken cancellationToken = default);

        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        bool Save();

        Task<bool> SaveAsync(CancellationToken cancellationToken = default);

        //Task SaveAsync(CancellationToken cancellationToken = default);

        int ExecuteSqlRaw(string sql, params object[] parameters);

        int ExecuteSqlInterpolated(FormattableString sql);

        IEnumerable<T> FromSqlRaw<T>(string sql, params object[] parameters) where T : class;

        IEnumerable<T> FromSqlInterpolated<T>(FormattableString sql) where T : class;

        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);

        Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql);

        Task<IEnumerable<T>> FromSqlRawAsync<T>(string sql, params object[] parameters) where T : class;

        Task<IEnumerable<T>> FromSqlInterpolatedAsync<T>(FormattableString sql) where T : class;

        IQueryable<T> ExecuteStore<T>(string sql, object param) where T : class;

        DataSet ExecuteSpDataSet(string sql, object param);

        DataTable ExecuteSpDataTable(string sql, object param);

        void ExecuteSp(string sql, object param);
    }

    public interface IUnitOfWork<out TU> : IUnitOfWork { }
}
