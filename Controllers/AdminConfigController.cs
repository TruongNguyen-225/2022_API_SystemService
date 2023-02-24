using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPICore3.Dto.AdminConfigDto;
using SystemServiceAPICore3.Utilities.Constants;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class AdminConfigController : ControllerBase
    {
        private readonly IAdminConfig _adminConfig;
        public AdminConfigController(IAdminConfig adminConfig)
        {
            _adminConfig = adminConfig;
        }

        [HttpGet]
        [Route("GetAllTable")]
        public object GetAllTable()
        {
            try
            {
                var result = _adminConfig.GetAllTable();

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

        [HttpGet]
        [Route("GetColumns/{tableName}")]
        public object GetColumns(string tableName)
        {
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    var result = _adminConfig.GetColumns(tableName);

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

        [HttpPost]
        [Route("ExcuteQuery")]
        public object ExcuteQuery([FromBody] AdminConfigDto req)
        {
            try
            {
                string query = req.query;

                if (!string.IsNullOrEmpty(query))
                {
                    var result = _adminConfig.ExcuteQuery(req.query);

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

        [HttpPost]
        [Route("UploadFileTemplate")]
        public async Task<object> UploadFileTemplate(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var x = await _adminConfig.UploadFile(file);

                    return x;
                }

                return BadRequest();
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetPath")]
        public async Task<object> GetPath()
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "FileDownloaded", "xxxx.xlsx");
                return path;
            }
            catch
            {
                throw;
            }
        }
    }
}
