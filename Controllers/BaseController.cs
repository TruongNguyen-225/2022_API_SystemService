//******************************************************************************
//  ［File description］
//    BaseController<T>
//  ［Note］
//    BaseController<T> class
//
//  ［History］
//------------------------------------------------------------------------------
//  Number |  Date    | Name      | Comment
//------------------------------------------------------------------------------
//  1 : 2022 : SSC : Initial version (Auto Gen)
//
//  Copyright (C) FIS Corporation. 2022. All rights reserved.
//******************************************************************************

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SystemServiceAPICore3.Bo.Interface;

namespace SystemServiceAPICore3.Controllers
{
    /// <summary>
    /// The BaseController class
    /// </summary>
    public abstract class BaseController<T> : ApiControllerBase<T>
        where T : class
    {
        #region -- Variables --
        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        protected BaseController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        #endregion

        #region -- Actions --

        //[HttpGet]
        //[Route("GetList")]
        //public async Task<object> GetList(CustomDataSourceLoadOptions loadOptions)
        //{
        //    return await GetDevExtremeListAsync(loadOptions);
        //}

        //#endregion

        //#region -- Methods --

        //protected virtual async Task<object> GetDevExtremeListAsync(CustomDataSourceLoadOptions loadOptions, string param = "")
        //{
        //    // Get queryable.
        //    var queryable = GetQueryable(param);

        //    //
        //    var data = await DataSourceLoader.LoadAsync(queryable, loadOptions);

        //    //
        //    return await Task.FromResult(data);
        //}

        protected virtual IQueryable<T> GetQueryable(string param)
        {
            var bo = GetBoOject() as IBaseBo<T>;
            if (bo != null)
            {
                // Get query able from bo.
                return bo.GetQueryable(param);
            }

            // return empty queryable.
            return Array.Empty<T>().AsQueryable();
        }

        #endregion
    }
}