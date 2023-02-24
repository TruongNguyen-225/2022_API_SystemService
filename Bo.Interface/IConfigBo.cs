using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Entities.Table;

namespace SystemServiceAPI.Bo.Interface
{
    public interface IConfigBo
    {
        Task<object> GetScreen();

        Task<object> GetLevel();

        #region ---- configPrice ----

        Task<object> GetConfigPrice();

        Task<object> AddConfigPrice(ConfigPrice entity);

        Task<object> UpdateConfigPrice(int cofigPriceID, int postage);

        #endregion

        #region ---- Account ----

        Task<object> GetAccount();

        Task<object> GetAccountByID(int accountID);

        Task<object> UpdateAccount(Account entity);

        Task<object> DeleteAccount(int accountID);

        #endregion

        #region ---- File Upload ----

        Task<object> GetFileUpload();

        Task<object> UploadFile(IFormFile file);

        Task<object> AddFileUpload(FileUpload entity);

        Task<object> UpdateFileUpload(int fileUploadID, IFormFile file);

        #endregion
    }
}

