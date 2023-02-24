using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Controllers;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Dto.AdminConfigDto;
using SystemServiceAPICore3.Entities.Table;
using SystemServiceAPICore3.Utilities.Constants;
namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class ConfigController : BaseController<ConfigPriceDto>
    {
        #region -- Variables --

        private readonly IConfigBo configBo;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ConfigController(IServiceProvider serviceProvider, IConfigBo configBo)
            : base(serviceProvider)
        {
            this.configBo = configBo;
        }

        #endregion

        /// <summary>
        /// Danh sách cấu hình cước phí
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetConfigPrice")]
        public async Task<object> GetConfigPrice()
        {
            try
            {
                var result = await configBo.GetConfigPrice();

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Thêm mới cấu hình cước phí
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddConfigPrice")]
        public async Task<object> AddConfigPrice([FromBody] ConfigPrice entity)
        {
            try
            {
                var result = await configBo.AddConfigPrice(entity);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Cập nhật giá cước phí
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateConfigPrice")]
        public async Task<object> UpdateConfigPrice([FromBody] ConfigPriceRequest request)
        {
            try
            {
                int configID = request.ConfigID;
                int postage = request.Postage;

                if (configID > 0 && postage > 0)
                {
                    var result = await configBo.UpdateConfigPrice(configID, postage);

                    return Ok(new
                    {
                        Result = result,
                        Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                    });
                }

                return BadRequest();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Danh sách màn hình
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetScreen")]
        public async Task<object> GetScreen()
        {
            try
            {
                var result = await configBo.GetScreen();

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Danh sách hạn mức
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLevel")]
        public async Task<object> GetLevel()
        {
            try
            {
                var result = await configBo.GetLevel();

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Danh sách file upload
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFileUpload")]
        public async Task<object> GetFileUpload()
        {
            try
            {
                var result = await configBo.GetFileUpload();

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("UploadFileTemplate")]
        public async Task<object> UploadFileTemplate(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var x = await configBo.UploadFile(file);

                    return x;
                }

                return BadRequest();
            }
            catch
            {
                throw;
            }
        }

        #region ---- Account ----

        /// <summary>
        /// Danh sách tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAccount")]
        public async Task<object> GetAccount()
        {
            try
            {
                var result = await configBo.GetAccount();

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Chi tiết tài khoản theo accountID
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAccountByID/{accountID}")]
        public async Task<object> GetAccountByID(int accountID)
        {
            try
            {
                var result = await configBo.GetAccountByID(accountID);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Cập nhật hoặc thêm mới tài khoản
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateAccount")]
        public async Task<object> UpdateAccount([FromBody] Account request)
        {
            try
            {
                var result = await configBo.UpdateAccount(request);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        [HttpPut]
        [Route("DeleteAccount/{accountID}")]
        public async Task<object> DeleteAccount(int accountID)
        {
            try
            {
                var result = await configBo.DeleteAccount(accountID);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}