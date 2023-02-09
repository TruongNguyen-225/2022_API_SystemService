using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Context;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.Report;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;
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
            var startDate = req.StartTime;
            var serviceID = req.ServiceID;
            var retailID = req.RetailID;
            var endDate = req.EndTime;

            var viewCustomerQueryable = customerBo.GetQueryableViewCustomer();
            var monthlyTransactionQueryable = GetQueryable<MonthlyTransaction>();

            var viewMonthlyTransactionQueryable = (from transaction in monthlyTransactionQueryable
                                                   from customer in viewCustomerQueryable.
                                                   Where(x => x.CustomerID == transaction.CustomerID).DefaultIfEmpty()
                                                   where serviceID > 6 ? true : transaction.ServiceID == serviceID
                                                   && transaction.RetailID == req.RetailID
                                                   && transaction.DateTimeAdd.Date >= startDate
                                                   && transaction.DateTimeAdd.Date <= endDate
                                                   orderby transaction.ServiceID descending
                                                   orderby transaction.DateTimeAdd ascending
                                                   select new MonthlyTransactionResponse
                                                   {
                                                       STT = 1,
                                                       ID = transaction.ID,
                                                       ServiceID = customer.ServiceID,
                                                       ServiceName = customer.ServiceName,
                                                       CustomerID = customer.CustomerID,
                                                       FullName = customer.FullName,
                                                       Code = customer.Code,
                                                       RetailID = customer.RetailID,
                                                       RetailName = customer.RetailName,
                                                       BankID = customer.BankID,
                                                       BankName = customer.BankName,
                                                       Money = transaction.Money,
                                                       Postage = transaction.Postage,
                                                       Total = transaction.Total,
                                                       Month = transaction.DateTimeAdd.Month,
                                                       Year = transaction.DateTimeAdd.Year,
                                                       DateTimeAdd = transaction.DateTimeAdd
                                                   });

            if (viewMonthlyTransactionQueryable.Any())
            {
                return await Task.FromResult(viewMonthlyTransactionQueryable.ToList());
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

            List<MonthlyTransactionResponse> data = await GetByCondition(req);
            DataTable dataTable = Utility.ToDataTable(data);

            string pathTemplate = @"https://pss.itdvgroup.com/template/TemplateExportDataReport.xlsx";

            return EpplusHelper.ExportExcel(pathTemplate, 8, 1, 11, "%", dataTable);
        }

        public byte[] TestExport()
        {
            //DateTime? fromDate = new DateTime(2022, 12, 01);
            //DateTime? toDate = DateTime.Now;

            //List<vw_MonthlyTransaction> data = _dbContext.vw_MonthlyTransactions.Where(
            //x => x.RetailID == 2 &&
            //            x.DateTimeAdd.Date >= fromDate &&
            //            x.DateTimeAdd.Date <= toDate
            //        ).OrderBy(x => x.ServiceID).OrderBy(x => x.DateTimeAdd).ToList();


            //DataTable dataTable = Utility.ToDataTable(data);

            //FileNameParams fileNameParams = new FileNameParams
            //{
            //    FileName = String.Format(ExportExcelConstants.FILE_NAME_TRANSACTION_FILE_NAME, "Cafe Nhớ", fromDate.ConvertDateTimeToString103(), toDate.ConvertDateTimeToString103()),
            //    FromDate = fromDate.ConvertDateTimeToString103(),
            //    ToDate = toDate.ConvertDateTimeToString103(),
            //    TimeExport = toDate.Value,
            //};

            //CellParams cellParams = new CellParams
            //{
            //    MaxColumn = 9,
            //    StartColumn = 1,
            //    StartRow = 7,
            //};

            //ExcelParamDefault excelParamDefault = new ExcelParamDefault
            //{
            //    fileNameParam = fileNameParams,
            //    cellParam = cellParams
            //};

            //string pathTemplate = @"/Volumes/Data/6.Office/1.Excel/1.ExcelTemplate/TemplateExportMonthlyReport.xlsx";
            //byte[] excel = ExportExcelWithEpplusHelper.LoadFileTemplate(pathTemplate, dataTable, excelParamDefault);

            return null;
        }
    }
}
