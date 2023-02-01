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
using SystemServiceAPI.Entities.View;
using SystemServiceAPICore3.Utilities;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Bo
{
    public class ReportBo :IReport
    {
        private readonly AppDbContext _dbContext;

        public ReportBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<ResponseResults> GetByCondition(ReportRequestDto req)
        {
            ResponseResults response = new ResponseResults();
            try
            {
                List<vw_MonthlyTransaction> data = new List<vw_MonthlyTransaction>();

                if(req.ServiceID > 6)
                {
                    data = _dbContext.vw_MonthlyTransactions.Where(
                        x => x.RetailID == req.RetailID &&
                        x.DateTimeAdd.Date >= req.StartTime.Date  &&
                        x.DateTimeAdd.Date <= req.EndTime.Date
                    ).OrderBy(x => x.ServiceID).OrderBy(x => x.DateTimeAdd).ToList();
                }
                else
                {
                    data = _dbContext.vw_MonthlyTransactions.Where(
                        x => x.ServiceID == req.ServiceID &&
                        x.RetailID == req.RetailID &&
                        x.DateTimeAdd.Date >= req.StartTime.Date &&
                        x.DateTimeAdd.Date <= req.EndTime.Date
                    ).OrderBy(x => x.ServiceID).OrderBy(x => x.DateTimeAdd).ToList();
                }
                

                if(data.Count > 0)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = data;
                    response.Msg = "SUCCESS";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Result = null;
                    response.Msg = "NOT FOUND";
                }

            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }

        public async Task<ResponseResults> HistoryReport()
        {
            ResponseResults response = new ResponseResults();
            try
            {
                var data = _dbContext.HistoryExportData.ToList();

                if (data.Count > 0)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = data;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }

        public byte[] Export(ExportDto req)
        {
            DataTable dataTable = new DataTable();
            List<string> listID = req.listID.Split(';').ToList();

            var data = _dbContext.vw_MonthlyTransactions.Where(x => listID.Contains(x.ID.ToString()) && x.RetailID == req.retailID).ToList();
            dataTable = Utility.ToDataTable(data);
            string pathTemplate = @"https://pss.itdvgroup.com/template/TemplateExportDataReport.xlsx";

            return EpplusHelper.ExportExcel(pathTemplate, 8, 1, 11, "%", dataTable);
        }

        public byte[] TestExport()
        {
            DateTime? fromDate = new DateTime(2022, 12, 01);
            DateTime? toDate = DateTime.Now;

            List<vw_MonthlyTransaction> data = _dbContext.vw_MonthlyTransactions.Where(
            x => x.RetailID == 2 &&
                        x.DateTimeAdd.Date >= fromDate &&
                        x.DateTimeAdd.Date <= toDate
                    ).OrderBy(x => x.ServiceID).OrderBy(x => x.DateTimeAdd).ToList();


            DataTable dataTable = Utility.ToDataTable(data);

            FileNameParams fileNameParams = new FileNameParams
            {
                FileName = String.Format(ExportExcelConstants.FILE_NAME_TRANSACTION_FILE_NAME, "Cafe Nhớ", fromDate.ConvertDateTimeToString103(), toDate.ConvertDateTimeToString103()),
                FromDate = fromDate.ConvertDateTimeToString103(),
                ToDate = toDate.ConvertDateTimeToString103(),
                TimeExport = toDate.Value,
            };

            CellParams cellParams = new CellParams
            {
                MaxColumn = 9,
                StartColumn = 1,
                StartRow = 7,
            };

            ExcelParamDefault excelParamDefault = new ExcelParamDefault
            {
                fileNameParam = fileNameParams,
                cellParam = cellParams
            };

            string pathTemplate = @"/Volumes/Data/6.Office/1.Excel/1.ExcelTemplate/TemplateExportMonthlyReport.xlsx";
            byte[] excel = ExportExcelWithEpplusHelper.LoadFileTemplate(pathTemplate, dataTable, excelParamDefault);

            return excel;
        }
    }
}
