using System;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Context;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Bo.Interface;

namespace SystemServiceAPICore3.Bo
{
    public class AuthenticationBo : IAuthenticationBo
    {
        private readonly AppDbContext dbContext;
        public AuthenticationBo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Account> GetUser(string username, string password)
        {
            var account = dbContext.Accounts.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
            return await Task.FromResult(account);
        }

        public void UpdateRefreshToken(Account account)
        {
            account.LastLogin = DateTime.Now;
            account.DateTimeAdd = DateTime.Now;

            Account entity = dbContext.Accounts.Update(account).Entity;
            dbContext.SaveChangesAsync();
        }

        public async Task<Account> GetUserByRefreshToke(string refreshToken, string username)
        {
            Account account = dbContext.Accounts.Where(x => x.RefreshToken == refreshToken  && x.UserName ==  username).FirstOrDefault();

            return await Task.FromResult(account);
        }
    }
}
