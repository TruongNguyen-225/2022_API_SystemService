using System.Threading.Tasks;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IDashboard
    {
        Task<object> GetValueDashboard(int month);
        Task<object> GetBarChart(int take);
        Task<object> GetPieChart(int month);
        Task<object> GetPieChartService(int month);
    }
}
