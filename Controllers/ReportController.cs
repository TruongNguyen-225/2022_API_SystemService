using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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

        /// <summary>
        /// Export data to excel template
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Export")]
        public async Task<FileResult> Export()
        {
            DateTime startDate = new DateTime(2023, 02, 01);
            DateTime endDate = new DateTime(2023, 02, 28);

            ReportRequestDto req = new ReportRequestDto
            {
                ServiceID = 100,
                RetailID = 2,
                StartTime = startDate,
                EndTime = endDate
            };

            byte[] res = await _reportBo.ExportAsync(req);
            if (res != null)
            {
                return File(res.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
            }

            return null;
        }

        [HttpGet]
        [Route("TestExport")]
        public FileResult TestExport()
        {
            byte[] res = _reportBo.TestExport();

            if (res != null)
            {
                return File(res.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
            }

            return null;
        }
    }
}
