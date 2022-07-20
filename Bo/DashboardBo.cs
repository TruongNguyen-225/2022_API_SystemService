using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Helpers;

namespace SystemServiceAPI.Bo
{
    public class DashboardBo : IDashboard
    {
        private readonly AppDbContext _dbContext;
        public DashboardBo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<object> GetValueDashboard(int month)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                int countTransaction = _dbContext.vw_MonthlyTransactions.Where(x => x.Month == month && x.Year == DateTime.Now.Year).Count();
                int currentCost = _dbContext.vw_MonthlyTransactions.Where(x => x.Month == month && x.Year == DateTime.Now.Year).Select(X => X.Money).Sum();
                int currentRecive = _dbContext.vw_MonthlyTransactions.Where(x => x.Month == month && x.Year == DateTime.Now.Year).Select(X => X.Total).Sum();
                int currentProfit = _dbContext.vw_MonthlyTransactions.Where(x => x.Month == month && x.Year == DateTime.Now.Year).Select(X => X.Postage).Sum();
                int totalBudget = _dbContext.vw_MonthlyTransactions.Select(X => X.Postage).Sum();
                int countMonthDone = _dbContext.vw_MonthlyTransactions.GroupBy(x => x.Month).Select(x=>x.Key).Count();

                Dictionary<string, int> result = new Dictionary<string, int>();
                result.Add("CountTransaction", countTransaction);
                result.Add("CurrentCost", currentCost);
                result.Add("CurrentRecive", currentRecive);
                result.Add("CurrentProfit", currentProfit);
                result.Add("TotalBudget", totalBudget);
                result.Add("CountMonthDone", countMonthDone);

                response.Code = (int)HttpStatusCode.OK;
                response.Result = result;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = "ERROR";
            }

            return await Task.FromResult(response);
        }

        public async Task<object> GetBarChart(int take)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var data = _dbContext.vw_DataBarCharts.OrderByDescending(x => x.Year).OrderByDescending(x => x.Month).Take(take); 

                response.Code = (int)HttpStatusCode.OK;
                response.Result = data;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = "ERROR";
            }

            return await Task.FromResult(response);
        }

        public async Task<object> GetPieChart(int month)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var data = _dbContext.vw_PieCharts.Where(x => x.Month == month && x.Year == DateTime.Now.Year).OrderBy(x => x.RetailID).FirstOrDefault();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = data;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = "ERROR";
            }

            return await Task.FromResult(response);
        }

        public async Task<object> GetPieChartService(int month)
        {
            ResponseResults response = new ResponseResults();

            try
            {
                var data = _dbContext.vw_PieChartServices.Where(x => x.Month == month && x.Year == DateTime.Now.Year).OrderBy(x => x.ServiceID).FirstOrDefault();

                response.Code = (int)HttpStatusCode.OK;
                response.Result = data;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Msg = "ERROR";
            }

            return await Task.FromResult(response);
        }
    }
}
