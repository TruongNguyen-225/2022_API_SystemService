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
        #region -- parameters --

        private readonly AppDbContext _dbContext;

        #endregion

        #region -- contructor --

        public BillBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        #endregion

        #region -- implements --



        #endregion

        public async Task<List<vw_MonthlyTransaction>> GetByServiceID(int serviceID)
        {

            List<vw_MonthlyTransaction> result = new List<vw_MonthlyTransaction>();

            var data = _dbContext.vw_MonthlyTransactions.Where(
                   x => x.ServiceID == serviceID &&
                   x.Month == DateTime.Now.Month &&
                   x.Year == DateTime.Now.Year
               ).OrderByDescending(x => x.DateTimeAdd);

            if (data.Any())
            {
                result = await data.ToListAsync();
            }

            return await Task.FromResult(result);
        }

        public async Task<object> GetByMonth(BillFilterDto req)
        {
            ResponseResults response = new ResponseResults();
            try
            {
                List<vw_MonthlyTransaction> data = await _dbContext.vw_MonthlyTransactions.Where(
                    x => x.Month == req.Month &&
                    x.Year == DateTime.Now.Year &&
                    x.ServiceID == req.ServiceID
                ).ToListAsync();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = data;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }

        public async Task<ResponseResults> Post(BillRequestDto req)
        {
            ResponseResults response = new ResponseResults();
            try
            {
                //check xem đơn đã tồn tại chưa, đối với hóa đơn tiền điện
                if(req.ServiceID == 1)
                {
                    var checkExist = _dbContext.MonthlyTransactions.Where(
                            x => x.Code == req.Code && 
                            x.ServiceID == 1 && 
                            x.DateTimeAdd.Month == DateTime.Now.Month && 
                            x.DateTimeAdd.Year == DateTime.Now.Year
                        ).FirstOrDefault();

                    if(checkExist != null)
                    {
                        response.Code = (int)HttpStatusCode.InternalServerError;
                        response.Result = null;
                        response.Msg = "Mã số tiền điện " +req.Code + " đã được đóng";

                        return await Task.FromResult(response);
                    }
                }

                MonthlyTransaction m = new MonthlyTransaction();
                m.CustomerID = req.CustomerID;
                m.ServiceID = req.ServiceID;
                m.RetailID = req.RetailID;
                m.BankID = req.BankID;
                m.Code = req.Code;
                m.Money = req.Money;
                m.Postage = req.Postage;
                m.Total = req.Total;
                m.Status = req.Status;
                m.DateTimeAdd = DateTime.Now;
                m.DateTimeUpdate = null;

                _dbContext.Add(m);
                _dbContext.SaveChanges();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = m;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
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