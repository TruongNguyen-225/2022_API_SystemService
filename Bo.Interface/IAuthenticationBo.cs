using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Dto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Dto.Other;

namespace SystemServiceAPICore3.Bo.Interface
{
    public interface IAuthenticationBo
    {
        Task<Account> GetUser(string username, string password);
        void UpdateRefreshToken(Account account);
    }
}
