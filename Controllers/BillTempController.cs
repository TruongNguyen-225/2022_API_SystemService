using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPICore3.Utilities.Constants;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class BillTempController : ControllerBase
    {
        private readonly IBillTempBo billTempBo;
        public BillTempController(IBillTempBo billBo)
        {
            this.billTempBo = billBo;
        }

        /// <summary>
        /// Tạo dữ liệu tạm cho tháng hiện tại từ tháng trước
        /// Đưa vào tháng trước đó
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("InitData/{month}")]
        public object InitData(int month)
        {
            try
            {
                var queryable = billTempBo.InitData(month);

                return Ok(new
                {
                    Result = queryable,
                    Messages = queryable == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Load data temp binding table
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDataTempByMonth")]
        public object GetDataTempByMonth()
        {
            try
            {
                var queryable = billTempBo.GetDataTempByMonth(null);

                return Ok(new
                {
                    Result = queryable?.ToList(),
                    Messages = queryable == null ? StatusConstants.NOT_FOUND : StatusConstants.SUCCESS
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Thêm mới giao dịch tạm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertTransaction")]
        public async Task<object> InsertTransactionAsync(BillInsertDto request)
        {
            int serviceID = request.ServiceID;
            string code = request.Code;

            if (serviceID == 1)
            {
                bool isExisted = await billTempBo.CheckBeforeAddBillElectricity(serviceID, code);

                if (isExisted)
                {
                    return Ok(new
                    {
                        Result = default(object),
                        Messages = String.Format(StatusConstants.IS_EXISTED, "Giao dịch ")
                    });
                }
            }

            var result = await billTempBo.InsertTransactionAsync(request);

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
        public object UpdateTransactionAsync(BillUpdateDto req)
        {
            try
            {
                var result = billTempBo.UpdateTransactionAsync(req);

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
                var result = await billTempBo.DeleteTransactionByBillID(billID);

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
                var result = await billTempBo.DeleteTransactionsAsync(request);

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
                var result = billTempBo.PrintMultiRow(request);

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
        /// In tất cả danh sách khách hàng tiền điện đầu tháng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("PrintBillElectricInit")]
        public async Task<object> PrintBillElectricInit()
        {
            try
            {
                var result = await billTempBo.PrintBillElectricInit();

                if (result != null)
                {
                    return File(result.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }

                return BadRequest();
            }
            catch
            {
                throw;
            }
        }
    }
}

