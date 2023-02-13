using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SystemServiceAPICore3.Bo.Interface;
using SystemServiceAPICore3.Domain.Helpers;
using SystemServiceAPICore3.Logging.Interfaces;

namespace SystemServiceAPICore3.Controllers
{
    public abstract class ApiControllerBase<TDto> : ControllerBase
        where TDto : class
    {
        #region -- Variables --

        protected readonly ISscLogger logger;
        protected readonly IEntityBo<TDto> entityBo;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        public ApiControllerBase(IServiceProvider serviceProvider)
        {
            this.logger = serviceProvider.GetService<ISscLogger>();
            this.entityBo = serviceProvider.GetService<IEntityBo<TDto>>();
        }

        #endregion

        #region -- Actions --

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<TDto> GetAsync(string id)
        {
            try
            {
                // Get define type property.
                var objId = EntityHelper.GetActualKeys<TDto>(id.Split('^'));
                if (objId != null && objId.Length > 0)
                {
                    // Get entity by ID.
                    var boObject = GetBoOject();
                    return await boObject.GetAsync(objId);
                }

                return null;
            }
            catch (Exception ex)
            {
                // Throw web api exception.
                HandleException(ex, () => " id = " + id);
                throw;
            }
        }

        [HttpGet]
        [Route("")]
        public virtual async Task<TDto[]> GetListAsync()
        {
            try
            {
                var boObject = GetBoOject();
                var result = await boObject.GetListAsync();
                return result.ToArray();
            }
            catch (Exception ex)
            {
                // Throw web api exception.
                HandleException(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<TDto> CreateAsync([FromBody] TDto dto)
        {
            try
            {
                var boObject = GetBoOject();
                return await boObject.InsertAsync(dto);
            }
            catch (Exception ex)
            {
                // Throw web api exception.
                HandleException(ex, () => EntityHelper.GetNameValue(dto));
                throw;
            }
        }

        [HttpPost]
        [Route("Multi")]
        public virtual async Task<TDto[]> CreateAsync([FromBody] TDto[] dtos)
        {
            try
            {
                var boObject = GetBoOject();
                return await boObject.InsertAsync(dtos);
            }
            catch (Exception ex)
            {
                // Throw web api exception.
                HandleException(ex, () => EntityHelper.GetNameValue(dtos));
                throw;
            }
        }

        [HttpPut]
        [Route("")]
        public virtual async Task<TDto> UpdateAsync([FromBody] TDto dto)
        {
            try
            {
                // Get entity by ID.
                var boObject = GetBoOject();
                return await boObject.UpdateAsync(dto);
            }
            catch (Exception ex)
            {
                // Throw web api exception.
                HandleException(ex, () => EntityHelper.GetNameValue(dto));
                throw;
            }
        }

        [HttpPut]
        [Route("Multi")]
        public virtual async Task<TDto[]> UpdateAsync([FromBody] TDto[] dtos)
        {
            try
            {
                // Get entity by ID.
                var boObject = GetBoOject();
                return await boObject.UpdateAsync(dtos);
            }
            catch (Exception ex)
            {
                // Throw web api exception.
                HandleException(ex, () => EntityHelper.GetNameValue(dtos));
                throw;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task<bool> DeleteAsync(string id)
        {
            try
            {
                // Get define type property.
                var objId = EntityHelper.GetActualKeys<TDto>(id.Split('^'));
                if (objId != null && objId.Length > 0)
                {
                    // Delete entity by key.
                    var boObject = GetBoOject();
                    return await boObject.DeleteAsync(objId);
                }

                return false;
            }
            catch (Exception ex)
            {
                // Throw web api exception.
                HandleException(ex, () => $" id = {id}");
                throw;
            }
        }

        [HttpGet]
        [Route("Count")]
        public virtual async Task<int> CountAsync()
        {
            try
            {
                // Get entity by ID.
                var boObject = GetBoOject();
                return await boObject.CountAsync();
            }
            catch (Exception ex)
            {
                // Throw web api exception.
                HandleException(ex);
                throw;
            }
        }

        //[HttpGet]
        //[Route("paging/{pageNo}/{pageSize}")]
        //public virtual async Task<TDto[]> GetListPagedAsync(int pageNo, int pageSize)
        //{
        //    try
        //    {
        //        // Get entity by ID.
        //        var boObject = GetBoOject();
        //        var result = await boObject.GetListPagedAsync(null, pageNo, pageSize);
        //        return result.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Throw web api exception.
        //        HandleException(ex);
        //        throw;
        //    }
        //}

        #endregion

        #region -- Methods --

        protected virtual Exception HandleException(Exception ex, Func<string> message = null, [CallerMemberName] string method = "")
        {
            try
            {
                // Get log message.
                var msg = message != null ? message() : string.Empty;

                // Log exception
                logger?.Log(LogLevel.Error, ex, method + " : " + msg);
            }
            catch (Exception e)
            {
                logger?.Log(LogLevel.Error, e, e.Message);
            }

            // Wrapper handle exception.
            return ex;
        }

        protected virtual IEntityBo<TDto> GetBoOject()
        {
            return entityBo;
        }

        #endregion
    }
}