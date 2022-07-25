using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;
using SystemServiceAPI.Helpers;

namespace SystemServiceAPI.Bo
{
    public class BillTempBo: IBillTempBo
    {
        private readonly AppDbContext _dbContext;
        public BillTempBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<object> GetData()
        {
            ResponseResults response = new ResponseResults();
            try
            {
                int serviceID = 1;
                List<vw_MonthlyTransactionTemp> data = await _dbContext.vw_MonthlyTransactionTemp.Where(
                   x => x.ServiceID == serviceID &&
                   x.Month == DateTime.Now.Month &&
                   x.Year == DateTime.Now.Year
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
                    var checkExist = _dbContext.MonthlyTransactionTemp.Where(
                            x => x.Code == req.Code && 
                            x.ServiceID == 1 && 
                            x.DateTimeAdd.Month == DateTime.Now.Month && 
                            x.DateTimeAdd.Year == DateTime.Now.Year
                        ).FirstOrDefault();

                    if(checkExist != null)
                    {
                        response.Code = (int)HttpStatusCode.InternalServerError;
                        response.Result = null;
                        response.Msg = "Mã số tiền điện " +req.Code + " đã ton tai";

                        return await Task.FromResult(response);
                    }
                }

                    MonthlyTransactionTemp m = new MonthlyTransactionTemp();
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
                var billData = _dbContext.MonthlyTransactionTemp.Where(
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
                var data = _dbContext.MonthlyTransactionTemp.Where(x => x.ID == billID).FirstOrDefault();
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
                var billData = _dbContext.MonthlyTransactionTemp.Where(
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
                    foreach(MonthlyTransactionTemp item in billData)
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

        public async Task<ResponseResults> Import(int month)
        {
            ResponseResults response = new ResponseResults();

            var checkData = _dbContext.MonthlyTransactionTemp.ToList();
            if(checkData.Count > 0)
            {
                foreach(var item in checkData)
                {
                    _dbContext.MonthlyTransactionTemp.Remove(item);
                }
                _dbContext.SaveChanges();
            }

            var data = _dbContext.MonthlyTransactions
                .Where(x => x.ServiceID == 1 && x.DateTimeAdd.Month == month && x.DateTimeAdd.Year == DateTime.Now.Year)
                .ToList();

            if(data.Count > 0)
            {
                try
                {
                    foreach(var req in data)
                    {
                        MonthlyTransactionTemp m = new MonthlyTransactionTemp();
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

                        _dbContext.MonthlyTransactionTemp.Add(m);
                    }
                    
                    _dbContext.SaveChanges();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = data;
                    response.Msg = "SUCCESS";
                }
                catch(Exception ex)
                {
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Result = null;
                    response.Msg = ex.Message;
                }
            }
            else
            {
                response.Code = (int)HttpStatusCode.NotFound;
                response.Result = null;
                response.Msg = "NOT FOUND";
            }

            return await Task.FromResult(response);
        }

        public async Task<ResponseResults> PrintMultiRow(BillPrintAllDto req)
        {
            ResponseResults response = new ResponseResults();

            List<string> listID = req.ListBillID.Split(';').ToList();
            try
            {
                var billData = await _dbContext.MonthlyTransactionTemp.Where(
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