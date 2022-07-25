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
    public class BillTempController : ControllerBase
    {
        private readonly IBillTempBo _billBo;
        public BillTempController(IBillTempBo billBo)
        {
            _billBo = billBo;
        }

        [HttpGet]
        [Route("GetData")]
        public async Task<object> GetData()
        {
            return await _billBo.GetData();
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

        [HttpDelete]
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
        [Route("PrintMultiRow")]
        public async Task<object> PrintMultiRow(BillPrintAllDto req)
        {
            return await _billBo.PrintMultiRow(req);
        }

        [HttpPost]
        [Route("Import")]
        public async Task<object> Import([FromBody] int month)
        {
            return await _billBo.Import(month);
        }
    }
}

