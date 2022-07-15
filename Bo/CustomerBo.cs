﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.CustomerID;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;
using SystemServiceAPI.Helpers;

namespace SystemServiceAPI.Bo
{
    public class CustomerBo : ICustomer
    {
        private readonly AppDbContext _dbContext;
        public CustomerBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<ResponseResults> GetCustomerByID(int customerID)
        {
            ResponseResults response = new ResponseResults();
            try
            {
                var data = await _dbContext.vw_Customers.Where(x => x.CustomerID == customerID && x.IsDelete == false).FirstOrDefaultAsync();
                if (data != null)
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

        public async Task<ResponseResults> GetByCondition(CustomerRequestDto req)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                List<vw_Customer> data = new List<vw_Customer>();

                if (req.RetailID.HasValue && req.ServiceID.HasValue)
                {
                    data = await _dbContext.vw_Customers.Where(x => x.ServiceID == req.ServiceID && x.RetailID == req.RetailID && x.IsDelete == false).ToListAsync();
                }
                else
                {
                    if (req.RetailID.HasValue && !req.ServiceID.HasValue)
                    {
                        data = await _dbContext.vw_Customers.Where(x => x.RetailID == req.RetailID).ToListAsync();
                    }
                    else
                    {
                        data = await _dbContext.vw_Customers.Where(x => x.ServiceID == req.ServiceID).ToListAsync();
                    }
                }

                if (data.Count > 0)
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

        public async Task<ResponseResults> Post(AddCustomerDto req)
        {
            ResponseResults response = new ResponseResults();
            try
            {
                //với khách hàng đóng tiền điện => code không trùng
                if (req.ServiceID == 1)
                {
                    var isExistCode = _dbContext.Customers.Where(
                            x => x.Code == req.Code &&
                            x.ServiceID == 1
                        ).FirstOrDefault();

                    if (isExistCode != null)
                    {
                        response.Code = (int)HttpStatusCode.InternalServerError;
                        response.Result = null;
                        response.Msg = "Mã số tiền điện " + req.Code + " đã tồn tại";

                        return await Task.FromResult(response);
                    }

                    var isExistName = await _dbContext.Customers.Where(
                           x => x.FullName.ToLower() == req.FullName.ToLower() &&
                           x.ServiceID == 1
                       ).ToListAsync();

                    if (isExistName != null && isExistName.Count > 0)
                    {
                        req.FullName = req.FullName + "(" + isExistName.Count() + 1 + ")";
                    }
                }
                else
                {
                    var isExistName = await _dbContext.Customers.Where(y => y.FullName.ToLower() == req.FullName.ToLower()).ToListAsync();

                    if (isExistName.Count > 0)
                    {
                        req.FullName = req.FullName + "|" + _dbContext.Banks.Where(x => x.BankID == req.BankID).Select(y => y.ShortName).FirstOrDefault();
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

        public async Task<ResponseResults> Put(UpdateCustomerDto req)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var customerData = _dbContext.Customers.Where(
                        x => x.CustomerID == req.CustomerID
                    ).FirstOrDefault();

                if (customerData != null)
                {
                    if (req.FullName.ToLower() == customerData.FullName.ToLower())
                    {
                        if (req.ServiceID == 1)
                        {
                            var countNameExist = _dbContext.Customers.Where(x => x.FullName.ToLower() == customerData.FullName.ToLower()).ToList();

                            if (countNameExist.Count > 0)
                            {
                                req.FullName = req.FullName + "(" + countNameExist.Count + 1 + ")";
                            }
                        }
                        else
                        {
                            req.FullName = req.FullName + "|" + _dbContext.Banks.Where(x => x.BankID == req.BankID).Select(y => y.ShortName).FirstOrDefault();
                        }
                    }

                    Customer entity = new Customer()
                    {
                        FullName = req.FullName,
                        Code = req.Code,
                        Address = customerData.Address,
                        Hotline = req.Hotline,
                        IsDelete = customerData.IsDelete,
                        DateTimeAdd = customerData.DateTimeAdd,
                        DateTimeUpdate = DateTime.Now,
                        MonthPrint = customerData.MonthPrint,
                        YearPrint = customerData.YearPrint,
                        IsPrint = customerData.IsPrint,
                        RetailID = req.RetailID,
                        ServiceID = customerData.ServiceID,
                        BankID = req.BankID,
                        VillageID = req.VillageID
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

        public async Task<ResponseResults> DeleteByID(int customerID)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var customerData = _dbContext.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
                if (customerData == null)
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Result = null;
                    response.Msg = "FAIL";
                }
                else
                {
                    Customer entity = new Customer()
                    {
                        FullName = customerData.FullName,
                        Code = customerData.Code,
                        Address = customerData.Address,
                        Hotline = customerData.Hotline,
                        IsDelete = true,
                        DateTimeAdd = customerData.DateTimeAdd,
                        DateTimeUpdate = DateTime.Now,
                        MonthPrint = customerData.MonthPrint,
                        YearPrint = customerData.YearPrint,
                        IsPrint = customerData.IsPrint,
                        RetailID = customerData.RetailID,
                        ServiceID = customerData.ServiceID,
                        BankID = customerData.BankID,
                        VillageID = customerData.VillageID
                    };

                    _dbContext.Update(entity);
                    _dbContext.SaveChanges();

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

        public async Task<ResponseResults> DeleteMultiRow(DeleteCustomerDto req)
        {
            ResponseResults response = new ResponseResults();
            List<string> listID = req.ListCustomerID.Split(';').ToList();
            try
            {
                var customers = _dbContext.Customers.Where(
                        x => x.ServiceID == req.ServiceID &&
                        listID.Contains(x.CustomerID.ToString()) &&
                        x.IsDelete == false
                    ).ToList();

                if (customers == null)
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Result = null;
                    response.Msg = "NOT FOUND";
                }
                else
                {
                    foreach (var item in customers)
                    {
                        item.IsDelete = true;
                    }

                    _dbContext.Update(customers);
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
    }
}
