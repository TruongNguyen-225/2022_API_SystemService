using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemServiceAPICore3.Repositories.Interfaces;
using SystemServiceAPICore3.Repositories;
using SystemServiceAPICore3.Logging.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SystemServiceAPICore3.Uow.Interface
{
    public abstract class EfCoreUnitOfWork<TU> : UnitOfWork<TU>, IBaseEfCoreUnitOfWork
    {
        #region -- Variables --

        private DbContext context;

        private IDbContextTransaction transactionScope;

        #endregion

        #region -- Properties --

        protected DbContext Context
        {
            get => context;
            set
            {
                context = value;

                if (this.context != null && this.context.Database != null)
                {
                    ConnectionString = this.context.Database.GetDbConnection()?.ConnectionString;
                    ProviderName = this.context.Database.ProviderName;
                }
            }
        }

        #endregion

        #region -- Constructors --

        public EfCoreUnitOfWork(DbContext context, ISscLogger logger)
            : base(logger)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));

            if (this.context != null && this.context.Database != null)
            {
                ConnectionString = this.context.Database.GetDbConnection()?.ConnectionString;
                ProviderName = this.context.Database.ProviderName;
            }
        }

        #endregion

        #region -- Overrides --

        #region Implement UnitOfWork

        public override bool Exist()
        {
            return Context != null && (Context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
        }

        public override void BeginTransaction()
        {
            if (Context.Database.CurrentTransaction == null)
            {
                transactionScope = Context.Database.BeginTransaction();
            }
        }

        public override void Commit()
        {
            if (transactionScope != null)
            {
                transactionScope.Commit();
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        public override void Rollback()
        {
            if (transactionScope != null)
            {
                transactionScope.Rollback();
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        //public override void CreateSavepoint(string savePoint = "default")
        //{
        //    if (transactionScope != null)
        //    {
        //        transactionScope.CreateSavepoint(savePoint);
        //    }
        //}

        //public override void RollbackToSavepoint(string savePoint = "default")
        //{
        //    if (transactionScope != null && !string.IsNullOrEmpty(savePoint))
        //    {
        //        transactionScope.RollbackToSavepoint(savePoint);
        //        transactionScope.Dispose();
        //        transactionScope = null;
        //    }
        //}

        public override async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (Context.Database.CurrentTransaction == null)
            {
                transactionScope = await Context.Database.BeginTransactionAsync(cancellationToken);
            }
        }

        public override async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (transactionScope != null)
            {
                await transactionScope.CommitAsync(cancellationToken);
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        public override async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (transactionScope != null)
            {
                await transactionScope.RollbackAsync(cancellationToken);
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        //public override async Task CreateSavepointAsync(string savePoint = "default", CancellationToken cancellationToken = default)
        //{
        //    if (transactionScope != null)
        //    {
        //        await transactionScope.CreateSavepointAsync(savePoint, cancellationToken);
        //    }
        //}

        //public override async Task RollbackToSavepointAsync(string savePoint = "default", CancellationToken cancellationToken = default)
        //{
        //    if (transactionScope != null && !string.IsNullOrEmpty(savePoint))
        //    {
        //        await transactionScope.RollbackToSavepointAsync(savePoint, cancellationToken);
        //        transactionScope.Dispose();
        //        transactionScope = null;
        //    }
        //}

        public override bool Save()
        {
            try
            {
                return (Context.SaveChanges() >= 0);
            }
            catch (Exception e)
            {
                // Handle error.
                HandleError(e);

                throw;
            }
        }

        public override async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await Context.SaveChangesAsync(cancellationToken);
                return (result >= 0);
            }
            catch (Exception e)
            {
                // Handle error.
                HandleError(e);

                throw;
            }
        }

        //public override async Task SaveAsync(CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        await context.SaveChangesAsync(cancellationToken);
        //    }
        //    catch (Exception e)
        //    {
        //        // Handle error.
        //        HandleError(e);

        //        throw;
        //    }
        //}

        protected override IGenericRepository<T> CreateRepository<T>()
        {
            return new EfCoreGenericRepository<T>(Context);
        }

        protected override void DisposeData()
        {
            base.DisposeData();

            // Compete and dispose transaction.
            if (transactionScope != null)
            {
                transactionScope.Commit();
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        #endregion

        #region Implement IBaseEfCoreUnitOfWork

        public IBaseEfCoreGenericRepository<TEntity> EfRepository<TEntity>() where TEntity : class
        {
            return Repository<TEntity>() as IBaseEfCoreGenericRepository<TEntity>;
        }

        public bool AutoDetectChange
        {
            get => this.Context.ChangeTracker.AutoDetectChangesEnabled;
            set => this.Context.ChangeTracker.AutoDetectChangesEnabled = value;
        }

        public string ConnectionString { get; protected set; }

        public string ProviderName { get; protected set; }

        public override int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return parameters == null ? Context.Database.ExecuteSqlRaw(sql) : Context.Database.ExecuteSqlRaw(sql, parameters);
        }

        public override int ExecuteSqlInterpolated(FormattableString sql)
        {
            return Context.Database.ExecuteSqlInterpolated(sql);
        }

        public override IEnumerable<T> FromSqlRaw<T>(string sql, params object[] parameters) where T : class
        {
            return parameters == null ? Context.Set<T>().FromSqlRaw(sql) : Context.Set<T>().FromSqlRaw(sql, parameters);
        }

        public override IEnumerable<T> FromSqlInterpolated<T>(FormattableString sql) where T : class
        {
            return Context.Set<T>().FromSqlInterpolated(sql);
        }

        public override async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return parameters == null ? await Context.Database.ExecuteSqlRawAsync(sql) : await Context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public override async Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql)
        {
            return await Context.Database.ExecuteSqlInterpolatedAsync(sql);
        }

        public override async Task<IEnumerable<T>> FromSqlRawAsync<T>(string sql, params object[] parameters) where T : class
        {
            return parameters == null ? await Context.Set<T>().FromSqlRaw(sql).ToListAsync() : await Context.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }

        public override async Task<IEnumerable<T>> FromSqlInterpolatedAsync<T>(FormattableString sql) where T : class
        {
            return await Context.Set<T>().FromSqlInterpolated(sql).ToListAsync();
        }

        public override IQueryable<T> ExecuteStore<T>(string sql, object param) where T : class
        {
            // Get execution params.
            var exeParam = GetExecutionParams(sql, param, (int)CommandType.StoredProcedure);

            // Execute store.
            return Context.Set<T>().FromSqlRaw(exeParam.Item1, exeParam.Item2);
        }

        #endregion

        #endregion

        #region -- Methods --

        protected virtual void HandleError(Exception ex)
        {
        }

        protected abstract Tuple<string, object[]> GetExecutionParams(string sql, object param, int? type);

        protected abstract object[] GetExecutionParams(object param);

        #endregion
    }
}
