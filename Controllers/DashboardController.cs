using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;

namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboard _dashboardBo;
        public DashboardController(IDashboard dashboardBo)
        {
            _dashboardBo = dashboardBo;
        }

        [HttpGet]
        [Route("GetValueDashboard/{month}")]
        public async Task<object> GetValueDashboard(int month)
        {
            return await _dashboardBo.GetValueDashboard(month);
        }

        [HttpGet]
        [Route("GetBarChart/{take}")]
        public async Task<object> GetBarChart(int take)
        {
            return await _dashboardBo.GetBarChart(take);
        }

        [HttpGet]
        [Route("GetPieChart/{month}")]
        public async Task<object> GetPieChart(int month)
        {
            return await _dashboardBo.GetPieChart(month);
        }

        [HttpGet]
        [Route("GetPieChartService/{month}")]
        public async Task<object> GetPieChartService(int month)
        {
            return await _dashboardBo.GetPieChartService(month);
        }
    }
}
