using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.CustomerID;
using SystemServiceAPICore3.Controllers;
using SystemServiceAPICore3.Dto;

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
            return await customerBo.GetCustomerByID(customerID);
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
            return await customerBo.GetCustomerByServiceID(serviceID);
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
            return await customerBo.GetByCondition(req);
        }

        /// <summary>
        /// Create new a customer information
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Post")]
        public async Task<object> Post(AddCustomerDto req)
        {
            return await customerBo.Post(req);
        }

        /// <summary>
        /// Update a customer information
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Put")]
        public async Task<object> Put(UpdateCustomerDto req)
        {
            return await customerBo.Put(req);
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
            return await customerBo.DeleteByID(customerID);
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
            return await customerBo.DeleteMultiRow(req);
        }
    }
}
