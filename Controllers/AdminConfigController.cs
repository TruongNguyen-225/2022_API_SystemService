using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPICore3.Dto.AdminConfigDto;

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
        public async Task<object> GetAllTable()
        {
            return await _adminConfig.GetAllTable();
        }

        [HttpGet]
        [Route("GetColumns/{tableName}")]
        public async Task<object> GetColumns(string tableName)
        {
            return await _adminConfig.GetColumns(tableName);
        }

        [HttpPost]
        [Route("ExcuteQuery")]
        public async Task<object> ExcuteQuery([FromBody] AdminConfigDto req)
        {
            return await _adminConfig.ExcuteQuery(req.query);
        }
    }
}
