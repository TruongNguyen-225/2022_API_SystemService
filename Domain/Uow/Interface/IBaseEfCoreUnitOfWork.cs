using System;
using SystemServiceAPICore3.Repositories.Interfaces;

namespace SystemServiceAPICore3.Uow.Interface
{
    public interface IBaseEfCoreUnitOfWork : IUnitOfWork
    {
        IBaseEfCoreGenericRepository<TEntity> EfRepository<TEntity>() where TEntity : class;

        bool AutoDetectChange { get; set; }

        string ConnectionString { get; }
    }
}