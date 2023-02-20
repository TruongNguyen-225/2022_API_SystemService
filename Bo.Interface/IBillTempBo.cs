﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.Table;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IBillTempBo
    {
        IQueryable<MonthlyTransactionTemp> GetDataTempByMonth(int? month);

        Task<bool> CheckBeforeAddBillElectricity(int serviceID, string code);

        Task<object> InsertTransactionAsync(BillInsertDto req);

        object UpdateTransactionAsync(BillUpdateDto req);

        Task<object> DeleteTransactionByBillID(int billID);

        Task<object> DeleteTransactionsAsync(BillDeleteDto req);

        Task<object> InitData(int month);

        byte[] PrintMultiRow(BillPrintTransactionsDto req);

        Task<byte[]> PrintBillElectricInit();

        Task<Dictionary<string, int>> GetTotalBillElectric(List<string> code);
    }
}

