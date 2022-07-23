using System.Threading.Tasks;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.CustomerID;

namespace SystemServiceAPI.Bo.Interface
{
    public interface ICustomer
    {
        Task<ResponseResults> GetCustomerByID(int customerID);
        Task<ResponseResults> GetCustomerByServiceID(int serviceID);
        Task<ResponseResults> GetByCondition(CustomerRequestDto req);
        Task<ResponseResults> Post(AddCustomerDto req);
        Task<ResponseResults> Put(UpdateCustomerDto req);
        Task<ResponseResults> DeleteByID(int customerID);
        Task<ResponseResults> DeleteMultiRow(DeleteCustomerDto req);
        //Task<ResponseResults> FindByCondition(BillSearchDto req);
    }
}
