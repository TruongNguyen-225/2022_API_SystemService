using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.Report;
using SystemServiceAPI.Helpers;

namespace SystemServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReport _reportBo;
        public ReportController(IReport reportBo)
        {
            _reportBo = reportBo;
        }

        /// <summary>
        /// Filter data by conditions
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetByCondition")]
        public async Task<object> GetByCondition(ReportRequestDto req)
        {
            return await _reportBo.GetByCondition(req);
        }
    }
}
