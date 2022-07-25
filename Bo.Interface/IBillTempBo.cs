using System;
using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.View;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IBillTempBo
    {
        Task<object> GetData();
        Task<ResponseResults> Post(BillRequestDto req);
        Task<ResponseResults> Put(BillUpdatetDto req);
        Task<ResponseResults> DeleteByID(int billID);
        Task<ResponseResults> DeleteMultiRow(BillDeleteDto req);
        Task<ResponseResults> Import(int month);
        Task<ResponseResults> PrintMultiRow(BillPrintAllDto req);
    }
}

