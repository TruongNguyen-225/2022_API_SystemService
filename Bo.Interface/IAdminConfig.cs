using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IAdminConfig
    {
        object GetAllTable();
        object GetColumns(string tableName);
        object ExcuteQuery(string cmd);
        Task<object> UploadFile(IFormFile file);
    }
}