using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IBillBo
    {
        Task<List<vw_MonthlyTransaction>> GetByServiceID(int serviceID);
        Task<List<vw_MonthlyTransaction>> GetByMonth(BillFilterDto req);
        Task<MonthlyTransaction> Post(BillRequestDto req);
        Task<bool> CheckBeforeAddBillElectricity(int serviceID, string code);
        Task<ResponseResults> Put(BillUpdatetDto req);
        Task<ResponseResults> DeleteByID(int billID);
        Task<ResponseResults> DeleteMultiRow(BillDeleteDto req);
        //Task<ResponseResults> FindByCondition(BillSearchDto req);
        Task<ResponseResults> Import(int monthPrev); //chỉ dành cho tiền điện
        Task<byte[]> Print(int billID);
        Task<ResponseResults> PrintMultiRow(BillPrintAllDto req);
    }
}

