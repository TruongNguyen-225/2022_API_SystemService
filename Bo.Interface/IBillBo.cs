using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;
using SystemServiceAPICore3.Dto.Other;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IBillBo
    {
        IQueryable<MonthlyTransactionResponse> GetQueryableViewMonthlyTransaction(int? month, int? year);

        Task<object> GetTransactionByServiceID(int serviceID);

        List<MonthlyTransactionResponse> GetTransactionByConditions(BillFilterDto req);

        Task<object> InsertTransactionAsync(BillInsertDto req);

        Task<bool> CheckBeforeAddBillElectricity(int serviceID, string code);

        object UpdateTransactionAsync(BillUpdateDto req);

        Task<object> DeleteTransactionByBillID(int billID);

        Task<object> DeleteTransactionsAsync(BillDeleteDto req);

        //Task<ResponseResults> FindByCondition(BillSearchDto req);

        Task<object> Import(int monthPrev); //chỉ dành cho tiền điện

        Task<byte[]> Print(int billID);

        byte[] PrintMultiRow(BillPrintTransactionsDto req);
    }
}

