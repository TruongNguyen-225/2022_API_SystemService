using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.CustomerID;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;
using SystemServiceAPICore3.Dto.Other;

namespace SystemServiceAPI.Bo.Interface
{
    public interface ICustomer
    {
        IQueryable<CustomerResponse> GetQueryableViewCustomer();

        Task<object> GetCustomerByID(int customerID);

        Task<Customer> GetCustomer(int customerID);

        Task<object> GetCustomerByServiceID(int serviceID);

        Task<object> GetByCondition(CustomerRequestDto req);

        Task<bool> CheckCustomerIsExist(string code, int serviceID, int retailID);

        Task<object> InsertCustomer(AddCustomerDto req);

        Task<object> UpdateCustomer(Customer customer, UpdateCustomerDto req);

        Task<object> DeleteByID(int customerID);

        Task<object> DeleteMultiRow(DeleteCustomerDto req);

        //Task<ResponseResults> FindByCondition(BillSearchDto req);
    }
}
