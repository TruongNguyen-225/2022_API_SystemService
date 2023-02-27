using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.CustomerDto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Controllers;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class CustomerController : BaseController<CustomerDto>
    {
        #region -- Variables --

        private readonly ICustomer customerBo;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public CustomerController(IServiceProvider serviceProvider, ICustomer customerBo)
            : base(serviceProvider)
        {
            this.customerBo = customerBo;
        }

        #endregion

        /// <summary>
        /// Get information customer by CustomerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomerByID/{customerID}")]
        public async Task<object> GetCustomerByID(int customerID)
        {
            try
            {
                var result = await customerBo.GetCustomerByID(customerID);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get information customer by ServiceID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomerByServiceID/{serviceID}")]
        public async Task<object> GetCustomerByServiceID(int serviceID)
        {
            try
            {
                var result = await customerBo.GetCustomerByServiceID(serviceID);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Danh sách khách hàng theo điều kiện
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCustomerByCondition")]
        public async Task<object> GetCustomerByCondition([FromBody] CustomerRequestDto request)
        {
            try
            {
                int? serviceID = request.ServiceID;
                int? retailID = request.RetailID;

                if(serviceID.HasValue && !retailID.HasValue)
                {

                    var result = await customerBo.GetCustomerByServiceID((int)serviceID);

                    return Ok(new
                    {
                        Result = result,
                        Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                    });
                }
                else
                {
                    var result = await customerBo.GetCustomerByCondition(serviceID, retailID);

                    return Ok(new
                    {
                        Result = result,
                        Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                    });
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get data customer by multi conditions
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetByCondition")]
        public async Task<object> GetByCondition(CustomerRequestDto req)
        {
            try
            {
                var result = await customerBo.GetByCondition(req);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Create new a customer information
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertCustomer")]
        public async Task<object> InsertCustomerAsync(AddCustomerDto req)
        {
            try
            {
                int serviceID = req.ServiceID;
                string code = req.Code;
                string fullName = req.FullName;
                int retailID = req.RetailID;
                int? bankID = req.BankID;

                if (!String.IsNullOrEmpty(code) && !String.IsNullOrEmpty(fullName))
                {
                    bool checkCustomerExisted = await customerBo.CheckCustomerIsExist(code, serviceID, retailID);
                    if (!checkCustomerExisted)
                    {
                        var result = await customerBo.InsertCustomer(req);
                        if (result != null)
                        {
                            return Ok(new
                            {
                                Result = result,
                                Messages = String.Empty
                            });
                        }
                    }

                    return BadRequest(new
                    {
                        Result = default(object),
                        Messages = String.Format(CustomerConstants.CUSTOMER_SERVICE_1_IS_EXISTED, code)
                    });
                }

                return BadRequest(new
                {
                    Result = default(object),
                    Messages = CustomerConstants.REQUEST_INVALID
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update a customer information
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCustomer")]
        public async Task<object> UpdateCustomer(UpdateCustomerDto req)
        {
            try
            {
                int customerID = req.CustomerID;
                string code = req.Code;
                string fullName = req.FullName;

                if (!String.IsNullOrEmpty(code) && !String.IsNullOrEmpty(fullName))
                {
                    Customer customer = await customerBo.GetCustomer(customerID);

                    if (customer != null)
                    {
                        var result = await customerBo.UpdateCustomer(customer, req);

                        return Ok(new
                        {
                            Result = result,
                            Messages = result == null ? StatusConstants.UPDATE_FAIL : StatusConstants.SUCCESS
                        });
                    }

                    return NotFound();
                }

                return BadRequest(new
                {
                    Result = default(object),
                    Messages = CustomerConstants.REQUEST_INVALID
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Find and delete customer by CustomerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteByID/{customerID}")]
        public async Task<object> DeleteByID(int customerID)
        {
            try
            {
                var result = await customerBo.DeleteByID(customerID);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Find and delete customers by list customerID
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteMultiRow")]
        public async Task<object> DeleteMultiRow(DeleteCustomerDto req)
        {
            try
            {
                var result = await customerBo.DeleteMultiRow(req);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }
    }
}
