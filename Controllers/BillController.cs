using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        [Route("GetTransactionByConditions")]
        public object GetTransactionByConditions(BillFilterDto req)
        {
            try
            {
                var result = billBo.GetTransactionByConditions(req);

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
        public object UpdateTransactionAsync(BillUpdateDto req)
        {
            try
            {
                var result = billBo.UpdateTransactionAsync(req);

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

        /// <summary>
        /// In một hóa đơn
        /// </summary>
        /// <param name="billID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PrintTransaction/{billID}")]
        public async Task<FileResult> PrintTransactionAsync(int billID)
        {
            try
            {
                var result = await billBo.Print(billID);
                if (result != null)
                {
                    //TODO
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
        /// Chưa cần thiết, chức năng này sẽ dùng trong billTempController -> in đầu tháng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PrintTransactions")]
        public async Task<object> PrintTransactionsAsync(/*BillPrintTransactionsDto request*/)
        {
            try
            {
                //BillPrintTransactionsDto request = new BillPrintTransactionsDto
                //{
                //    ServiceID = 1,
                //    ListBillID = "4476;4477;4478;4480;4483;4484;4485;4486;4487;4488;4489;4490;4491;4492;4493;4494;4495;4496;4497;4498;4499;4500;4501;4502;4503;4504;4505;4506;4507;4508;4509;4510;4511;4512;4513;4514;4515;4516;4517;4518;4519;4520;4521;4522;4523;4524;4525;4526;4527;4528;4529;4530;4531;4532;4533;4534;4535;4536;4537",
                //    Month = 1,
                //    Year = 2023
                //};

                //var result = billBo.PrintMultiRow(request);

                //if (result != null)
                //{
                //    return File(result.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                //}

                return Ok(new
                {
                    Result = default(object),
                    Messages = StatusConstants.NOT_FOUND
                });
            }
            catch
            {
                throw;
            }
        }
    }
}