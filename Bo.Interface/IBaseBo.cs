using System;
using System.Linq;

namespace SystemServiceAPICore3.Bo.Interface
{
    /// <summary>
    /// The IBaseBo interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Ssc.Service.IEntityBo{T}" />
    public interface IBaseBo<T> : IEntityBo<T>
        where T : class
    {
        IQueryable<T> GetQueryable(string param);
    }
}

