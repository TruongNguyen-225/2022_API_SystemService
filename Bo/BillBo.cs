using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Bo;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Dto.Other;
using SystemServiceAPICore3.Utilities;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Bo
{
    public class BillBo : BaseBo<MonthlyTransactionDto, MonthlyTransaction>, IBillBo
    {
        #region -- Variables --

        private readonly ICustomer customerBo;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        public BillBo(IServiceProvider serviceProvider, ICustomer customerBo) : base(serviceProvider)
        {
            this.customerBo = customerBo;
        }

        #endregion

        #region -- Overrides --
        #endregion

        /// <summary>
        /// Convert vw_MonthlyTransaction to linQ
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public IQueryable<MonthlyTransactionResponse> GetQueryableViewMonthlyTransaction(int? month, int? year)
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            var viewCustomerQueryable = customerBo.GetQueryableViewCustomer();
            var monthlyTransactionQueryable = GetQueryable<MonthlyTransaction>();
            //var monthlyTransactionCurrentMonth = monthlyTransactionQueryable.Where(x => x.DateTimeAdd.Month == month && x.DateTimeAdd.Year == year);

            var viewMonthlyTransactionQueryable = (from transaction in monthlyTransactionQueryable
                                                   from customer in viewCustomerQueryable.
                                                   Where(x => x.CustomerID == transaction.CustomerID).DefaultIfEmpty()
                                                   where transaction.DateTimeAdd.Month == (month.HasValue ? month : currentMonth) && transaction.DateTimeAdd.Year == (year.HasValue ? year : currentYear)
                                                   //orderby transaction.ID descending
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

            return viewMonthlyTransactionQueryable;
        }

        /// <summary>
        /// Danh sách giao dịch theo dịch vụ
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public async Task<object> GetTransactionByServiceID(int serviceID)
        {
            var viewMonthyTransactionQueryable =  GetQueryableViewMonthlyTransaction(null, null);
            var queryable = viewMonthyTransactionQueryable
                .Where(x => x.ServiceID == serviceID )
                .OrderByDescending(x => x.DateTimeAdd);

            if (queryable.Any())
            {
                return await Task.FromResult(queryable.ToList());
            }

            return await Task.FromResult(default(object));
        }

        /// <summary>
        /// Danh sách giao dịch với chi nhánh + dịch vụ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<MonthlyTransactionResponse> GetTransactionByConditions(BillFilterDto req)
        {
            int serviceID = req.ServiceID;
            int? retailID = req.RetailID;
            int? month = req.Month;
            int? year = req.Year;

            var viewMonthyTransactionQueryable = GetQueryableViewMonthlyTransaction(month, year);
            var queryable = viewMonthyTransactionQueryable
                .Where(x => x.ServiceID == serviceID && (retailID.HasValue ?  x.RetailID == retailID : true) && (month.HasValue ? x.Month == month : true) && (year.HasValue ? x.Year == year : true))
                .OrderByDescending(x => x.DateTimeAdd);

            if (queryable.Any())
            {
                var result = queryable.ToList();
                return result;
            }

            return new List<MonthlyTransactionResponse>();
        }

        /// <summary>
        /// Thêm mới giao dịch
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<object> InsertTransactionAsync(BillInsertDto req)
        {
            var monthlyTransactionRepository = GetRepository<MonthlyTransaction>();
            var target = mapper.Map<BillInsertDto, MonthlyTransaction> (req);
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
            var monthlyTransactionRepository = GetRepository<MonthlyTransaction>();
            var monthlyTransactionQueryable = GetQueryable<MonthlyTransaction>();

            var transaction = monthlyTransactionQueryable
                .Where(x => x.ID == req.ID && x.DateTimeAdd.Month == month && x.DateTimeAdd.Year == year)
                .FirstOrDefault();

            if(transaction != null)
            {
                transaction = mapper.Map<BillUpdateDto, MonthlyTransaction>(req, transaction);
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
            var monthlyTransactionRepository = GetRepository<MonthlyTransaction>();
            var transaction = monthlyTransactionRepository.FindBy(x => x.ID == billID).FirstOrDefault();

            if(transaction != null)
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

                var monthlyTransactionRepository = GetRepository<MonthlyTransaction>();
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

        //public async Task<ResponseResults> FindByCondition(BillSearchDto req)
        //{

        //}

        public async Task<object> Import(int monthPrev)
        {
            return null;
        }

        public async Task<byte[]> Print(int billID)
        {
            try
            {
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;

                var viewMonthlyTransactionQueryable = GetQueryableViewMonthlyTransaction(month, year);
                var dataBill = viewMonthlyTransactionQueryable.Where(x => x.ID == billID && x.Month == month && x.Year == year);

                if(dataBill.Any())
                {
                    DataTable table = Utility.ToDataTable(dataBill.ToList());
                    byte[] byteArray = ExcelUtility.CreateAndWriteBillExcel(table);

                    return byteArray;
                }

                return new byte[] {};
            }
            catch
            {
                throw;
            }
        }

        public byte[] PrintMultiRow(BillPrintTransactionsDto req)
        {
            var tblCustomer = GetQueryable<Customer>();
            var tblBillTemp = GetQueryable<MonthlyTransaction>();

            List<string> listID = req.ListBillID.Split(";").ToList();

            var listBillTemp = (from temp in tblBillTemp
                                from customer in tblCustomer.Where(x => x.CustomerID == temp.CustomerID)
                                where temp.DateTimeAdd.Month == req.Month && temp.DateTimeAdd.Year == req.Year &&
                                    temp.ServiceID == req.ServiceID && listID.Contains(temp.ID.ToString())
                                orderby temp.RetailID
                                select new
                                {
                                    FullName = customer.FullName,
                                    Code = customer.Code,
                                    Money = temp.Money,
                                    Postage = temp.Postage,
                                    Total = temp.Total
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