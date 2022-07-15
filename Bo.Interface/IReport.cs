using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.Report;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IReport
    {
        Task<ResponseResults> GetByCondition(ReportRequestDto req);
    }
}
