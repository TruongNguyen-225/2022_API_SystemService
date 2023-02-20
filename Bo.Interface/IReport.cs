using System.Collections.Generic;
using System.Threading.Tasks;
using SystemServiceAPI.Dto.Report;
using SystemServiceAPICore3.Dto.Other;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IReport
    {
        Task<List<MonthlyTransactionResponse>> GetByCondition(ReportRequestDto req);

        Task<object> HistoryReport();

        Task<byte[]> ExportAsync(ReportRequestDto req);
    }
}
