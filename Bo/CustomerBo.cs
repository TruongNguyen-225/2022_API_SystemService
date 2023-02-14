using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.CustomerID;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Bo;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Dto.Other;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Bo
{
    public class CustomerBo : BaseBo<CustomerDto, Customer>, ICustomer
    {
        #region -- Variables --

        private readonly IServiceProvider serviceProvider;

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
            this.serviceProvider = serviceProvider;
        }

        #endregion

        #region -- Overrides --
        #endregion

        #region -- Implements --

        /// <summary>
        /// View vw_Customer to linQ
        /// </summary>
        /// <returns></returns>
        public IQueryable<CustomerResponse> GetQueryableViewCustomer()
        {
            var customerQueryable = GetQueryable<Customer>();
            var bankQueryable = GetQueryable<Bank>();
            var retailQueryable = GetQueryable<Retail>();
            var serviceQueryable = GetQueryable<Service>();
            var customerRepository = GetRepository<Customer>();

            var queryable = (from customer in customerQueryable
                             from retail in retailQueryable.Where(x => x.RetailID == customer.RetailID).DefaultIfEmpty()
                             from service in serviceQueryable.Where(x => x.ServiceID == customer.ServiceID).DefaultIfEmpty()
                             from bank in bankQueryable.Where(x => x.BankID == customer.BankID).DefaultIfEmpty()
                             where customer.IsDelete == false
                             //orderby customer.FullName ascending, customer.DateTimeAdd descending
                             select new CustomerResponse
                             {
                                 STT = 1,
                                 ServiceID = service.ServiceID,
                                 ServiceName = service.ServiceName,
                                 BankID = bank.BankID,
                                 BankName = bank.ShortName,
                                 CustomerID = customer.CustomerID,
                                 RetailID = retail.RetailID,
                                 RetailName = retail.RetailName,
                                 FullName = customer.FullName,
                                 Code = customer.Code,
                                 VillageID = customer.VillageID,
                                 Hotline = customer.Hotline,
                                 Status = customer.IsDelete ? "Đã xoá" : "Đang hoạt động",
                                 IsDelete = customer.IsDelete,
                                 DateTimeAdd = customer.DateTimeAdd,
                                 DateTimeUpdate = customer.DateTimeUpdate
                             });

            return queryable;
        }

        /// <summary>
        /// Lấy thông tin tiền điện của khách hàng theo ID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> GetBillElectricByCustomerID(int customerID)
        {
            var tempBo = serviceProvider.GetService<IBillTempBo>();
            int serviceID = 1;
            int month = DateTime.Now.Month;

            var queryable = tempBo.GetDataTempByMonth(month);
            var dataTemp = queryable.Where(x => x.ServiceID == serviceID && x.CustomerID == customerID).FirstOrDefault();

            if (dataTemp != null)
            {
                return await Task.FromResult(dataTemp);
            }

            var customer = GetCustomerByID(customerID);
            return await Task.FromResult(customer);
        }


        /// <summary>
        /// Lấy thông tin khách hàng khi biết customerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<object> GetCustomerByID(int customerID)
        {
            var customerQueryable = GetQueryableViewCustomer();
            var result = customerQueryable.Where(x => x.CustomerID == customerID).ToList();

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Lấy khách hàng khi biết customerID, trả về Customer Entity
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomer(int customerID)
        {
            var customerQueryable = GetQueryable<Customer>();
            var customer = await customerQueryable
                                    .Where(x => x.CustomerID == customerID && x.IsDelete == false)
                                    .FirstOrDefaultAsync();

            return await Task.FromResult(customer);
        }

        /// <summary>
        /// Lấy danh sách khách hàng với serviceID
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public async Task<object> GetCustomerByServiceID(int serviceID)
        {
            var customerQueryable = GetQueryableViewCustomer();
            var result = customerQueryable.Where(x => x.ServiceID == serviceID);

            if (result.Any())
            {
                return await Task.FromResult(result.ToList());
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
        public async Task<object> GetByCondition(CustomerRequestDto req)
        {
            int? retailID = req.RetailID;
            int? serviceID = req.ServiceID;

            var customerQueryable = GetQueryableViewCustomer();
            IQueryable<CustomerResponse> result = null;

            if (retailID.HasValue && serviceID.HasValue)
            {
                result = customerQueryable.Where(customer => customer.ServiceID == serviceID.Value && customer.RetailID == retailID.Value);
            }
            else if (retailID.HasValue && !serviceID.HasValue)
            {
                result = customerQueryable.Where(customer => customer.RetailID == retailID.Value);
            }
            else
            {
                result = customerQueryable.Where(customer => customer.ServiceID == serviceID.Value);
            }

            if (result.Any())
            {
                return await Task.FromResult(result.ToList());
            }

            return await Task.FromResult(default(object));
        }

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<object> InsertCustomer(AddCustomerDto req)
        {
            int serviceID = req.ServiceID;
            string code = req.Code;
            string fullName = req.FullName;
            int retailID = req.RetailID;
            int? bankID = req.BankID;

            var bankQueryable = GetQueryable<Bank>();
            var retailQueryable = GetQueryable<Retail>();

            var destination = mapper.Map<AddCustomerDto, CustomerDto>(req);
            var retail = retailQueryable.Where(x => x.RetailID == retailID).FirstOrDefault();

            //Tiền điện
            if (serviceID == 1)
            {
                code = destination.Code;
                code = code.Substring(code.Length - 5, code.Length);
                destination.FullName = $"{retail.RetailName} | {fullName} | {code}";
            }
            else
            {
                var bank = bankQueryable.Where(x => x.BankID == bankID.Value).FirstOrDefault();
                destination.FullName = $"{retail.RetailName} | {fullName} | {bank.ShortName}";
            }

            destination.DateTimeAdd = DateTime.Now;
            destination = Insert(destination);

            return await Task.FromResult(destination);
        }

        /// <summary>
        /// Kiểm tra xem khách hàng đã tồn tại chưa
        /// </summary>
        /// <param name="code"></param>
        /// <param name="serviceID"></param>
        /// <param name="retailID"></param>
        /// <returns></returns>
        public async Task<bool> CheckCustomerIsExist(string code, int serviceID, int retailID)
        {
            var customerQueryable = GetQueryableViewCustomer();
            var customerByCode = await customerQueryable
                    .Where(x => x.Code == code && x.RetailID == retailID && x.ServiceID == serviceID)
                    .FirstOrDefaultAsync();

            if (customerByCode != null)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        /// <summary>
        /// Kiểm tra xem có khách hàng đã tồn tại với code chuẩn bị thêm mới/cập nhật chưa
        /// </summary>
        /// <param name="code"></param>
        /// <param name="retailID"></param>
        /// <returns></returns>
        public async Task<bool> CheckCustomerIsExistByCode(string code, int retailID)
        {
            var customerQueryable = GetQueryableViewCustomer();
            var customer = customerQueryable
                .Where(x => x.RetailID == retailID && (x.Code == code || x.Code == (CustomerConstants.DISTRICT_CODE_ELECTRIC + code)))
                .FirstOrDefault();

            if (customer != null)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<object> UpdateCustomer(Customer customer, UpdateCustomerDto req)
        {
            int serviceID = req.ServiceID;
            string code = req.Code;
            string fullName = req.FullName;
            int retailID = req.RetailID;
            int customerID = req.CustomerID;
            int? bankID = req.BankID;

            var bankQueryable = GetQueryable<Bank>();
            var retailQueryable = GetQueryable<Retail>();
            var customerRepository = GetRepository<Customer>();

            customer = mapper.Map<UpdateCustomerDto, Customer>(req, customer);
            var retail = retailQueryable.Where(x => x.RetailID == retailID).FirstOrDefault();

            //Tiền điện
            if (serviceID == 1)
            {
                code = customer.Code;
                code = code.Substring(code.Length - 5, code.Length);
                customer.FullName = $"{retail.RetailName} | {fullName} | {code}";
            }
            else
            {
                var bank = bankQueryable.Where(x => x.BankID == bankID.Value).FirstOrDefault();
                customer.FullName = $"{retail.RetailName} | {fullName} | {bank.ShortName}";
            }

            customer.DateTimeUpdate = DateTime.Now;
            customer = customerRepository.Update(customer, true);

            return await Task.FromResult(customer);
        }

        /// <summary>
        /// Xoá một khách hàng với customerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<object> DeleteByID(int customerID)
        {
            var customerRepository = GetRepository<Customer>();
            var customer = customerRepository.FindBy(x => x.CustomerID == customerID && x.IsDelete == false).FirstOrDefault();

            if (customer != null)
            {
                customer.IsDelete = true;
                customer.DateTimeAdd = DateTime.Now;

                customer = customerRepository.Update(customer, true);
                return await Task.FromResult(customer);
            }

            return await Task.FromResult(default(object));
        }

        /// <summary>
        /// Xoá nhiều khách hàng
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<object> DeleteMultiRow(DeleteCustomerDto req)
        {
            try
            {
                var unitOfWork = GetDataContext();
                unitOfWork.BeginTransaction();

                List<string> listID = req.ListCustomerID.Split(';').ToList();
                int serviceID = req.ServiceID;

                var customerRepository = GetRepository<Customer>();
                var queryable = customerRepository.FindBy(x => x.ServiceID == serviceID && listID.Contains(x.CustomerID.ToString()) && x.IsDelete == false);

                if (queryable.Any())
                {
                    var customers = queryable.ToList();
                    foreach (var customer in customers)
                    {
                        customer.IsDelete = true;
                        customer.DateTimeAdd = DateTime.Now;
                        customerRepository.Update(customer, false);
                    }

                    unitOfWork.Save();
                    unitOfWork.Commit();

                    return await Task.FromResult(customers);
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

        #endregion
    }
}

