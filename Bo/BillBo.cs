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
    public class BillBo: IBillBo
    {
        private readonly AppDbContext _dbContext;
        public BillBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<object> GetByServiceID(int serviceID)
        {
            ResponseResults response = new ResponseResults();
            try
            {
                List<vw_MonthlyTransaction> data = await _dbContext.vw_MonthlyTransactions.Where(
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

                _dbContext.Add(req);
                _dbContext.SaveChanges();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = null;
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
                    MonthlyTransaction entity = new MonthlyTransaction()
                    {
                        CustomerID = billData.CustomerID,
                        ServiceID = billData.ServiceID,
                        RetailID = req.RetailID,
                        BankID = req.BankID,
                        Code = billData.Code,
                        Money = req.Money,
                        Postage = req.Postage,
                        Total = req.Total,
                        Status = billData.Status,
                        DateTimeAdd = billData.DateTimeAdd,
                        DateTimeUpdate = DateTime.Now
                    };

                    _dbContext.Update(entity);
                    _dbContext.SaveChanges();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = null;
                    response.Msg = "SUCCESS";

                    return await Task.FromResult(response);
                }
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
                    _dbContext.Remove(billID);
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
            List<string> listID = req.listBillID.Split(';').ToList();
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
                    response.Msg = "FAIL";
                }
                else
                {
                    _dbContext.Remove(billData);
                    _dbContext.SaveChanges(true);

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = null;
                    response.Msg = "SUCCESS";
                }
                

                response.Code = (int)HttpStatusCode.OK;
                response.Result = null;
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

        //public async Task<ResponseResults> FindByCondition(BillSearchDto req)
        //{

        //}

        public async Task<ResponseResults> Import(int monthPrev)
        {
            return null;
        }

        public async Task<ResponseResults> Print(int billID)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var dataBill = await _dbContext.MonthlyTransactions.Where(x => x.ID == billID).FirstOrDefaultAsync();
                if(dataBill != null)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = dataBill;
                    response.Msg = "SUCCESS";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Result = null;
                    response.Msg = "NOT FOUND";
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

        public async Task<ResponseResults> PrintAll(int serviceID)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var dataBill = await _dbContext.MonthlyTransactions.Where(x => x.ServiceID == serviceID).ToListAsync();
                if (dataBill != null)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = dataBill;
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
    }
}