using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SystemServiceAPI.Context;
using SystemServiceAPICore3.Domain.General.Uow;
using SystemServiceAPICore3.Logging.Interfaces;

namespace SystemServiceAPICore3.Uow
{
    /// <summary>
    /// The EssiHRPUow class
    /// </summary>
    public class UnitOfWork : GeneralEfCoreUnitOfWork<UnitOfWork>
    {
        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        public UnitOfWork(IServiceProvider serviceProvider)
            : this(serviceProvider.GetService<AppDbContext>(), serviceProvider.GetService<ISscLogger>())
        {
        }

        public UnitOfWork(DbContext context, ISscLogger logger) : base(context, logger)
        {
        }

        #endregion
    }
}

