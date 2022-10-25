using System.Collections.Generic;
using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.CustomerID;
using SystemServiceAPI.Entities.View;

namespace SystemServiceAPI.Bo.Interface
{
    public interface ICustomer
    {
        Task<object> GetCustomerByID(int customerID);
        Task<object> GetCustomerByServiceID(int serviceID);
        Task<List<vw_Customer>> GetByCondition(CustomerRequestDto req);
        Task<object> Post(AddCustomerDto req);
        Task<ResponseResults> Put(UpdateCustomerDto req);
        Task<ResponseResults> DeleteByID(int customerID);
        Task<ResponseResults> DeleteMultiRow(DeleteCustomerDto req);
        //Task<ResponseResults> FindByCondition(BillSearchDto req);
    }
}
