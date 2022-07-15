using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BillDto;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class BillController : ControllerBase
    {
        private readonly IBillBo _billBo;
        public BillController(IBillBo billBo)
        {
            _billBo = billBo;
        }

        [HttpGet]
        [Route("GetByServiceID/{serviceID}")]
        public async Task<object> GetByServiceID(int serviceID)
        {
            return await _billBo.GetByServiceID(serviceID);
        }

        [HttpPost]
        [Route("GetByMonth")]
        public async Task<object> GetByMonth(BillFilterDto req)
        {
            return await _billBo.GetByMonth(req);
        }

        [HttpPost]
        [Route("Post")]
        public async Task<object> Post(BillRequestDto req)
        {
            return await _billBo.Post(req);
        }

        [HttpPost]
        [Route("Put")]
        public async Task<object> Put(BillUpdatetDto req)
        {
            return await _billBo.Put(req);
        }

        [HttpPost]
        [Route("DeleteByID/{billID}")]
        public async Task<object> DeleteByID(int billID)
        {
            return await _billBo.DeleteByID(billID);
        }

        [HttpPost]
        [Route("DeleteMultiRow")]
        public async Task<object> DeleteMultiRow(BillDeleteDto req)
        {
            return await _billBo.DeleteMultiRow(req);
        }

        [HttpPost]
        [Route("Print/{billID}")]
        public async Task<object> Print(int billID)
        {
            return await _billBo.Print(billID);
        }

        [HttpPost]
        [Route("PrintAll")]
        public async Task<object> PrintAll(int serviceID)
        {
            return await _billBo.PrintAll(serviceID);
        }
    }
}

