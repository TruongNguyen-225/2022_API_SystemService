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
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterData _masterBo;
        public MasterDataController(IMasterData masterBo)
        {
            _masterBo = masterBo;
        }

        [HttpGet]
        [Route("GetServices")]
        public async Task<object> GetServices()
        {
            return await _masterBo.GetServices();
        }

        [HttpGet]
        [Route("GetRetails")]
        public async Task<object> GetRetails()
        {
            return await _masterBo.GetRetails();
        }

        [HttpGet]
        [Route("GetBanks")]
        public async Task<object> GetBanks()
        {
            return await _masterBo.GetBanks();
        }

        [HttpGet]
        [Route("GetVillages")]
        public async Task<object> GetVillages()
        {
            return await _masterBo.GetVillages();
        }
    }
}

