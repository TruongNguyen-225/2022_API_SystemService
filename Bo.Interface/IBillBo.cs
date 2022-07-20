using System;
using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.View;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IBillBo
    {
        Task<object> GetByServiceID(int serviceID);
        Task<object> GetByMonth(BillFilterDto req);
        Task<ResponseResults> Post(BillRequestDto req);
        Task<ResponseResults> Put(BillUpdatetDto req);
        Task<ResponseResults> DeleteByID(int billID);
        Task<ResponseResults> DeleteMultiRow(BillDeleteDto req);
        //Task<ResponseResults> FindByCondition(BillSearchDto req);
        Task<ResponseResults> Import(int monthPrev); //chỉ dành cho tiền điện
        Task<ResponseResults> Print(int billID);
        Task<ResponseResults> PrintMultiRow(BillPrintAllDto req);
    }
}

