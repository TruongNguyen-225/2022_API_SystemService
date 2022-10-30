using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPICore3.Controllers;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Utilities.Constants;
namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class BillController : BaseController<MonthlyTransactionDto>
    {
        #region -- Variables --

        private readonly IBillBo billBo;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public BillController(IServiceProvider serviceProvider, IBillBo bilBo)
            : base(serviceProvider)
        {
            this.billBo = bilBo;
        }

        #endregion

        /// <summary>
        /// Danh sách giao dịch theo dịch vụ
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransactionByServiceID/{serviceID}")]
        public async Task<object> GetTransactionByServiceID(int serviceID)
        {
            try
            {
                var result = await billBo.GetTransactionByServiceID(serviceID);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Danh sách giao dịch theo điều kiện
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTransactionByMonth")]
        public async Task<object> GetTransactionByMonthAsync(BillFilterDto req)
        {
            try
            {
                var result = await billBo.GetTransactionByMonth(req);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("InsertTransaction")]
        public async Task<object> InsertTransactionAsync(BillInsertDto request)
        {
            int serviceID = request.ServiceID;
            string code = request.Code;

            if (serviceID == 1)
            {
                bool isExisted = await billBo.CheckBeforeAddBillElectricity(serviceID, code);

                if (isExisted)
                {
                    return Ok(new
                    {
                        Result = default(object),
                        Messages = String.Format(StatusConstants.IS_EXISTED, "Giao dịch ")
                    });
                }
            }

            var result = await billBo.InsertTransactionAsync(request);

            return Ok(new
            {
                Result = result,
                Messages = result == null ? StatusConstants.UPDATE_FAIL : StatusConstants.SUCCESS
            });
        }

        /// <summary>
        /// Chỉnh sửa giao dịch
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateTransaction")]
        public async Task<object> UpdateTransactionAsync(BillUpdateDto req)
        {
            try
            {
                var result = await billBo.UpdateTransactionAsync(req);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Xoá giao dịch bới billID
        /// </summary>
        /// <param name="billID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteTransactionByID/{billID}")]
        public async Task<object> DeleteTransactionByIDAsync(int billID)
        {
            try
            {
                var result = await billBo.DeleteTransactionByBillID(billID);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Xoá nhiều giao dịch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteTransactions")]
        public async Task<object> DeleteTransactionsAsync(BillDeleteDto request)
        {
            try
            {
                var result = await billBo.DeleteTransactionsAsync(request);

                return Ok(new
                {
                    Result = result,
                    Messages = result == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("PrintTransaction/{billID}")]
        public async Task<FileResult> PrintTransactionAsync(int billID)
        {
            try
            {
                var result = await billBo.Print(billID);
                if (result != null)
                {
                    return File(result.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// In nhiều giao dịch cùng lúc
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PrintTransactions")]
        public async Task<object> PrintTransactionsAsync(BillPrintTransactionsDto request)
        {
            try
            {
                var result = await billBo.PrintMultiRow(request);

                //if (result != null)
                //{
                //    return File(result.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                //}

                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}