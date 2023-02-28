using System.Threading.Tasks;
using SystemServiceAPI.Entities.Table;

namespace SystemServiceAPICore3.Bo.Interface
{
    public interface IAuthenticationBo
    {
        Task<Account> GetUser(string username, string password);

        void UpdateRefreshToken(Account account);

        Task<Account> GetUserByRefreshToke(string refreshToken, string username);
    }
}
