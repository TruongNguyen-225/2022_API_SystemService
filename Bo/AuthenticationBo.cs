using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Dto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Helpers;
using SystemServiceAPICore3.Bo.Interface;
using SystemServiceAPICore3.Dto.Other;

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
    }
}
