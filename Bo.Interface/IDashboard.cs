using System.Threading.Tasks;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IDashboard
    {
        Task<object> GetValueDashboard(int? month, int? year);
        Task<object> GetBarChart(int take, int? month, int? year);
        Task<object> GetPieChart(int? month, int? year);

        //Task<object> GetPieChartService(int month);
    }
}
