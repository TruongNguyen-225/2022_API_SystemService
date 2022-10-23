using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemServiceAPICore3.Logging.Interfaces;
using SystemServiceAPICore3.Repositories.Interfaces;
using SystemServiceAPICore3.Uow.Interface;

namespace SystemServiceAPICore3.Uow
{
    public abstract class UnitOfWork<TU> : IUnitOfWork<TU>
    {
        #region -- Variables --

        protected bool disposed;

        protected ISscLogger logger;

        protected Dictionary<Type, object> repositories;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        public UnitOfWork(ISscLogger logger)
        {
            this.logger = logger;
        }

        #endregion

        #region -- Overrides --
        #endregion

        #region -- Methods --

        #region Implement IUnitOfWork

        public abstract bool Exist();

        public abstract void BeginTransaction();

        public abstract void Commit();

        public abstract void Rollback();

        public abstract void CreateSavepoint(string savePoint);

        public abstract void RollbackToSavepoint(string savePoint);

        public abstract Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        public abstract Task CommitAsync(CancellationToken cancellationToken = default);

        public abstract Task RollbackAsync(CancellationToken cancellationToken = default);

        public abstract Task CreateSavepointAsync(string savePoint, CancellationToken cancellationToken = default);

        public abstract Task RollbackToSavepointAsync(string savePoint, CancellationToken cancellationToken = default);

        public virtual IGenericRepository<TEntity> Repository<TEntity>()
            where TEntity : class
        {
            // Create if have.
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            // Create Repository type if have.
            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = CreateRepository<TEntity>();
            }

            return (IGenericRepository<TEntity>)repositories[type];
        }

        protected abstract IGenericRepository<T> CreateRepository<T>() where T : class;

        public abstract bool Save();

        public abstract Task<bool> SaveAsync(CancellationToken cancellationToken = default);

        //public abstract Task SaveAsync(CancellationToken cancellationToken = default);

        public abstract int ExecuteSqlRaw(string sql, params object[] parameters);

        public abstract int ExecuteSqlInterpolated(FormattableString sql);

        public abstract IEnumerable<T> FromSqlRaw<T>(string sql, params object[] parameters) where T : class;

        public abstract IEnumerable<T> FromSqlInterpolated<T>(FormattableString sql) where T : class;

        public abstract Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);

        public abstract Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql);

        public abstract Task<IEnumerable<T>> FromSqlRawAsync<T>(string sql, params object[] parameters) where T : class;

        public abstract Task<IEnumerable<T>> FromSqlInterpolatedAsync<T>(FormattableString sql) where T : class;

        public abstract IQueryable<T> ExecuteStore<T>(string sql, object param) where T : class;

        public abstract DataSet ExecuteSpDataSet(string sql, object param);

        public abstract DataTable ExecuteSpDataTable(string sql, object param);

        public abstract void ExecuteSp(string sql, object param);

        #endregion

        #region Implement IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Dispose data.
            if (!this.disposed)
            {
                DisposeData();
            }

            // Mark dispose.
            this.disposed = true;
        }

        protected virtual void DisposeData()
        {
            // Clear repositories
            repositories?.Clear();
        }

        #endregion

        #endregion
    }
}
