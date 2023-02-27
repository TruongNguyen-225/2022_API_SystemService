using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.Report;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Bo;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Dto.Other;
using SystemServiceAPICore3.Utilities;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Bo
{
    public class ReportBo :BaseBo<CustomerDto, Customer>, IReport
    {
        private readonly IBillBo billBo;
        private readonly ICustomer customerBo;

        public ReportBo(IServiceProvider serviceProvider, IBillBo billBo, ICustomer customerBo) : base(serviceProvider)
        {
            this.billBo = billBo;
            this.customerBo = customerBo;
        }

        public async Task<List<MonthlyTransactionResponse>> GetByCondition(ReportRequestDto req)
        {
            var startDate = new DateTime(req.StartTime.Year, req.StartTime.Month, req.StartTime.Day);
            var endDate = new DateTime(req.EndTime.Year, req.EndTime.Month, req.EndTime.Day).AddDays(1).AddSeconds(-1);
            var serviceID = req.ServiceID;
            var retailID = req.RetailID;

            //var viewCustomerQueryable = customerBo.GetQueryableViewCustomer();
            var monthlyTransactionQueryable = GetQueryable<MonthlyTransaction>();
            var customerQueryable = GetQueryable<Customer>();
            var bankQueryable = GetQueryable<Bank>();
            var retailQueryable = GetQueryable<Retail>();
            var serviceQueryable = GetQueryable<Service>();

            var viewMonthlyTransactionQueryable = (from transaction in monthlyTransactionQueryable
                                                   from customer in customerQueryable.Where(x => x.CustomerID == transaction.CustomerID).DefaultIfEmpty().Where(x => !x.IsDelete)
                                                   from retail in retailQueryable.Where(x => x.RetailID == transaction.RetailID).DefaultIfEmpty()
                                                   from service in serviceQueryable.Where(x => x.ServiceID == transaction.ServiceID).DefaultIfEmpty()
                                                   from bank in bankQueryable.Where(x => x.BankID == transaction.BankID).DefaultIfEmpty()
                                                   where transaction.RetailID == retailID
                                                   && transaction.DateTimeAdd.Date >= startDate
                                                   && transaction.DateTimeAdd.Date <= endDate
                                                   && (serviceID > 6 ? true : transaction.ServiceID == serviceID)
                                                   select new MonthlyTransactionResponse
                                                   {
                                                       STT = 1,
                                                       ID = transaction.ID,
                                                       ServiceID = transaction.ServiceID,
                                                       ServiceName = service.ServiceName,
                                                       CustomerID = transaction.CustomerID,
                                                       FullName = customer.FullName,
                                                       Code = transaction.Code,
                                                       RetailID = transaction.RetailID,
                                                       RetailName = retail.DeputizeName,
                                                       BankID = customer.BankID,
                                                       BankName = bank.ShortName,
                                                       Money = transaction.Money,
                                                       Postage = transaction.Postage,
                                                       Total = transaction.ServiceID == 5 ? transaction.Postage - transaction.Money : transaction.Total,
                                                       Month = transaction.DateTimeAdd.Month,
                                                       Year = transaction.DateTimeAdd.Year,
                                                       DateTimeAdd = transaction.DateTimeAdd
                                                   });

            if (viewMonthlyTransactionQueryable.Any())
            {
                return await Task.FromResult(viewMonthlyTransactionQueryable.OrderByDescending(x => x.ServiceID).OrderByDescending(x => x.DateTimeAdd).ToList());
            }

            return await Task.FromResult(new List<MonthlyTransactionResponse>());
        }

        public async Task<object> HistoryReport()
        {
            var tblHistory = GetQueryable<HistoryExportData>();
            var histories = tblHistory.Where(x => x.DateTimeAdd.Value.Year == DateTime.Now.Year).OrderByDescending(x => x.DateTimeAdd);

            if (histories.Any())
            {
                return await Task.FromResult(histories.ToList());
            }

            return Task.FromResult(default(object));
        }

        public async Task<byte[]> ExportAsync(ReportRequestDto req)
        {
            var startDate = req.StartTime;
            var serviceID = req.ServiceID;
            var retailID = req.RetailID;
            var endDate = req.EndTime;

            string startTimeString = DateTimeHelper.ConvertDateTimeToString103(startDate);
            string endTimeString = DateTimeHelper.ConvertDateTimeToString103(endDate);

            List<MonthlyTransactionResponse> data = await GetByCondition(req);
            DataTable dataTable = Utility.ToDataTable(data);

            var tblRetail = GetQueryable<Retail>();
            string retailName = tblRetail.Where(x => x.RetailID == retailID).Select(x => x.RetailName).FirstOrDefault();

            string pathTemplate = @"https://pss.itdvgroup.com/template/TemplateExportDataReport_V1.xlsx";

            FileNameParams fileNameParams = new FileNameParams
            {
                FileName = String.Format(ExportExcelConstants.FILE_NAME_TRANSACTION_FILE_NAME, retailName.ToUpper(), startTimeString, endTimeString),
                FromDate = startTimeString,
                ToDate = endTimeString,
                TimeExport = endDate,
            };

            CellParams cellParams = new CellParams
            {
                MaxColumn = 10,
                StartColumn = 1,
                StartRow = 8,
            };

            ExcelParamDefault excelParamDefault = new ExcelParamDefault
            {
                fileNameParam = fileNameParams,
                cellParam = cellParams
            };

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient wc = new HttpClient(clientHandler))
            {
                Stream stream = await wc.GetStreamAsync(pathTemplate);
                byte[] excel = ExportExcelWithEpplusHelper.LoadFileTemplate(stream, dataTable, excelParamDefault, false);
                
                return excel;
            }
        }
    }
}
