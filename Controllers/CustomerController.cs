using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.CustomerID;

namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customerBo;
        public CustomerController(ICustomer customerBo)
        {
            _customerBo = customerBo;
        }
        /// <summary>
        /// Get information customer by CustomerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomerByID/{customerID}")]
        public async Task<object> GetCustomerByID(int customerID)
        {
            return await _customerBo.GetCustomerByID(customerID);
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
            return await _customerBo.GetByCondition(req);
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
            return await _customerBo.Post(req);
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
            return await _customerBo.Put(req);
        }

        /// <summary>
        /// Find and delete customer by CustomerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteByID/{customerID}")]
        public async Task<object> DeleteByID(int customerID)
        {
            return await _customerBo.DeleteByID(customerID);
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
            return await _customerBo.DeleteMultiRow(req);
        }
    }
}
