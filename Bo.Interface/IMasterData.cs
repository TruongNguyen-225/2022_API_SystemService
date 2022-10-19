using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IMasterData
    {
        Task<object> GetServices();
        Task<object> GetRetails();
        Task<object> GetBanks();
        Task<object> GetVillages();
    }
}
