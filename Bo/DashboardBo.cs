using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Bo;
using SystemServiceAPICore3.Dto;

namespace SystemServiceAPI.Bo
{
    public class DashboardBo : BaseBo<CustomerDto, Customer>, IDashboard
    {
        #region --- Verriable ---

        private readonly IBillBo billBo;

        #endregion

        #region --- Constructor ---

        public DashboardBo(IServiceProvider serviceProvider, IBillBo billBo) : base(serviceProvider)
        {
            this.billBo = billBo;
        }

        #endregion

        #region  --- Implements 


        #endregion

        public async Task<object> GetValueDashboard(int? month, int? year)
        {
            var vwMonthlyTransaction = billBo.GetQueryableViewMonthlyTransaction(month, year);
            int countTransaction = vwMonthlyTransaction.Count();
            int currentCost = (int)vwMonthlyTransaction.Select(x => x.Money).Sum();
            int currentRecive = (int)vwMonthlyTransaction.Select(x => x.Total).Sum();
            int currentProfit = (int)vwMonthlyTransaction.Select(x => x.Postage).Sum();
            int totalBudget = (int)vwMonthlyTransaction.Select(x => x.Postage).Sum();
            int countMonthDone = vwMonthlyTransaction.GroupBy(x => x.Month).Select(x => x.Key).Count();

            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("CountTransaction", countTransaction);
            result.Add("CurrentCost", currentCost);
            result.Add("CurrentRecive", currentRecive);
            result.Add("CurrentProfit", currentProfit);
            result.Add("TotalBudget", totalBudget);
            result.Add("CountMonthDone", countMonthDone);

            return await Task.FromResult(result);
        }


        public async Task<object> GetPieChart(int? month, int? year)
        {
            var vwMonthlyTransaction = billBo.GetQueryableViewMonthlyTransaction(month, year);
            var tblRetail = GetQueryable<Retail>();

            var vwPieChart = from x in vwMonthlyTransaction
                             from r in tblRetail.Where(r => r.RetailID == x.RetailID).DefaultIfEmpty()
                             group x by new
                             {
                                 x.RetailID,
                                 x.RetailName,
                                 x.Month,
                                 x.Year
                             } into gcs
                             select new
                             {
                                 RetailID = gcs.Key.RetailID,
                                 RetailName = gcs.Key.RetailName,
                                 Money = vwMonthlyTransaction.Select(x => x.Money).Sum(),
                                 Postage = vwMonthlyTransaction.Select(x => x.Postage).Sum(),
                                 Total = vwMonthlyTransaction.Select(x => x.Total).Sum(),
                                 Month = gcs.Key.Month,
                                 Year = gcs.Key.Year,
                                 Time = gcs.Key.Month > 10 ? gcs.Key.Month.ToString() + "/" + gcs.Key.Year : "0" + gcs.Key.Month.ToString() + "/" + gcs.Key.Year
                             };

            return await Task.FromResult(vwPieChart);
        }

        public async Task<object> GetBarChart(int take, int? month, int? year)
        {
            var vwMonthlyTransaction = billBo.GetQueryableViewMonthlyTransaction(month, year);
            var tblRetail = GetQueryable<Retail>();
            var vwPieChart = GetPieChart(month, year);
            //TODO

            return await Task.FromResult(default(object));
        }

        //public async Task<object> GetPieChartService(int month)
        //{
        //    ResponseResults response = new ResponseResults();

        //    try
        //    {
        //        var data = _dbContext.vw_PieChartServices.Where(x => x.Month == month && x.Year == DateTime.Now.Year).OrderBy(x => x.ServiceID).FirstOrDefault();

        //        response.Code = (int)HttpStatusCode.OK;
        //        response.Result = data;
        //        response.Msg = "SUCCESS";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Code = (int)HttpStatusCode.InternalServerError;
        //        response.Result = null;
        //        response.Msg = "ERROR";
        //    }

        //    return await Task.FromResult(response);
        //}
    }
}
