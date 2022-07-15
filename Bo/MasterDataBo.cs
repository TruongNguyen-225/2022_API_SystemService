using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Helpers;

namespace SystemServiceAPI.Bo
{
    public class MasterDataBo : IMasterData
    {
        private readonly AppDbContext _dbContext;
        public MasterDataBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<ResponseResults> GetServices()
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var data = _dbContext.Services.ToList();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = data;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.OK;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }

        public async Task<ResponseResults> GetRetails()
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var data = _dbContext.Retails.ToList();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = data;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }

        public async Task<ResponseResults> GetBanks()
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var data = _dbContext.Banks.ToList();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = data;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.OK;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }

        public async Task<ResponseResults> GetVillages()
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var data = _dbContext.Villages.ToList();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = data;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.OK;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }
    }
}
