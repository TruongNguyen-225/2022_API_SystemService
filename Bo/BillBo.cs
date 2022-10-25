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
using SystemServiceAPICore3.Utilities;

namespace SystemServiceAPI.Bo
{
    public class BillBo : IBillBo
    {
        #region -- Variables --

        private readonly AppDbContext _dbContext;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        public BillBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        #endregion

        #region -- Overrides --
        #endregion

        public async Task<List<vw_MonthlyTransaction>> GetByServiceID(int serviceID)
        {
            List<vw_MonthlyTransaction> result = new List<vw_MonthlyTransaction>();
            var queryable = _dbContext.vw_MonthlyTransactions.Where(
                   x => x.ServiceID == serviceID &&
                   x.Month == DateTime.Now.Month &&
                   x.Year == DateTime.Now.Year
               ).OrderByDescending(x => x.DateTimeAdd);

            if (queryable.Any())
            {
                result = await queryable.ToListAsync();
            }

            return await Task.FromResult(result);
        }

        public async Task<List<vw_MonthlyTransaction>> GetByMonth(BillFilterDto req)
        {
            List<vw_MonthlyTransaction> result = new List<vw_MonthlyTransaction>();
            var queryable = _dbContext.vw_MonthlyTransactions.Where(
                      x => x.Month == req.Month &&
                      x.Year == DateTime.Now.Year &&
                      x.ServiceID == req.ServiceID
                  );

            if (queryable.Any())
            {
                result = await queryable.ToListAsync();
            }

            return await Task.FromResult(result);
        }

        public async Task<MonthlyTransaction> Post(BillRequestDto req)
        {
            MonthlyTransaction transaction = new MonthlyTransaction();
            transaction.CustomerID = req.CustomerID;
            transaction.ServiceID = req.ServiceID;
            transaction.RetailID = req.RetailID;
            transaction.BankID = req.BankID;
            transaction.Code = req.Code;
            transaction.Money = req.Money;
            transaction.Postage = req.Postage;
            transaction.Total = req.Total;
            transaction.Status = req.Status;
            transaction.DateTimeAdd = DateTime.Now;
            transaction.DateTimeUpdate = null;

            _dbContext.Add(transaction);
            _dbContext.SaveChanges();

            return await Task.FromResult(transaction);
        }

        public async Task<bool> CheckBeforeAddBillElectricity(int serviceID, string code)
        {
            var dataElectric = _dbContext.MonthlyTransactions.Where(
                        x => x.Code == code &&
                        x.ServiceID == serviceID &&
                        x.DateTimeAdd.Month == DateTime.Now.Month &&
                        x.DateTimeAdd.Year == DateTime.Now.Year
                    ).FirstOrDefault();

            if (dataElectric != null)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<ResponseResults> Put(BillUpdatetDto req)
        {
            ResponseResults response = new ResponseResults();
            try
            {
                var billData = _dbContext.MonthlyTransactions.Where(
                        x => x.ID == req.ID &&
                        x.DateTimeAdd.Month == DateTime.Now.Month &&
                        x.DateTimeAdd.Year == DateTime.Now.Year
                    ).FirstOrDefault();

                if(billData != null)
                {
                    billData.Money = req.Money;
                    billData.Postage = req.Postage;
                    billData.Total = req.Total;
                    billData.DateTimeUpdate = DateTime.Now;
                    
                    _dbContext.Update(billData);
                    _dbContext.SaveChanges();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = billData;
                    response.Msg = "SUCCESS";

                    return await Task.FromResult(response);
                }

                response.Code = (int)HttpStatusCode.NotFound;
                response.Result = null;
                response.Msg = "NOT FOUND";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.NotModified;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }

        public async Task<ResponseResults> DeleteByID(int billID)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var data = _dbContext.MonthlyTransactions.Where(x => x.ID == billID).FirstOrDefault();
                if(data == null)
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Result = null;
                    response.Msg = "NOT FOUND";
                }
                else
                {
                    _dbContext.Remove(data);
                    _dbContext.SaveChanges();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = null;
                    response.Msg = "SUCCESS";
                }
            }
            catch(Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }

        public async Task<ResponseResults> DeleteMultiRow(BillDeleteDto req)
        {
            ResponseResults response = new ResponseResults();
            List<string> listID = req.ListBillID.Split(';').ToList();
            try
            {
                var billData = _dbContext.MonthlyTransactions.Where(
                        x => x.ServiceID == req.ServiceID &&
                        listID.Contains(x.ID.ToString()) &&
                        x.DateTimeAdd.Month == req.Month &&
                        x.DateTimeAdd.Year == req.Year
                    );

                if (billData == null)
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Result = null;
                    response.Msg = "NOT FOUND";
                }
                else
                {
                    foreach(MonthlyTransaction item in billData)
                    {
                        _dbContext.Remove(item);
                    }

                    _dbContext.SaveChanges(true);

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = null;
                    response.Msg = "SUCCESS";
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

        //public async Task<ResponseResults> FindByCondition(BillSearchDto req)
        //{

        //}

        public async Task<ResponseResults> Import(int monthPrev)
        {
            return null;
        }

        public async Task<byte[]> Print(int billID)
        {
            try
            {
                List<vw_MonthlyTransaction> dataBill = await _dbContext.vw_MonthlyTransactions.Where(x => x.ID == billID).ToListAsync();
                if(dataBill != null)
                {
                    DataTable table = Utility.ToDataTable(dataBill);
                    byte[] byteArray = ExcelUtility.CreateAndWriteBillExcel(table);

                    return byteArray;
                }
            }
            catch(Exception ex)
            {
            }

            return null;
        }

        public async Task<ResponseResults> PrintMultiRow(BillPrintAllDto req)
        {
            ResponseResults response = new ResponseResults();

            List<string> listID = req.ListBillID.Split(';').ToList();
            try
            {
                var billData = await _dbContext.MonthlyTransactions.Where(
                        x => x.ServiceID == req.ServiceID &&
                        listID.Contains(x.ID.ToString()) &&
                        x.DateTimeAdd.Month == req.Month &&
                        x.DateTimeAdd.Year == req.Year
                    ).ToListAsync();

                if (billData.Count < 1)
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Result = null;
                    response.Msg = "NOT FOUND";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = billData;
                    response.Msg = "SUCCESS";
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
    }
}