using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IMasterData
    {
        Task<ResponseResults> GetServices();
        Task<ResponseResults> GetRetails();
        Task<ResponseResults> GetBanks();
        Task<ResponseResults> GetVillages();
    }
}
