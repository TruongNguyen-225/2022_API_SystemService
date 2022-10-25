using System;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SystemServiceAPICore3.Bo.Interface;
using SystemServiceAPICore3.Uow.Interface;

namespace SystemServiceAPICore3.Bo
{
    /// <summary>
    /// The BaseBo class
    /// </summary>
    /// <typeparam name="TDto">The type of the t dto.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="Ssc.Service.EntityBo{TDto, TEntity}" />
    public class BaseBo<TDto, TEntity> : EntityBo<TDto, TEntity>, IBaseBo<TDto>
        where TDto : class
        where TEntity : class
    {
        #region -- Variables --
        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBo{TDto, TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public BaseBo(IServiceProvider serviceProvider)
            : base(serviceProvider.GetService<IUnitOfWork<IUnitOfWork>>(), serviceProvider.GetService<IMapper>())
        {
        }

        #endregion

        #region -- Overrides --
        #endregion

        #region -- Methods --

        #region Implement IBaseBo

        public virtual IQueryable<TDto> GetQueryable(string param)
        {
            var modelQuery = GetQueryable<TEntity>();
            var dtoQueryable = mapper.ProjectTo<TDto>(modelQuery);

            return dtoQueryable;
        }

        #endregion

        #endregion
    }
}

