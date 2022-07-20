using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Dto.Report;
using SystemServiceAPI.Entities.View;
using SystemServiceAPI.Helpers;

namespace SystemServiceAPI.Bo
{
    public class ReportBo :IReport
    {
        private readonly AppDbContext _dbContext;
        public ReportBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<ResponseResults> GetByCondition(ReportRequestDto req)
        {
            ResponseResults response = new ResponseResults();
            try
            {
                List<vw_MonthlyTransaction> data = new List<vw_MonthlyTransaction>();

                if(req.ServiceID > 6)
                {
                    data = _dbContext.vw_MonthlyTransactions.Where(
                        x => x.RetailID == req.RetailID &&
                        x.DateTimeAdd >= req.StartTime &&
                        x.DateTimeAdd <= req.EndTime
                    ).OrderBy(x => x.ServiceID).OrderBy(x => x.DateTimeAdd).ToList();
                }
                else
                {
                    data = _dbContext.vw_MonthlyTransactions.Where(
                        x => x.ServiceID == req.ServiceID &&
                        x.RetailID == req.RetailID &&
                        x.DateTimeAdd >= req.StartTime &&
                        x.DateTimeAdd <= req.EndTime
                    ).OrderBy(x => x.ServiceID).OrderBy(x => x.DateTimeAdd).ToList();
                }
                

                if(data.Count > 0)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Result = data;
                    response.Msg = "SUCCESS";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Result = null;
                    response.Msg = "NOT FOUND";
                }

            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = ex.Message;
            }

            return await Task.FromResult(response);
        }
    }
}
