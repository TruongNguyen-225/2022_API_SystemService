using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Bo;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Dto.Other;
using SystemServiceAPICore3.Entities.Table;
using SystemServiceAPICore3.Utilities;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPI.Bo
{
    public class ConfigBo : BaseBo<ConfigPriceDto, ConfigPrice>, IConfigBo
    {
        #region -- Variables --
        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        public ConfigBo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #endregion

        #region -- Overrides --
        #endregion

        public async Task<object> GetConfigPrice()
        {
            var configPriceQueryable = GetQueryable<ConfigPrice>();
            var serviceQueryable = GetQueryable<Service>();
            var levelQueryable = GetQueryable<Level>();

            var queryable = from config in configPriceQueryable
                            from service in serviceQueryable.Where(x => x.ServiceID == config.ServiceID).DefaultIfEmpty()
                            from level in levelQueryable.Where(x => x.ID == config.LevelID).DefaultIfEmpty()
                            select new
                            {
                                ID = config.ConfigID,
                                ServiceID = config.ServiceID,
                                ServiceName = service.ServiceName,
                                LevelID = config.LevelID,
                                TransactionLimit = level.TransactionLimit,
                                Postage = config.Postage
                            };

            if (queryable.Any())
            {
                return await Task.FromResult(queryable.ToList());
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> GetScreen()
        {
            var screenQueryable = GetQueryable<Screen>();
            var queryable = from screen in screenQueryable
                            select new
                            {
                                ID = screen.ID,
                                ScreenName = screen.ScreenName,
                                Describe = screen.Describe
                            };

            if (queryable.Any())
            {
                return await Task.FromResult(queryable.ToList());
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> GetLevel()
        {
            var levelQueryable = GetQueryable<Level>();
            var queryable = from level in levelQueryable
                            select new
                            {
                                ID = level.ID,
                                TransactionLimit = level.TransactionLimit
                            };

            if (queryable.Any())
            {
                return await Task.FromResult(queryable.ToList());
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> GetFileUpload()
        {
            var fileUploadQueryable = GetQueryable<FileUpload>();
            var screenQueryable = GetQueryable<Screen>();


            var queryable = from file in fileUploadQueryable
                            from screen in screenQueryable.Where(x => x.ID == file.ScreenID).DefaultIfEmpty()
                            select new
                            {
                                ID = file.ID,
                                ScreenID = file.ScreenID,
                                ScreenName = screen.ScreenName,
                                Describe = screen.Describe,
                                FileName = file.FileName,
                                FileSize = file.FileSize,
                                FilePath = file.FilePath,
                                FileType = file.FileType,
                                DateTimeAdd = file.DateTimeAdd,
                                DateTimeUpdate = file.DateTimeUpdate,
                            };

            if (queryable.Any())
            {
                return await Task.FromResult(queryable.ToList());
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> AddConfigPrice(ConfigPrice entity)
        {
            var configPriceRespository = GetRepository<ConfigPrice>();

            if (entity != null)
            {
                await configPriceRespository.InsertAsync(entity);
                await configPriceRespository.SaveAsync();

                return await Task.FromResult(entity);
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> UpdateConfigPrice(int cofigPriceID, int postage)
        {
            var configPriceQueryable = GetQueryable<ConfigPrice>();
            var configPriceRespository = GetRepository<ConfigPrice>();

            var entity = configPriceQueryable.Where(x => x.ConfigID == cofigPriceID).FirstOrDefault();

            if (entity != null)
            {
                entity.Postage = postage;

                await configPriceRespository.UpdateAsync(entity);
                await configPriceRespository.SaveAsync();

                return await Task.FromResult(entity);
            }

            return await Task.FromResult(default(object));
        }

        public Task<object> AddFileUpload(FileUpload entity)
        {
            throw new NotImplementedException();
        }

        public Task<object> UpdateFileUpload(int fileUploadID, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<object> UploadFile(IFormFile file)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var bytes = ms.ToArray();

            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(bytes);

            string hostAddress = @"https://pss.itdvgroup.com/template";
            string localAddress = @"https://localhost:5001";

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

            // here it is important that second parameter matches with name given in API.
            form.Add(content: fileContent, name: "file", fileName: file.FileName);

            var httpClient = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(hostAddress)
            };

            var response = await httpClient.PostAsync("", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<object> GetAccount()
        {
            var accountQueryable = GetQueryable<Account>();
            var queryable = accountQueryable.Where(x => x.Active);

            if (queryable.Any())
            {
                return await Task.FromResult(queryable.ToList());
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> GetAccountByID(int accountID)
        {
            var accountQueryable = GetQueryable<Account>();
            var account = accountQueryable.Where(x => x.Active && x.AccountID == accountID).FirstOrDefault();

            return await Task.FromResult(account);
        }

        public async Task<object> UpdateAccount(Account entity)
        {
            var accountRepository = GetRepository<Account>();
            var accountID = entity.AccountID;

            //update
            if (accountID > 0)
            {
                var account = accountRepository.FindBy(x => x.AccountID == accountID && x.Active).FirstOrDefault();
                if (account != null)
                {
                    var destination = mapper.Map<Account>(entity);
                    await accountRepository.UpdateAsync(destination);
                    await accountRepository.SaveAsync();

                    return await Task.FromResult(destination);
                }

                return await Task.FromResult(default(object));
            }
            else
            {
                var result = await accountRepository.InsertAsync(entity);
                await accountRepository.SaveAsync();

                return await Task.FromResult(result);
            }
        }

        public async Task<object> DeleteAccount(int accountID)
        {
            var accountRepository = GetRepository<Account>();
            var account = accountRepository.FindBy(x => x.AccountID == accountID && x.Active).FirstOrDefault();

            if (account != null)
            {
                account.Active = false;

                var result = await accountRepository.UpdateAsync(account);
                await accountRepository.SaveAsync();

                return await Task.FromResult(result);
            }

            return await Task.FromResult(default(object));
        }
    }
}