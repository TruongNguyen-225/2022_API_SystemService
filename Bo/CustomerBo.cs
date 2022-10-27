using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Context;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.CustomerID;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;
using SystemServiceAPICore3.Bo;
using SystemServiceAPICore3.Dto;

namespace SystemServiceAPI.Bo
{
    public class CustomerBo : BaseBo<CustomerDto, Customer>, ICustomer
    {
        #region -- Variables --

        private readonly AppDbContext _dbContext;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the <see cref="TS_tblLeaveRecordBo"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public CustomerBo(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        #endregion

        #region -- Overrides --
        #endregion

        #region -- Implements --

        /// <summary>
        /// Lấy thông tin khách hàng khi biết customerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<object> GetCustomerByID(int customerID)
        {
            var customerQueryable = GetQueryable<vw_Customer>();
            var dataCustomer = await customerQueryable
                                    .Where(x => x.CustomerID == customerID && x.IsDelete == false)
                                    .FirstOrDefaultAsync();

            return await Task.FromResult(dataCustomer);
        }

        /// <summary>
        /// Lấy danh sách khách hàng với serviceID
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public async Task<object> GetCustomerByServiceID(int serviceID)
        {
            var customerQueryable = GetQueryable<vw_Customer>();
            var customers = customerQueryable
                            .Where(x => x.ServiceID == serviceID && x.IsDelete == false)
                            .OrderByDescending(x => x.DateTimeAdd);

            if (customers.Any())
            {
                var result = await customers.ToListAsync();
                return await Task.FromResult(result);
            }

            return await Task.FromResult(default(object));
        }

        /// <summary>
        /// Lấy danh sách khách hàng theo nhiều điều kiện
        /// Theo serviceID
        /// Theo RetailID
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<vw_Customer>> GetByCondition(CustomerRequestDto req)
        {
            List<vw_Customer> result = new List<vw_Customer>();

            int? retailID = req.RetailID;
            int? serviceID = req.ServiceID;
            var customerQueryable = GetQueryable<vw_Customer>();

            if (retailID.HasValue && serviceID.HasValue)
            {
                var customers = customerQueryable
                    .Where(x => x.ServiceID == serviceID.Value
                        && x.RetailID == retailID.Value
                        && x.IsDelete == false)
                    .OrderByDescending(x => x.DateTimeAdd);

                if (customers.Any())
                {
                    result = await customers.ToListAsync();
                }
            }
            else if (retailID.HasValue && !serviceID.HasValue)
            {
                var customers = customerQueryable
                    .Where(x => x.RetailID == retailID.Value && x.IsDelete == false)
                    .OrderByDescending(x => x.DateTimeAdd);

                if (customers.Any())
                {
                    result = await customers.ToListAsync();
                }
            }
            else
            {
                var customers = customerQueryable
                    .Where(x => x.ServiceID == serviceID.Value && x.IsDelete == false)
                    .OrderByDescending(x => x.DateTimeAdd);

                if (customers.Any())
                {
                    result = await customers.ToListAsync();
                }
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<object> Post(AddCustomerDto req)
        {
            int serviceID = req.ServiceID;
            string code = req.Code;
            string fullName = req.FullName;
            int retailID = req.RetailID;
            int? bankID = req.BankID;

            var customerQueryable = GetQueryable<Customer>();
            var bankQueryable = GetQueryable<Bank>();
            var retailQueryable = GetQueryable<Retail>();
            var serviceQueryable = GetQueryable<Service>();
            var customerRepository = GetRepository<Customer>();

            //Tiền điện
            if (serviceID == 1)
            {
                var customerByCode = await customerQueryable
                    .Where(x => x.Code == code && x.ServiceID == serviceID && x.IsDelete == false)
                    .FirstOrDefaultAsync();

                if (customerByCode != null)
                {
                    return await Task.FromResult(default(object));
                }

                var customerByFullName = (from queryable in customerQueryable
                                          .Where(x => x.FullName.ToLower() == fullName.ToLower()
                                            && x.ServiceID == serviceID
                                            && x.IsDelete == false)
                                          select new
                                          {
                                              Code = queryable.Code,
                                              RetailName = queryable.RetailID
                                          }).FirstOrDefaultAsync();

                if (customerByFullName != null)
                {
                    code = customerByFullName.Result.Code;
                    code = code.Substring(code.Length - 5, code.Length);
                    req.FullName += $"{fullName}|{code}";
                }

                var customerDto = mapper.Map<AddCustomerDto, CustomerDto>(req);
                customerDto = Insert(customerDto);

                return await Task.FromResult(customerDto);
            }

            var customersByCode = (from customer in customerQueryable
                                   from retail in retailQueryable.Where(x => x.RetailID == customer.RetailID).DefaultIfEmpty()
                                   from service in serviceQueryable.Where(x => x.ServiceID == customer.ServiceID).DefaultIfEmpty()
                                   from bank in bankQueryable.Where(x => x.BankID == customer.BankID).DefaultIfEmpty()
                                   where customer.Code == code
                                        && customer.ServiceID != 1
                                        && customer.IsDelete == false
                                   select new
                                   {
                                       Code = customer.Code,
                                       FullName = customer.FullName,
                                       BankName = bank.ShortName,
                                       BankID = bank.BankID,
                                       RetailName = retail.RetailName,
                                       RetailID = retail.RetailID,
                                       ServiceName = service.ServiceName,
                                       ServiceID = service.ServiceID
                                   }).FirstOrDefault();
            //TH: Code đã tồn tại ở bất kì chi nhánh nào, dịch vụ nào trừ tiền điện
            if (customersByCode != null)
            {
                string codeQuery = customersByCode.Code;
                string fullNameQuery = customersByCode.FullName;
                string bankNameQuery = customersByCode.BankName;
                string retailNameQuery = customersByCode.RetailName;
                int? bankIDQuery = customersByCode.BankID;
                int retailIDQuery = customersByCode.RetailID;

                //TH1. Cùng code và chi nhánh và cùng luôn ngân hàng => Đã tồn tại
                if (retailID == retailIDQuery && bankID == bankIDQuery)
                {
                    return await Task.FromResult(default(object));
                }

                //TH2: Cùng code khác chi nhánh => insert Chi Nhánh|Tên Khách
                req.FullName = String.Format("{0}|{1}", retailNameQuery, fullNameQuery);
            }

            string retailNameByID = retailQueryable.Where(x => x.RetailID == retailID).Select(x => x.RetailName).FirstOrDefault();
            req.FullName = String.Format("{0}|{1}", retailNameByID, fullName);

            var dto = mapper.Map<AddCustomerDto, CustomerDto>(req);
            dto = Insert(dto);

            return await Task.FromResult(dto);
        }

        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
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

                    customerData.FullName = req.FullName;
                    customerData.Code = req.Code;
                    customerData.Hotline = req.Hotline;
                    customerData.RetailID = req.RetailID;
                    customerData.BankID = req.BankID;
                    customerData.VillageID = req.VillageID;
                    customerData.DateTimeUpdate = DateTime.Now;

                    _dbContext.Update(customerData);
                    _dbContext.SaveChanges();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = customerData;
                    response.Msg = "SUCCESS";

                    return await Task.FromResult(response);
                }

                response.Code = (int)HttpStatusCode.NotFound;
                response.Result = customerData;
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
                    response.Msg = "NOT FOUND";
                }
                else
                {
                    customerData.IsDelete = true;
                    customerData.DateTimeUpdate = DateTime.Now;

                    _dbContext.Update(customerData);
                    _dbContext.SaveChanges();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = customerData;
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
                    foreach (Customer item in customers)
                    {
                        item.IsDelete = true;
                        _dbContext.Update(item);
                    }

                    _dbContext.SaveChanges(true);

                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = customers;
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

        #endregion
    }
}
