using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using DocumentFormat.OpenXml.Drawing;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Context;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;
using SystemServiceAPICore3.Bo;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Dto.Other;
using SystemServiceAPICore3.Utilities;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Bo
{
    public class BillTempBo : BaseBo<MonthlyTransaction_TempDto, MonthlyTransactionTemp>, IBillTempBo
    {
        #region -- Variables --

        private readonly ICustomer customerBo;
        private readonly IBillBo billBo;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        public BillTempBo(IServiceProvider serviceProvider, ICustomer customerBo, IBillBo billBo) : base(serviceProvider)
        {
            this.customerBo = customerBo;
            this.billBo = billBo;
        }

        #endregion

        #region -- Overrides --
        #endregion

        public IQueryable<MonthlyTransactionTemp> GetDataTempByMonth(int month)
        {
            int currentMonth = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            var monthlyTransactionTemp = GetQueryable<MonthlyTransactionTemp>();
            var transactionQueryable = from transaction in monthlyTransactionTemp
                                       where transaction.DateTimeAdd.Month == month && transaction.DateTimeAdd.Year == (month == 12 ? year - 1 : year)
                                       orderby transaction.RetailID descending
                                       select transaction;

            return transactionQueryable;
        }

        public async Task<Dictionary<string, int>> GetTotalBillElectric(List<string> listCode)
        {
            int money = 0;
            string code = String.Empty;
            XmlDocument xmltest = new XmlDocument();
            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (var item in listCode)
            {
                string URL = $"https://www.cskh.evnspc.vn/ThanhToanTienDien/XuLyThanhToanTrucTuyenTienDien?MaKhachHang={item}";
                string html = await GetWebContent(URL);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var element = doc.DocumentNode.SelectNodes("//div[@class='customer-info']");
                var temp = "";
                if (element != null)
                {
                    var text = element.FirstOrDefault().InnerText.Trim().Replace("\n", "").Replace("\r", "");
                    var search = "Tổng tiền:";
                    temp = text.Substring(text.IndexOf(search) + search.Length);
                    temp = temp.Split(' ')[0].Replace(".","");
                    money = DataAccess.CorrectValue(temp, 0);
                    result.Add(code, money);
                }
            }

            return await Task.FromResult(result);
        }

        // Tải về trang web và trả về chuỗi nội dung
        public static async Task<string> GetWebContent(string url)
        {
            // Khởi tạo http client
            using var httpClient = new HttpClient();

            // Thiết lập các Header nếu cần
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            try
            {
                // Thực hiện truy vấn GET
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // Phát sinh Exception nếu mã trạng thái trả về là lỗi
                response.EnsureSuccessStatusCode();

                // Đọc nội dung content trả về - ĐỌC CHUỖI NỘI DUNG
                string htmltext = await response.Content.ReadAsStringAsync();
                return htmltext;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static int GetMoneyFromHTML(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var element = doc.DocumentNode.SelectNodes("//div[@class='customer-info']");
            var temp = String.Empty;
            int result = 0;

            if (element != null)
            {
                var text = element.FirstOrDefault().InnerText.Trim().Replace("\n", "").Replace("\r", "");
                var search = "Tổng tiền:";
                temp = text.Substring(text.IndexOf(search) + search.Length);
                result = DataAccess.CorrectValue((temp.Replace(".", "").Split(' ')[0]), 0);
            }

            return result;
        }

        /// <summary>
        /// Init data from previous month
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<object> InitData(int month)
        {
            try
            {
                var unitOfWork = GetDataContext();
                await unitOfWork.BeginTransactionAsync();

                int currentMonth = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                var monthlyTransactionTemp = GetQueryable<MonthlyTransactionTemp>();
                var monthlyTransactionTempRepo = GetRepository<MonthlyTransactionTemp>();

                var transactionQueryable = GetDataTempByMonth(currentMonth);

                if (transactionQueryable.Any())
                {
                    var transactionsCurrentTemp = transactionQueryable.ToArray();
                    monthlyTransactionTempRepo.Delete(transactionsCurrentTemp);
                    monthlyTransactionTempRepo.Save();
                }

                BillFilterDto request = new BillFilterDto
                {
                    ServiceID = 1,
                    RetailID = null,
                    Month = month,
                    Year = month == 12 ? year - 1 : year
                };

                var transactions = billBo.GetTransactionByConditions(request);

                //insert data
                if (transactions.Count() > 0)
                {
                    List<string> listCode = new List<string>();

                    foreach (var item in transactions)
                    {
                        listCode.Add(item.Code);
                    }

                    Dictionary<string, int> result = await GetTotalBillElectric(listCode);

                    foreach (var item in transactions)
                    {
                        if (result.ContainsKey(item.Code))
                        {
                            item.Total = result.GetValueOrDefault(item.Code) + item.Postage;
                            item.Money = result.GetValueOrDefault(item.Code);
                            MonthlyTransactionTemp entity = mapper.Map<MonthlyTransactionTemp>(item);

                            monthlyTransactionTempRepo.Insert(entity);
                        }
                    }
                }

                await unitOfWork.CommitAsync();
                monthlyTransactionTempRepo.Save();

                return Task.FromResult(transactions);
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Thêm mới giao dịch
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<object> InsertTransactionAsync(BillInsertDto req)
        {
            var monthlyTransactionRepository = GetRepository<MonthlyTransactionTemp>();
            var target = mapper.Map<BillInsertDto, MonthlyTransactionTemp>(req);
            target.DateTimeAdd = DateTime.Now;

            await monthlyTransactionRepository.InsertAsync(target);
            await monthlyTransactionRepository.SaveAsync();

            var result = mapper.Map<MonthlyTransactionDto>(target);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Kiểm tra trước khi thêm giao dịch
        /// Với dịch vụ tiền điện, 1 tháng chỉ đóng 1 lần, nên không thể trùng code.
        /// </summary>
        /// <param name="serviceID"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> CheckBeforeAddBillElectricity(int serviceID, string code)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            var monthlyTransactionRepository = GetRepository<MonthlyTransaction>();

            var checkExisted = monthlyTransactionRepository.FindBy(x => x.Code == code
                && x.ServiceID == serviceID
                && x.DateTimeAdd.Month == month && x.DateTimeAdd.Year == year
                );

            if (checkExisted.Any())
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        /// <summary>
        /// Cập nhật giao dịch
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public object UpdateTransactionAsync(BillUpdateDto req)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            var monthlyTransactionRepository = GetRepository<MonthlyTransactionTemp>();
            var monthlyTransactionQueryable = GetQueryable<MonthlyTransactionTemp>();

            var transaction = monthlyTransactionQueryable
                .Where(x => x.ID == req.ID && x.DateTimeAdd.Month == month && x.DateTimeAdd.Year == year)
                .FirstOrDefault();

            if (transaction != null)
            {
                transaction = mapper.Map<BillUpdateDto, MonthlyTransactionTemp>(req, transaction);
                transaction.DateTimeUpdate = DateTime.Now;
                monthlyTransactionRepository.Update(transaction, true);

                return transaction;
            }

            return default(object);
        }

        /// <summary>
        /// Xoá một giao dịch bằng ID
        /// </summary>
        /// <param name="billID"></param>
        /// <returns></returns>
        public async Task<object> DeleteTransactionByBillID(int billID)
        {
            var monthlyTransactionRepository = GetRepository<MonthlyTransactionTemp>();
            var transaction = monthlyTransactionRepository.FindBy(x => x.ID == billID).FirstOrDefault();

            if (transaction != null)
            {
                monthlyTransactionRepository.Delete(transaction, true);

                return await Task.FromResult(transaction);
            }

            return await Task.FromResult(default(object));
        }

        /// <summary>
        /// Xoá nhiều giao dịch
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<object> DeleteTransactionsAsync(BillDeleteDto req)
        {
            try
            {
                var unitOfWork = GetDataContext();
                unitOfWork.BeginTransaction();

                List<string> listID = req.ListBillID.Split(';').ToList();
                int serviceID = req.ServiceID;
                int month = req.Month;
                int year = req.Year;

                var monthlyTransactionRepository = GetRepository<MonthlyTransactionTemp>();
                var queryable = monthlyTransactionRepository
                    .FindBy(x => x.ServiceID == serviceID
                                && listID.Contains(x.CustomerID.ToString())
                                && x.DateTimeAdd.Month == month
                                && x.DateTimeAdd.Year == year);

                if (queryable.Any())
                {
                    var transactions = queryable.ToList();
                    foreach (var transaction in transactions)
                    {
                        monthlyTransactionRepository.Delete(transaction, false);
                    }

                    unitOfWork.Save();
                    unitOfWork.Commit();

                    return await Task.FromResult(transactions);
                }
                unitOfWork.Commit();
                return await Task.FromResult(default(object));
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public byte[] PrintMultiRow(BillPrintTransactionsDto req)
        {
            var tblCustomer = GetQueryable<Customer>();
            var tblBillTemp = GetQueryable<MonthlyTransactionTemp>();
            var tblRetail = GetQueryable<Retail>();

            List<string> listID = req.ListBillID.Split(";").ToList();

            var listBillTemp = (from temp in tblBillTemp
                                from customer in tblCustomer.Where(x => x.CustomerID == temp.CustomerID).DefaultIfEmpty()
                                from retail in tblRetail.Where(x => x.RetailID == temp.RetailID).DefaultIfEmpty()
                                where
                                    temp.DateTimeAdd.Month == req.Month
                                    && temp.DateTimeAdd.Year == req.Year
                                    && temp.ServiceID == req.ServiceID
                                    && listID.Contains(temp.ID.ToString())
                                orderby temp.RetailID
                                select new
                                {
                                    FullName = customer.FullName,
                                    Code = customer.Code,
                                    Money = temp.Money,
                                    Postage = temp.Postage,
                                    Total = temp.Total,
                                    RetailName = retail.RetailName,
                                }).ToList();

            DataTable dataTable = Utility.ToDataTable(listBillTemp);

            DateTime? fromDate = new DateTime(req.Year, req.Month, 1);
            DateTime? toDate = new DateTime(req.Year, req.Month, 20);

            FileNameParams fileNameParams = new FileNameParams
            {
                FileName = ExportExcelConstants.FILE_NAME_LIST_ELECTRIC_BILL,
                FromDate = fromDate.ConvertDateTimeToString103(),
                ToDate = toDate.ConvertDateTimeToString103(),
                TimeExport = toDate.Value,
            };

            CellParams cellParams = new CellParams
            {
                MaxColumn = 20,
                StartColumn = 3,
                StartRow = 7,
            };

            ExcelParamDefault excelParamDefault = new ExcelParamDefault
            {
                fileNameParam = null,
                cellParam = cellParams
            };

            string pathTemplate = @"/Volumes/Data/6.Office/1.Excel/1.ExcelTemplate/TemplateElectricBill.xlsx";
            byte[] excel = ExportExcelWithEpplusHelper.LoadFileTemplate(pathTemplate, dataTable, excelParamDefault, true);

            return excel;
        }

        public byte[] PrintBillElectricInit()
        {
            var tblCustomer = GetQueryable<Customer>();
            var tblBillTemp = GetQueryable<MonthlyTransactionTemp>();
            var tblRetail = GetQueryable<Retail>();

            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int serviceID = 1;

            var listBillTemp = (from temp in tblBillTemp
                                from customer in tblCustomer.Where(x => x.CustomerID == temp.CustomerID).DefaultIfEmpty()
                                from retail in tblRetail.Where(x => x.RetailID == temp.RetailID).DefaultIfEmpty()
                                where
                                    temp.DateTimeAdd.Month == month
                                    && temp.DateTimeAdd.Year == year
                                    && temp.ServiceID == serviceID
                                    && temp.Status != 2  //2 : printed
                                orderby temp.RetailID
                                select new
                                {
                                    FullName = customer.FullName,
                                    Code = customer.Code,
                                    Money = temp.Money,
                                    Postage = temp.Postage,
                                    Total = temp.Total,
                                    RetailName = retail.RetailName,
                                }).ToList();

            DataTable dataTable = Utility.ToDataTable(listBillTemp);

            DateTime? fromDate = new DateTime(year, month, 1);
            DateTime? toDate = new DateTime(year, month, 20);

            FileNameParams fileNameParams = new FileNameParams
            {
                FileName = ExportExcelConstants.FILE_NAME_LIST_ELECTRIC_BILL,
                FromDate = fromDate.ConvertDateTimeToString103(),
                ToDate = toDate.ConvertDateTimeToString103(),
                TimeExport = toDate.Value,
            };

            CellParams cellParams = new CellParams
            {
                MaxColumn = 20,
                StartColumn = 3,
                StartRow = 7,
            };

            ExcelParamDefault excelParamDefault = new ExcelParamDefault
            {
                fileNameParam = null,
                cellParam = cellParams
            };

            string pathTemplate = @"/Volumes/Data/6.Office/1.Excel/1.ExcelTemplate/TemplateElectricBill.xlsx";
            //string pathTemplate = @"https://latex.itdvgroup.com/Report/Template/initBillTemplate.xlsx";
            byte[] excel = ExportExcelWithEpplusHelper.LoadFileTemplate(pathTemplate, dataTable, excelParamDefault, true);

            return excel;
        }

    }
}