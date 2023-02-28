using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.Report;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            try
            {
                var result = await _reportBo.GetByCondition(req);

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
        /// Export data to excel template
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Export")]
        public async Task<object> Export([FromBody] ReportRequestDto req)
        {
            try
            {
                byte[] res = await _reportBo.ExportAsync(req);

                if (res != null)
                {
                    return File(res.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }

                return NotFound();
            }
            catch
            {
                throw;
            }
        }
    }
}
