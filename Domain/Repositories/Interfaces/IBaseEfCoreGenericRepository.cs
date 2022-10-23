using System;
using System.Linq;

namespace SystemServiceAPICore3.Repositories.Interfaces
{
    public interface IBaseEfCoreGenericRepository<T> : IGenericRepository<T> where T : class
    {
        bool AsNoTracking { get; set; }

        IQueryable<T> AsQueryable();

        IQueryable<T> FromSqlRaw(string sql, params object[] parameters);

        IQueryable<T> FromSqlInterpolated(FormattableString sql);
    }
}

