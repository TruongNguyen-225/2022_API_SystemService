using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public IQueryable<MonthlyTransactionResponse> GetQueryableViewMonthlyTransaction()
        {
            var viewCustomerQueryable = customerBo.GetQueryableViewCustomer();
            var monthlyTransactionQueryable = GetQueryable<MonthlyTransaction>();

            var viewMonthlyTransactionQueryable = (from transaction in monthlyTransactionQueryable
                                                   from customer in viewCustomerQueryable.
                                                   Where(x => x.CustomerID == transaction.CustomerID).DefaultIfEmpty()
                                                   orderby transaction.ID descending
                                                   select new MonthlyTransactionResponse
                                                   {
                                                       STT = 1,
                                                       ID = transaction.ID,
                                                       ServiceID = transaction.ServiceID,
                                                       ServiceName = customer.ServiceName,
                                                       CustomerID = transaction.CustomerID,
                                                       FullName = customer.FullName,
                                                       Code = customer.Code,
                                                       RetailID = customer.RetailID,
                                                       RetailName = customer.RetailName,
                                                       BankID = transaction.BankID,
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
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            var viewMonthyTransactionQueryable =  GetQueryableViewMonthlyTransaction();
            var queryable = viewMonthyTransactionQueryable
                .Where(x => x.ServiceID == serviceID && x.Month == month && x.Year == year)
                .OrderByDescending(x => x.DateTimeAdd);;

            if (queryable.Any())
            {
                return await Task.FromResult(queryable.ToList());
            }

            return await Task.FromResult(default(object));
        }

        /// <summary>
        /// Danh sách giao dịch với tháng + dịch vụ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public object GetTransactionByMonth(BillFilterDto req)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int serviceID = req.ServiceID;
            int monthRequest = req.Month;

            var viewMonthyTransactionQueryable = GetQueryableViewMonthlyTransaction();
            var queryable = viewMonthyTransactionQueryable
                .Where(x => x.ServiceID == serviceID && x.Month == monthRequest && x.Year == year)
                .OrderByDescending(x => x.DateTimeAdd);

            if (queryable.Any())
            {
                var result = queryable.ToList();
                return result;
            }

            return default(object);
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
        public async Task<object> UpdateTransactionAsync(BillUpdateDto req)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            var monthlyTransactionRepository = GetRepository<MonthlyTransaction>();

            var viewMonthlyTransactionQueryable = GetQueryableViewMonthlyTransaction();
            var transaction = viewMonthlyTransactionQueryable
                .Where(x => x.ID == req.ID && x.DateTimeAdd.Value.Month == month && x.DateTimeAdd.Value.Year == year)
                .FirstOrDefault();

            if(transaction != null)
            {
                var target = mapper.Map<BillUpdateDto, MonthlyTransaction>(req);
                target.DateTimeUpdate = DateTime.Now;
                monthlyTransactionRepository.Update(target, true);

                return await Task.FromResult(target);
            }

            return await Task.FromResult(default(object));
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
                var viewMonthlyTransactionQueryable = GetQueryableViewMonthlyTransaction();
                var dataBill = viewMonthlyTransactionQueryable.Where(x => x.ID == billID);

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

        public async Task<object> PrintMultiRow(BillPrintTransactionsDto req)
        {
            return await Task.FromResult(default(object));
        }
    }
}