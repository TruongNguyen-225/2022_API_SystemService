using System.Data;
using System.Threading.Tasks;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IAdminConfig
    {
        Task<object> GetAllTable();
        Task<object> GetColumns(string tableName);
        Task<object> ExcuteQuery(string cmd);
    }
}
