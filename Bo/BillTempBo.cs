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
            foreach (var item in listCode)
            {
                code = item;
                string URL = $"https://www.cskh.evnspc.vn/ThanhToanTienDien/XuLyThanhToanTrucTuyenTienDien?MaKhachHang={code}";
                string html = await GetWebContent(URL);
                //xmltest.LoadXml(html);

                //XmlNodeList elemlist = xmltest.GetElementsByTagName("p");
                //string result = elemlist[0].InnerXml;
            }

            //string html = @"<div>
            //                <div>
            //                        <p>
            //                            Id hóa đơn: 1194407175
            //                            Loại hóa đơn: Tiền điện
            //                            Tiền nợ: 163.664 đồng
            //                            Thuế nợ: 16.366 đồng
            //                            Tổng tiền: 180.030 đồng
            //                        </p>
            //                </div>
            //                <div>
            //                    <p ><span>Chọn nhà cung cấp</span></p>
            //                    <ul>
            //                        <li><a></a></li>
            //                    </ul>
            //                </div>
            //            </div>";

            //string html = "<item><name>wrench</name></item>";
            //xmltest.LoadXml(html);

            //XmlNodeList elemlist = xmltest.GetElementsByTagName("p");
            //string result = elemlist[0].InnerXml;

            return new Dictionary<string, int>();
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

                    await unitOfWork.CommitAsync();
                    monthlyTransactionTempRepo.Save();

                    return Task.FromResult(default(object));
                }
                else
                {
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

                        foreach(var item in transactions)
                        {
                            listCode.Add(item.Code);
                            //monthlyTransactionTempRepo.Insert(item);
                        }

                        Dictionary<string, int> result = await GetTotalBillElectric(listCode);

                        foreach (var item in transactions)
                        {
                            if (result.ContainsKey(item.Code))
                            {
                                item.Total = result.GetValueOrDefault(item.Code) + item.Postage;
                                item.Money = result.GetValueOrDefault(item.Code);
                            }
                        }
                    }

                    await unitOfWork.CommitAsync();
                    monthlyTransactionTempRepo.Save();

                    return Task.FromResult(transactions);
                }
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
    }
}