using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPICore3.Controllers;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class DashboardController : BaseController<CustomerDto>
    {
        private readonly IDashboard _dashboardBo;
        public DashboardController(IServiceProvider serviceProvider, IDashboard dashboardBo) :base(serviceProvider)
        {
            _dashboardBo = dashboardBo;
        }

        [HttpGet]
        [Route("GetValueDashboard/{month}")]
        public async Task<object> GetValueDashboard(int month)
        {
            try
            {
                var result = await _dashboardBo.GetValueDashboard(month, null);
                
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
        [Route("GetBarChart/{take}")]
        public async Task<object> GetBarChart(int take)
        {
            try
            {
                var result = await _dashboardBo.GetBarChart(take, null, null);

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
        [Route("GetPieChart/{month}")]
        public async Task<object> GetPieChart(int month)
        {
            try
            {
                var result = await _dashboardBo.GetPieChart(month, null);

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

        //[HttpGet]
        //[Route("GetPieChartService/{month}")]
        //public async Task<object> GetPieChartService(int month)
        //{
        //    return await _dashboardBo.GetPieChartService(month);
        //}
    }
}
